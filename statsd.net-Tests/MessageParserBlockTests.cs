using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using statsd.net.core;
using statsd.net.Framework;
using statsd.net.shared.Messages;
using statsd.net.shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using statsd.net;
using statsd.net.shared.Factories;
using log4net;
using statsd.net.core.Structures;
using System.Reflection;

namespace statsd.net_Tests
{
  [TestClass]
  public class MessageParserBlockTests
  {
    private TransformBlock<string, StatsdMessage> _block;
    private Mock<ISystemMetricsService> _systemMetrics;
    private Mock<ILog> _log;

    [TestInitialize]
    public void Initialise()
    {
      _systemMetrics = new Mock<ISystemMetricsService>();
      _log = new Mock<ILog>();
      _block = MessageParserBlockFactory.CreateMessageParserBlock(new CancellationToken(), 
        _systemMetrics.Object,
        _log.Object);
    }

    [TestMethod]
    public void ProcessedALine_IncrementedCounter()
    {
      _systemMetrics.Setup(p => p.LogCount("parser.linesSeen", 1)).Verifiable();

      _block.Post(new Counter("foo", 1).ToString());
      _block.WaitUntilAllItemsProcessed();

      _systemMetrics.VerifyAll();
    }

    [TestMethod]
    public void ProcessedABadLine_IncrementedBadLineCounter()
    {
      _systemMetrics.Setup(p => p.LogCount("parser.badLinesSeen", 1)).Verifiable();

      _block.Post("a bad line");
      _block.WaitUntilAllItemsProcessed();

      _systemMetrics.VerifyAll();
    }

    [TestMethod]
    public void ProcessedRawLine_GotValidRawMessageInstance()
    {
      var timestamp = DateTime.Now.Ticks;
      var metric = "a.raw.metric:100|r|" + timestamp;
      RunParseTest_NoTags(metric, typeof(Raw));
    }

    [TestMethod]
    public void ProcessedRawLine_NoTimeStamp_GotValidRawMessageInstance()
    {
      var metric = "a.raw.metric:100|r";
      RunParseTest_NoTags(metric, typeof(Raw));
    }

    [TestMethod]
    public void ProcessedRawLine_WithTags_GotValidRawMessageInstance()
    {
      var timestamp = DateTime.Now.Ticks;
      var metric = "a.raw.metric:100|r|" + timestamp;
      RunParseTest_WithTags(metric, typeof(Raw));
    }

    [TestMethod]
    public void ProcessedRawLine_NoTimeStampWithTags_GotValidRawMessageInstance()
    {
      var metric = "a.raw.metric:100|r";
      RunParseTest_WithTags(metric, typeof(Raw));
    }

    [TestMethod]
    public void ProcessedCounterLine_GotValidCounterMessageInstance()
    {
      var metric = "a.counter.metric:100|c|@0.1";
      RunParseTest_NoTags(metric, typeof(Counter));
    }

    [TestMethod]
    public void ProcessedCounterLine_NoSampleRate_GotValidCounterMessageInstance()
    {
      var metric = "a.counter.metric:100|c";
      RunParseTest_NoTags(metric, typeof(Counter));
    }

    [TestMethod]
    public void ProcessedCounterLine_WithTags_GotValidCounterMessageInstance()
    {
      var metric = "a.counter.metric:100|c|@0.1";
      RunParseTest_WithTags(metric, typeof(Counter));
    }

    [TestMethod]
    public void ProcessedCounterLine_NoSampleRateWithTags_GotValidCounterMessageInstance()
    {
      var metric = "a.counter.metric:100|c";
      RunParseTest_WithTags(metric, typeof(Counter));
    }

    [TestMethod]
    public void ProcessedTimingLine_GotValidTimingMessageInstance()
    {
      var metric = "a.timing.metric:320|ms";
      RunParseTest_NoTags(metric, typeof(Timing));
    }

    [TestMethod]
    public void ProcessedTimingLine_WithTags_GotValidTimingMessageInstance()
    {
      var metric = "a.timing.metric:320|ms";
      RunParseTest_WithTags(metric, typeof(Timing));
    }

    [TestMethod]
    public void ProcessedGaugeLine_GotValidGaugeMessageInstance()
    {
      var metric = "a.gauge.metric:333|g";
      RunParseTest_NoTags(metric, typeof(Gauge));
    }

    [TestMethod]
    public void ProcessedGaugeLine_WithTags_GotValidGaugeMessageInstance()
    {
      var metric = "a.gauge.metric:333|g";
      RunParseTest_WithTags(metric, typeof(Gauge));
    }

    [TestMethod]
    public void ProcessedSetLine_GotValidSetMessageInstance()
    {
      var metric = "a.set.metric:ABSA434As1|s";
      RunParseTest_NoTags(metric, typeof(Set));
    }

    [TestMethod]
    public void ProcessedSetLine_WithTags_GotValidSetMessageInstance()
    {
      var metric = "a.set.metric:765|s";
      RunParseTest_WithTags(metric, typeof(Set));
    }

    [TestMethod]
    public void ProcessedCalendarGramLine_GotValidCalendarGramMessageInstance()
    {
      var metric = "a.cg.metric:101|cg|h";
      RunParseTest_NoTags(metric, typeof(Calendargram));
    }

    [TestMethod]
    public void ProcessedCalendarGramLine_WithTags_GotValidCalendarGramMessageInstance()
    {
      var metric = "a.cg.metric:101|cg|h";
      RunParseTest_WithTags(metric, typeof(Calendargram));
    }

    protected void RunParseTest_NoTags(string metric, Type expectedType)
    {
      _systemMetrics.Setup(p => p.LogCount("parser.linesSeen", 1)).Verifiable();

      _block.Post(metric);
      var message = _block.Receive();

      Assert.IsInstanceOfType(message, expectedType);
      Assert.AreEqual(metric, message.ToString());
      _systemMetrics.VerifyAll();
    }

    protected void RunParseTest_WithTags(string metric, Type expectedType)
    {
      _systemMetrics.Setup(p => p.LogCount("parser.linesSeen", 1)).Verifiable();

      _block.Post(metric + "|#tags1=value,tags2");
      var message = _block.Receive();

      Assert.IsInstanceOfType(message, expectedType);
      Assert.AreEqual(metric, message.ToString());
      PropertyInfo prop = expectedType.GetProperty("Tags");
      Assert.AreEqual(prop.GetValue(message), 
          new MetricTags(new string[] { "tags1=value", "tags2" }));
      _systemMetrics.VerifyAll();
    }
  }
}
