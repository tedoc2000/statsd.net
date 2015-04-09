
using statsd.net.core.Structures;
namespace statsd.net.shared.Messages
{

  public abstract class TaggedStatsdMessage : StatsdMessage
  {
    public MetricTags Tags { get; private set; }
    public TaggedStatsdMessage(string[] tags)
    {
        Tags = new MetricTags(tags);
    }
  }
}
