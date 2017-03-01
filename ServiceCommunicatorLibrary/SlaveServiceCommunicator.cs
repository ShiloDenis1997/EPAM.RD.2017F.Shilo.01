using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using UserServiceLibrary;
using UserServiceLibrary.Interfaces;

namespace ServiceCommunicatorLibrary
{
    public class SlaveServiceCommunicator : MarshalByRefObject, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ISlaveService slaveService;
        private TcpClient serverConnection;
        private Thread serverListener;
        private IPAddress hostAddress;
        private int hostPort;

        public SlaveServiceCommunicator(
            ISlaveService slaveService, IPAddress hostAddress, int hostPort)
        {
            if (slaveService == null)
            {
                throw new ArgumentNullException($"{nameof(slaveService)} is null");
            }

            if (hostAddress == null)
            {
                throw new ArgumentNullException($"{nameof(hostAddress)} is null");
            }

            if (hostPort <= 0)
            {
                throw new ArgumentException($"{nameof(hostPort)} is less or equal to zero");
            }

            logger.Log(LogLevel.Trace, $"{nameof(SlaveServiceCommunicator)} ctor started");
            this.slaveService = slaveService;
            serverConnection = new TcpClient();
            this.hostAddress = hostAddress;
            this.hostPort = hostPort;
            serverListener = new Thread(ListenServer);
            logger.Log(
                LogLevel.Trace, $"{nameof(SlaveServiceCommunicator)} serverListener thread starting");
            serverListener.Start();
        }

        ~SlaveServiceCommunicator()
        {
            serverConnection?.Close();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            serverConnection?.Close();
        }
        
        private void ListenServer()
        {
            logger.Log(LogLevel.Trace, "Server listener thread started");
            serverConnection.Connect(hostAddress, hostPort);
            NetworkStream stream = serverConnection.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                while (true)
                {
                    UserEventArgs userArgs = (UserEventArgs)formatter.Deserialize(stream);
                    logger.Log(LogLevel.Info, $"{userArgs.User} received");
                    slaveService.UserAddedHandler(this, userArgs);
                }
            }
            catch (SocketException ex)
            {
                logger.Log(LogLevel.Error, ex);
            }
        }
    }
}