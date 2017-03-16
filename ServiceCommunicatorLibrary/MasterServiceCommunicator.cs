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
    /// <summary>
    /// Communicates with <see cref="IMasterService"/> and sends messages 
    /// to <see cref="SlaveServiceCommunicator"s/>
    /// </summary>
    public class MasterServiceCommunicator : MarshalByRefObject, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Master service to communicate with
        /// </summary>
        private IMasterService masterService;

        /// <summary>
        /// Collection of slaves connections to which it will send messages
        /// </summary>
        private BlockingCollection<TcpClient> slaves;

        /// <summary>
        /// Creates a new instance of <see cref="MasterServiceCommunicator"/>. Tries to connect
        /// to slaves. 
        /// </summary>
        /// <param name="masterService">Master service to communicate with</param>
        /// <param name="slavesConfigurations">Configurations of slaves</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null</exception>
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

            Logger.Log(LogLevel.Trace, $"{nameof(MasterServiceCommunicator)} ctor started");
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
                    Logger.Log(
                        LogLevel.Warn, 
                        ex,
                        $"Cannot connect to {slaveConfig.Address}:{slaveConfig.Port}");
                }
            }

            SubscribeToMaster();
        }

        /// <summary>
        /// Closes all connections to slaves
        /// </summary>
        ~MasterServiceCommunicator()
        {
            foreach (TcpClient slave in slaves)
            {
                slave?.Close();
            }
        }

        /// <summary>
        /// Closes all connections to slaves
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (TcpClient slave in slaves)
            {
                slave.Close();
            }
        }

        /// <summary>
        /// Subscribes for master events.
        /// </summary>
        private void SubscribeToMaster()
        {
            masterService.UserAdded += UserAddedHandler;
            masterService.UserRemoved += UserRemovedHandler;
        }

        /// <summary>
        /// Handles user add action and invokes sending a message to slaves
        /// </summary>
        /// <param name="sender">Master</param>
        /// <param name="args">Added user</param>
        private void UserAddedHandler(object sender, UserEventArgs args)
        {
            SendUserMessage(
                new CommunicationMessage { Operation = UserOperation.Add, User = args.User });
        }

        /// <summary>
        /// Handles user remove action and invokes sending a message to slaves
        /// </summary>
        /// <param name="sender">Master</param>
        /// <param name="args">Removed user</param>
        private void UserRemovedHandler(object sender, UserEventArgs args)
        {
            SendUserMessage(
                new CommunicationMessage { Operation = UserOperation.Remove, User = args.User });
        }

        /// <summary>
        /// Sends message to slave
        /// </summary>
        /// <param name="message">Message about added or removed user</param>
        private void SendUserMessage(CommunicationMessage message)
        {
            Logger.Log(LogLevel.Trace, $"{message.User} sending to slaves");
            BinaryFormatter formatter = new BinaryFormatter();
            foreach (TcpClient slave in slaves)
            {
                if (!slave.Connected)
                {
                    continue;
                }

                try
                {
                    NetworkStream stream = slave.GetStream();
                    formatter.Serialize(stream, message);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevel.Warn, ex, $"Cannot send message to slave {slave}");
                }
            }
        }
    }
}
