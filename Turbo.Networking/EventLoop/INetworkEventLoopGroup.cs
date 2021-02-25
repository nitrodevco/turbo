using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Networking.EventLoop
{
    public interface INetworkEventLoopGroup
    {
        public IEventLoopGroup Group { get; }
    }
}
