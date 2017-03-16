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
    /// <summary>
    /// Communicates with <see cref="ISlaveService"/> and receives messages from 
    /// <see cref="MasterServiceCommunicator"/>
    /// </summary>
    public class SlaveServiceCommunicator : MarshalByRefObject, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Slave service to communicate with
        /// </summary>
        private ISlaveService slaveService;

        /// <summary>
        /// Connection to server to receive messages
        /// </summary>
        private TcpClient serverConnection;

        /// <summary>
        /// Server listener to wait for server's connection
        /// </summary>
        private TcpListener serverListener;

        /// <summary>
        /// Creates a new instance of <see cref="SlaveServiceCommunicator"/>
        /// </summary>
        /// <param name="slaveService"></param>
        /// <param name="slaveAddress"></param>
        /// <param name="slavePort"></param>
        /// <exception cref="ArgumentNullException">Throws when one of the parameters is null</exception>
        /// <exception cref="ArgumentException">Throws when <see cref="slavePort"/>
        ///  is less or equal to zero</exception>
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

            Logger.Log(LogLevel.Trace, $"{nameof(SlaveServiceCommunicator)} ctor started");
            this.slaveService = slaveService;
            serverListener = new TcpListener(slaveAddress, slavePort);
            Logger.Log(
                LogLevel.Trace, $"{nameof(SlaveServiceCommunicator)} serverListener task starting");
            ListenServer();
        }

        /// <summary>
        /// Closes connection to server
        /// </summary>
        ~SlaveServiceCommunicator()
        {
            serverConnection?.Close();
        }

        /// <summary>
        /// Closes connection to server
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            serverConnection?.Close();
        }

        /// <summary>
        /// Asynchronously listen for server connection and then for messages from server
        /// </summary>
        private async void ListenServer()
        {
            Logger.Log(LogLevel.Trace, "Server listener thread started");
            try
            {
                serverListener.Start();
                serverConnection = await serverListener.AcceptTcpClientAsync();
                NetworkStream stream = serverConnection.GetStream();
                BinaryFormatter formatter = new BinaryFormatter();
                while (true)
                {
                    CommunicationMessage message = (CommunicationMessage)formatter.Deserialize(stream);
                    Logger.Log(LogLevel.Info, $"{message} received");
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
                Logger.Log(LogLevel.Error, ex);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Trace, ex);
            }
        }
    }
}