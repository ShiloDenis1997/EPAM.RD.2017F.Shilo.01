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
        private TcpListener serverListener;
        private IPAddress slaveAddress;
        private int slavePort;
        private Thread serverListenerThread;

        public SlaveServiceCommunicator(
            ISlaveService slaveService, IPAddress slaveAddress, int slavePort)
        {
            if (slaveService == null)
            {
                throw new ArgumentNullException($"{nameof(slaveService)} is null");
            }

            if (slaveAddress == null)
            {
                throw new ArgumentNullException($"{nameof(slaveAddress)} is null");
            }

            if (slavePort <= 0)
            {
                throw new ArgumentException($"{nameof(slavePort)} is less or equal to zero");
            }

            logger.Log(LogLevel.Trace, $"{nameof(SlaveServiceCommunicator)} ctor started");
            this.slaveService = slaveService;
            this.slaveAddress = slaveAddress;
            this.slavePort = slavePort;
            serverListener = new TcpListener(slaveAddress, slavePort);
            serverListenerThread = new Thread(ListenServer);
            logger.Log(
                LogLevel.Trace, $"{nameof(SlaveServiceCommunicator)} serverListener thread starting");
            serverListenerThread.Start();
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
            try
            {
                serverListener.Start();
                serverConnection = serverListener.AcceptTcpClient();
                NetworkStream stream = serverConnection.GetStream();
                BinaryFormatter formatter = new BinaryFormatter();
                while (true)
                {
                    CommunicationMessage message = (CommunicationMessage)formatter.Deserialize(stream);
                    logger.Log(LogLevel.Info, $"{message} received");
                    if (message.Operation == UserOperation.Add)
                    {
                        slaveService.UserAddedHandler(this, new UserEventArgs { User = message.User });
                    }
                    else
                    {
                        slaveService.UserRemovedHandler(this, new UserEventArgs { User = message.User });
                    }
                }
            }
            catch (SocketException ex)
            {
                logger.Log(LogLevel.Error, ex);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Trace, ex);
            }
        }
    }
}