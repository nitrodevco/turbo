namespace Turbo.Packets.Incoming.Handshake
{
    public class ClientHelloMessage : IMessageEvent
    {
        public string Production { get; }
        public string Platform { get; }
        public int ClientPlatform { get; }
        public int DeviceCategory { get; }

        public ClientHelloMessage(string production, string platform, int clientPlatform, int deviceCat)
        {
            this.Production = production;
            this.Platform = platform;
            this.ClientPlatform = clientPlatform;
            this.DeviceCategory = deviceCat;
        }
    }
}
