using System.Net;

namespace ServiceCommunicatorLibrary
{
    public class TcpClientConfiguration
    {
        public IPAddress Address { get; set; }

        public int Port { get; set; }
    }
}