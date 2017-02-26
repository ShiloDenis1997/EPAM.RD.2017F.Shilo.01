using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;
using UserStorageLibrary;

namespace HostApplication
{
    public class Mainprg
    {
        public static void Main(string[] args)
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

            UserStorage userStorage = new UserStorage(storageFilename);
            UserService userService = new UserService();
            userService.UserStorage = userStorage;

            foreach (var user in usersEnumeration)
            {
                userService.Add(user);
            }

            userService.SaveState();

            userService = new UserService();
            userService.UserStorage = userStorage;
            userService.LoadSavedState();

            IEnumerable<User> loadedUsers = userService.Search(u => true);
            foreach (var user in loadedUsers)
            {
                Console.WriteLine(user);
            }
        }
    }
}
