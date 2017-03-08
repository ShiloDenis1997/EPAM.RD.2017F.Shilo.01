using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using ServiceCommunicatorLibrary;
using ServiceManager.Exceptions;
using UserServiceConfigSection;
using UserServiceLibrary;
using UserServiceLibrary.Interfaces;
using UserStorageLibrary;

namespace ServiceManager
{
    public enum ServiceType
    {
        Master = 0,
        Slave = 1    
    }

    public class UserServiceManager
    {
        /// <summary>
        /// Lazy instance of <see cref="UserServiceManager"/>
        /// </summary>
        /// <exception cref="UnrecognizedServiceTypeException">When service type in 
        /// configs is unrecognized</exception>
        private static readonly Lazy<UserServiceManager> InstanceLazy = 
            new Lazy<UserServiceManager>(() => new UserServiceManager(), true);

        private AppDomain masterServiceDomain;
        private AppDomain slaveServiceDomain;
        private MasterServiceCommunicator masterCommunicator;
        private SlaveServiceCommunicator slaveCommunicator;

        private UserServiceSettingsConfigSection userServiceSection;

        /// <summary>
        /// Constructs new instance of <see cref="UserServiceManager"/>
        /// </summary>
        /// <exception cref="UnrecognizedServiceTypeException">When service type in 
        /// configs is unrecognized</exception>
        private UserServiceManager()
        {
            UserServiceSettingsConfigSection userServiceSection =
                        (UserServiceSettingsConfigSection)ConfigurationManager.GetSection("userService");
            if (userServiceSection == null)
            {
                throw new ConfigurationErrorsException("userServiceConfiguration is not provided");
            }

            this.userServiceSection = userServiceSection;
            string serviceTypeString = userServiceSection.Server.Type;
            switch (serviceTypeString)
            {
                case "Master":
                    ServiceType = ServiceType.Master;
                    break;
                case "Slave":
                    ServiceType = ServiceType.Slave;
                    break;
                default:
                    throw new UnrecognizedServiceTypeException();
            }
        }

        /// <summary> Instance of <see cref="UserServiceManager"/></summary>
        /// <exception cref="UnrecognizedServiceTypeException">When service type in 
        /// configs is unrecognized</exception>
        public static UserServiceManager Instance => InstanceLazy.Value;

        public ServiceType ServiceType { get; private set; }

        public IUserService GetConfiguredService()
        {
            switch (ServiceType)
            {
                case ServiceType.Master:
                    string storageFilename = userServiceSection.Storage.Filename;
                    
                    masterServiceDomain = AppDomain.CreateDomain(userServiceSection.Server.DomainName, null, null);
                    MasterUserService masterService = (MasterUserService)masterServiceDomain.CreateInstanceAndUnwrap(
                        "UserServiceLibrary",
                        "UserServiceLibrary.MasterUserService",
                        false,
                        BindingFlags.CreateInstance,
                        null,
                        new object[] { null, null, null },
                        null,
                        null);
                    UserStorage userStorage = new UserStorage(storageFilename);
                    masterService.UserStorage = userStorage;
                    List<TcpClientConfiguration> slavesConfigurations = new List<TcpClientConfiguration>();
                    foreach (SlaveElement slave in userServiceSection.SlaveItems)
                    {
                        slavesConfigurations.Add(new TcpClientConfiguration
                        {
                            Address = IPAddress.Parse(slave.IpAddress),
                            Port = int.Parse(slave.Port),
                        });
                    }

                    masterCommunicator = new MasterServiceCommunicator(
                        masterService, 
                        slavesConfigurations);
                    return masterService;
                case ServiceType.Slave:
                    string slaveAddress = userServiceSection.Server.IpAddress;
                    int slavePort = int.Parse(userServiceSection.Server.Port);
                    slaveServiceDomain = AppDomain.CreateDomain(
                        userServiceSection.Server.DomainName, null, null);
                    SlaveUserService slaveService = (SlaveUserService)slaveServiceDomain.CreateInstanceAndUnwrap(
                        "UserServiceLibrary", "UserServiceLibrary.SlaveUserService");
                    slaveCommunicator = new SlaveServiceCommunicator(
                        slaveService, IPAddress.Parse(slaveAddress), slavePort);
                    return slaveService;
                default:
                    return null;
            }
        }

        public void UnloadService()
        {
            switch (ServiceType)
            {
                case ServiceType.Master:
                    masterCommunicator?.Dispose();
                    AppDomain.Unload(masterServiceDomain);
                    return;
                case ServiceType.Slave:
                    slaveCommunicator?.Dispose();
                    AppDomain.Unload(slaveServiceDomain);
                    return;
            }
        }
    }
}
