
using Microsoft.VisualStudio.TestTools.UnitTesting;
using statsd.net.core.Structures;
namespace statsd.net_Tests
{
    [TestClass]
    public class MetricInfoTests
    {

        [TestMethod]
        public void GetHashCode_SameNameNoTags_SameHashCode()
        {
            MetricInfo m1 = new MetricInfo("foo");
            MetricInfo m2 = new MetricInfo("foo");

            Assert.AreEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_DifferentNameNoTags_DifferentHashCode()
        {
            MetricInfo m1 = new MetricInfo("foo");
            MetricInfo m2 = new MetricInfo("foo1231231");

            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_SameNameSameTagsDifferentOrder_SameHashCode()
        {
            MetricInfo m1 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag2" }));
            MetricInfo m2 = new MetricInfo("foo", new MetricTags(new string[] { "tag2", "tag1" }));

            Assert.AreEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_DifferentNameSameTags_DifferentHashCode()
        {
            MetricInfo m1 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag2" }));
            MetricInfo m2 = new MetricInfo("foo1", new MetricTags(new string[] { "tag1", "tag2" }));

            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_SameNameDifferentTags_DifferentHashCode()
        {
            MetricInfo m1 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag2" }));
            MetricInfo m2 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag3" }));

            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());
        }


        [TestMethod]
        public void Equals_SameNameNoTags_AreEquals()
        {
            MetricInfo m1 = new MetricInfo("foo");
            MetricInfo m2 = new MetricInfo("foo");

            Assert.IsTrue(m1.Equals(m2));
            Assert.IsTrue(m2.Equals(m1));
        }

        [TestMethod]
        public void Equals_DifferentNameNoTags_AreNotEquals()
        {
            MetricInfo m1 = new MetricInfo("foo");
            MetricInfo m2 = new MetricInfo("foo1231231");

            Assert.IsFalse(m1.Equals(m2));
            Assert.IsFalse(m2.Equals(m1));
        }

        [TestMethod]
        public void Equals_SameNameSameTagsDifferentOrder_AreEquals()
        {
            MetricInfo m1 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag2" }));
            MetricInfo m2 = new MetricInfo("foo", new MetricTags(new string[] { "tag2", "tag1" }));

            Assert.IsTrue(m1.Equals(m2));
            Assert.IsTrue(m2.Equals(m1));
        }

        [TestMethod]
        public void Equals_DifferentNameSameTags_AreNotEquals()
        {
            MetricInfo m1 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag2" }));
            MetricInfo m2 = new MetricInfo("foo1", new MetricTags(new string[] { "tag1", "tag2" }));

            Assert.IsFalse(m1.Equals(m2));
            Assert.IsFalse(m2.Equals(m1));
        }

        [TestMethod]
        public void Equals_SameNameDifferentTags_AreNotEquals()
        {
            MetricInfo m1 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag2" }));
            MetricInfo m2 = new MetricInfo("foo", new MetricTags(new string[] { "tag1", "tag3" }));

            Assert.IsFalse(m1.Equals(m2));
            Assert.IsFalse(m2.Equals(m1));
        }
    }
}
