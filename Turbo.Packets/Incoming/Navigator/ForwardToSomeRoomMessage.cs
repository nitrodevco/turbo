using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record ForwardToSomeRoomMessage : IMessageEvent
{
    /// <summary>
    ///     Known navigator link events:
    ///     <para>navigator/goto/home (handled clientside)</para>
    ///     <para>navigator/goto/{ID} (handled clientside)</para>
    ///     <para>navigator/goto/{string} (handled serverside)</para>
    ///     Known {string} values can be found in <see cref="Core.Game.Navigator.Constants.NavigatorRoomForwardType" />
    ///     ForwardData only contains the string data
    /// </summary>
    public string ForwardData { get; init; }
}