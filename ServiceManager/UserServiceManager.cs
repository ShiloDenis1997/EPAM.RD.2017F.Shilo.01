using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using ServiceCommunicatorLibrary;
using ServiceManager.Exceptions;
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

        /// <summary>
        /// Constructs new instance of <see cref="UserServiceManager"/>
        /// </summary>
        /// <exception cref="UnrecognizedServiceTypeException">When service type in 
        /// configs is unrecognized</exception>
        private UserServiceManager()
        {
            string serviceTypeString = ConfigurationManager.AppSettings["serviceType"];
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
                    string storageFilename = ConfigurationManager.AppSettings["storageFilename"];

                    // int slavesCount = int.Parse(ConfigurationManager.AppSettings["slavesCount"]);
                    masterServiceDomain = AppDomain.CreateDomain("Master domain", null, null);
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
                    masterCommunicator = new MasterServiceCommunicator(
                        masterService, IPAddress.Parse("127.0.0.1"), 8080);
                    return masterService;
                case ServiceType.Slave:
                    slaveServiceDomain = AppDomain.CreateDomain(
                        "Slave domain", null, null);
                    SlaveUserService slaveService = (SlaveUserService)slaveServiceDomain.CreateInstanceAndUnwrap(
                        "UserServiceLibrary", "UserServiceLibrary.SlaveUserService");
                    slaveCommunicator = new SlaveServiceCommunicator(
                        slaveService, IPAddress.Parse("127.0.0.1"), 8080);
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
