using Turbo.Packets.Incoming;
using Turbo.Packets.Sessions;

namespace Turbo.Packets
{
    public interface ICallable<T> where T : IMessageEvent
    {
        /// <summary>
        /// Executes callable logic and returns true to continue 
        /// normal execution of packet listenes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="session"></param>
        /// <returns>False if message listeners should be cancelled. True otherwise</returns>
        public bool Call(T message, ISession session);
    }
}
