using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statsd.net.shared.Messages
{
  public sealed class Timing : TaggedStatsdMessage
  {
    public double ValueMS { get; set; }

    public Timing(string name, double valueMS, string[] tags = null) : base(tags)
    {
      Name = name;
      ValueMS = valueMS;
      MessageType = MessageType.Timing;
    }

    public override string ToString()
    {
      return String.Format("{0}:{1}|ms", Name, ValueMS);
    }
  }
}
