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
    public class UserServiceManager
    {
        private static readonly Lazy<UserServiceManager> InstanceLazy = 
            new Lazy<UserServiceManager>(() => new UserServiceManager(), true);

        private AppDomain masterServiceDomain;
        private AppDomain slaveServiceDomain;
        private MasterServiceCommunicator masterCommunicator;
        private SlaveServiceCommunicator slaveCommunicator;

        private UserServiceManager()
        {
        }

        public static UserServiceManager Instance => InstanceLazy.Value;

        public IUserService GetConfiguredService(bool isMaster)
        {
            switch (isMaster)
            {
                case true:
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
                case false:
                    slaveServiceDomain = AppDomain.CreateDomain(
                        "Slave domain", null, null);
                    SlaveUserService slaveService = (SlaveUserService)slaveServiceDomain.CreateInstanceAndUnwrap(
                        "UserServiceLibrary", "UserServiceLibrary.SlaveUserService");
                    slaveCommunicator = new SlaveServiceCommunicator(
                        slaveService, IPAddress.Parse("127.0.0.1"), 8080);
                    return slaveService;
                default:
                    throw new UnrecognizedServiceTypeException(); 
            }
        }
    }
}
