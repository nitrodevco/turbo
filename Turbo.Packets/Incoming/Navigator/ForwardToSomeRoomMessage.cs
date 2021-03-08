using Turbo.Core.Navigator.Enums;

namespace Turbo.Packets.Incoming.Navigator
{
    public record ForwardToSomeRoomMessage : IMessageEvent
    {
        /// <summary>
        /// Known navigator link events:
        /// 
        /// navigator/goto/home (handled clientside)
        /// navigator/goto/{ID} (handled clientside)
        /// navigator/goto/{string} (handled serverside)
        /// 
        /// ForwardData only contains the string data
        /// </summary>
        public string ForwardData { get; init; }
    }
}
