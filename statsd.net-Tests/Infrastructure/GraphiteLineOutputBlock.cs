﻿using statsd.net.core.Messages;
using statsd.net.shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statsd.net_Tests.Infrastructure
{
  public class GraphiteLineOutputBlock : OutputBufferBlock<GraphiteLine>
  {
    public double this[string key]
    {
      get { return this.Items.First(p => p.Name == key).Quantity; }
    }
  }
}
