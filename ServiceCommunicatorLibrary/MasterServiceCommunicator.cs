using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
    public class MasterServiceCommunicator : MarshalByRefObject, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IMasterService masterService;
        private BlockingCollection<TcpClient> slaves;
        private TcpListener server;
        private Thread acceptingThread;

        public MasterServiceCommunicator(
            IMasterService masterService, IPAddress address, int port)
        {
            if (masterService == null)
            {
                throw new ArgumentNullException($"{nameof(masterService)} is null");
            }

            if (address == null)
            {
                throw new ArgumentNullException($"{nameof(address)} is null");
            }

            if (port <= 0)
            {
                throw new ArgumentException($"{nameof(port)} must be positive");
            }

            logger.Log(LogLevel.Trace, $"{nameof(MasterServiceCommunicator)} ctor started");
            this.masterService = masterService;
            server = new TcpListener(address, port);
            slaves = new BlockingCollection<TcpClient>();
            SubscribeToMaster();
            acceptingThread = new Thread(ListenForConnections);
            logger.Log(
                LogLevel.Trace, $"{nameof(MasterServiceCommunicator)} listening thread starting...");
            acceptingThread.Start();
        }

        ~MasterServiceCommunicator()
        {
            server?.Stop();
            foreach (TcpClient slave in slaves)
            {
                slave?.Close();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            server?.Stop();
            foreach (TcpClient slave in slaves)
            {
                slave.Close();
            }
        }

        private void SubscribeToMaster()
        {
            masterService.UserAdded += UserAddedHandler;
            masterService.UserRemoved += UserRemovedHandler;
        }

        private void UserAddedHandler(object sender, UserEventArgs args)
        {
            SendUserChanged(args);
        }

        private void UserRemovedHandler(object sender, UserEventArgs args)
        {
        }

        private void ListenForConnections()
        {
            logger.Log(
                LogLevel.Trace, $"{nameof(MasterServiceCommunicator)} listening thread started");
            server.Start();
            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    logger.Log(LogLevel.Info, "New client accepted");
                    slaves.Add(client);
                }
            }
            catch (SocketException ex)
            {
                logger.Log(LogLevel.Error, ex);
            }
        }

        private Task SendUserChanged(UserEventArgs userArgs)
        {
            logger.Log(LogLevel.Trace, $"{userArgs.User} sending to slaves");
            return Task.Factory.StartNew(() =>
            {
                foreach (TcpClient slave in slaves)
                {
                    NetworkStream stream = slave.GetStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, userArgs);
                }
            });
        }
    }
}
