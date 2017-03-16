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
        private List<AppDomain> slaveServicesDomains = new List<AppDomain>();
        private MasterServiceCommunicator masterCommunicator;
        private List<SlaveServiceCommunicator> slavesCommunicators = new List<SlaveServiceCommunicator>();

        private UserServiceSettingsConfigSection userServiceSection;

        /// <summary>
        /// Constructs new instance of <see cref="UserServiceManager"/>
        /// </summary>
        /// <exception cref="UnrecognizedServiceTypeException">Throws when service type in 
        /// configs is unrecognized</exception>
        /// <exception cref="ConfigurationErrorsException">Throws when userService section is not
        /// provided in configs</exception>
        private UserServiceManager()
        {
            userServiceSection =
                        (UserServiceSettingsConfigSection)ConfigurationManager.GetSection("userService");
            if (userServiceSection == null)
            {
                throw new ConfigurationErrorsException("userServiceConfiguration is not provided");
            }
            
            ServiceType = userServiceSection.Server.Ttype;
        }

        /// <summary> Instance of <see cref="UserServiceManager"/></summary>
        /// <exception cref="UnrecognizedServiceTypeException">Throws when service type in 
        /// configs is unrecognized</exception>
        public static UserServiceManager Instance => InstanceLazy.Value;

        /// <summary>
        /// Type of service or services that will be configured
        /// </summary>
        public ServiceType ServiceType { get; private set; }

        /// <summary>
        /// Configures master service and loads it in separate domain
        /// </summary>
        /// <returns>Configured <see cref="IMasterService"/> or null if <see cref="ServiceType"/>
        /// is not Master</returns>
        public IMasterService GetMasterService()
        {
            if (ServiceType != ServiceType.Master)
            {
                return null;
            }
            
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
            foreach (SlaveElement slave in userServiceSection.Server.SlaveItems)
            {
                slavesConfigurations.Add(new TcpClientConfiguration
                {
                    Address = slave.TipAddress,
                    Port = slave.Tport,
                });
            }

            masterCommunicator = new MasterServiceCommunicator(
                masterService, 
                slavesConfigurations);
            return masterService;
        }

        /// <summary>
        /// Configures slave services and loads them in separate domains
        /// </summary>
        /// <returns>Enumeration of configured <see cref="ISlaveService"/> or 
        /// null if <see cref="ServiceType"/> is not Slave</returns>
        public IEnumerable<ISlaveService> GetSlaveServices()
        {
            if (ServiceType != ServiceType.Slave)
            {
                return null;
            }
            
            List<ISlaveService> slaveServices = new List<ISlaveService>();
            foreach (SlaveElement slave in userServiceSection.Server.SlaveItems)
            {
                AppDomain slaveServiceDomain = AppDomain.CreateDomain(slave.DomainName, null, null);
                slaveServicesDomains.Add(slaveServiceDomain);
                SlaveUserService slaveService = (SlaveUserService)slaveServiceDomain.CreateInstanceAndUnwrap(
                    "UserServiceLibrary", "UserServiceLibrary.SlaveUserService");
                slaveServices.Add(slaveService);
                slavesCommunicators.Add(new SlaveServiceCommunicator(
                    slaveService, slave.TipAddress, slave.Tport));
            }

            return slaveServices;
        }

        /// <summary>
        /// Unloads services from domains according to a service type
        /// </summary>
        public void UnloadService()
        {
            switch (ServiceType)
            {
                case ServiceType.Master:
                    masterCommunicator?.Dispose();
                    AppDomain.Unload(masterServiceDomain);
                    return;
                case ServiceType.Slave:
                    foreach (var communicator in slavesCommunicators)
                    {
                        communicator.Dispose();
                    }

                    foreach (var domain in slaveServicesDomains)
                    {
                        AppDomain.Unload(domain);
                    }
                    
                    return;
            }
        }
    }
}
