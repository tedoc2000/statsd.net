using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statsd.net.shared.Messages
{
  public sealed class Gauge : TaggedStatsdMessage
  {
    public double Value { get; set; }

    public Gauge(string name, double value, string[] tags = null) : base(tags)
    {
      Name = name;
      Value = value;
      MessageType = MessageType.Gauge;
    }

    public override string ToString()
    {
      return String.Format("{0}:{1}|g", Name, Value);
    }
  }
}
