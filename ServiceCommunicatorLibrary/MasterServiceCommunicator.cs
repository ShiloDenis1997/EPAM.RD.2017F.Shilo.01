using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public MasterServiceCommunicator(
            IMasterService masterService, IEnumerable<TcpClientConfiguration> slavesConfigurations)
        {
            if (masterService == null)
            {
                throw new ArgumentNullException($"{nameof(masterService)} is null");
            }

            if (slavesConfigurations == null)
            {
                throw new ArgumentNullException($"{nameof(slavesConfigurations)} is null");
            }

            logger.Log(LogLevel.Trace, $"{nameof(MasterServiceCommunicator)} ctor started");
            this.masterService = masterService;
            slaves = new BlockingCollection<TcpClient>();
            foreach (var slaveConfig in slavesConfigurations)
            {
                try
                {
                    TcpClient slave = new TcpClient();
                    slave.Connect(slaveConfig.Address, slaveConfig.Port);
                    slaves.Add(slave);
                }
                catch (Exception ex)
                {
                    logger.Log(
                        LogLevel.Warn, 
                        ex,
                        $"Cannot connect to {slaveConfig.Address}:{slaveConfig.Port}");
                }
            }

            SubscribeToMaster();
        }

        ~MasterServiceCommunicator()
        {
            foreach (TcpClient slave in slaves)
            {
                slave?.Close();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
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
            SendUserMessage(
                new CommunicationMessage { Operation = UserOperation.Add, User = args.User });
        }

        private void UserRemovedHandler(object sender, UserEventArgs args)
        {
            SendUserMessage(
                new CommunicationMessage { Operation = UserOperation.Remove, User = args.User });
        }

        private void SendUserMessage(CommunicationMessage message)
        {
            logger.Log(LogLevel.Trace, $"{message.User} sending to slaves");
            
            foreach (TcpClient slave in slaves)
            {
                NetworkStream stream = slave.GetStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, message);
            }
        }
    }
}
