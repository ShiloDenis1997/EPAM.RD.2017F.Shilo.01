﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;
using UserServiceLibrary.Interfaces;
using UserStorageLibrary;

namespace HostApplication
{
    public class Mainprg
    {
        public static void Main(string[] args)
        {
            DemonstrateWithDomains();
            //DemonstrateWithoutDomains();
        }

        public static void DemonstrateWithDomains()
        {
            string storageFilename =
                ConfigurationManager.AppSettings["storageFilename"];
            int slavesCount = int.Parse(ConfigurationManager.AppSettings["slavesCount"]);

            AppDomain masterDomain = AppDomain.CreateDomain("Master domain", null, null);
            MasterUserService masterService = (MasterUserService) masterDomain.CreateInstanceAndUnwrap(
                "UserServiceLibrary", "UserServiceLibrary.MasterUserService", 
                false, BindingFlags.CreateInstance,
                null, new object[] {null, null, null}, null, null);

            AppDomain[] slaveDomains = new AppDomain[slavesCount];
            SlaveUserService[] slaveServices = new SlaveUserService[slavesCount];

            for (int i = 0; i < slavesCount; i++)
            {
                slaveDomains[i] = AppDomain.CreateDomain(
                    $"Slave domain #{i}", null, null);
                slaveServices[i] = (SlaveUserService) slaveDomains[i].CreateInstanceAndUnwrap(
                    "UserServiceLibrary", "UserServiceLibrary.SlaveUserService");
                masterService.UserAdded += slaveServices[i].UserAddedHandler;
                masterService.UserRemoved += slaveServices[i].UserRemovedHandler;
            }

            UserStorage userStorage = new UserStorage(storageFilename);
            masterService.UserStorage = userStorage;
            masterService.LoadSavedState();
            
            IEnumerable<User> loadedUsers = masterService.Search(u => true);
            foreach (var user in loadedUsers)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("From slaves: ");
            for (int i = 0; i < slaveServices.Length; i++)
            {
                Console.WriteLine($"\nSlave {i}:");
                foreach (var user in slaveServices[i].Search(u => true))
                {
                    Console.WriteLine(user);
                }

                masterService.Remove(masterService.Search(u => true).First());
            }

            foreach (var domain in slaveDomains)
            {
                AppDomain.Unload(domain);
            }

            AppDomain.Unload(masterDomain);
        }

        public static void DemonstrateWithoutDomains()
        {
            IEnumerable<User> usersEnumeration = new[]
            {
                new User
                {
                    DateOfBirth = DateTime.MinValue,
                    Firstname = "Denis",
                    Id = 1,
                    Secondname = "Shilo"
                },
                new User
                {
                    DateOfBirth  = DateTime.MaxValue,
                    Firstname = "Ivan",
                    Id = 2,
                    Secondname = "Matyash"
                },
                new User
                {
                    DateOfBirth = DateTime.MinValue,
                    Secondname = "Drozd",
                    Firstname = "Artsiom",
                    Id = 3
                },
            };

            string storageFilename =
                ConfigurationManager.AppSettings["storageFilename"];
            int slavesCount = int.Parse(ConfigurationManager.AppSettings["slavesCount"]);

            UserStorage userStorage = new UserStorage(storageFilename);
            MasterUserService masterUserService = new MasterUserService();
            masterUserService.UserStorage = userStorage;

            ISlaveService[] slaves = new SlaveUserService[slavesCount];
            for (int i = 0; i < slaves.Length; i++)
            {
                slaves[i] = new SlaveUserService();
                masterUserService.UserRemoved += slaves[i].UserRemovedHandler;
                masterUserService.UserAdded += slaves[i].UserAddedHandler;
            }

            //foreach (var user in usersEnumeration)
            //{
            //    masterUserService.Add(user);
            //}

            //masterUserService.SaveState();

            //masterUserService = new MasterUserService();
            //masterUserService.UserStorage = userStorage;
            masterUserService.LoadSavedState();

            IEnumerable<User> loadedUsers = masterUserService.Search(u => true);
            foreach (var user in loadedUsers)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("From slaves: ");
            for (int i = 0; i < slaves.Length; i++)
            {
                Console.WriteLine($"\nSlave {i}:");
                foreach (var user in slaves[i].Search(u => true))
                {
                    Console.WriteLine(user);
                }

                masterUserService.Remove(usersEnumeration.Skip(i).First());
            }
        }
    }
}
