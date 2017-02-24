using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UserServiceLibrary;
using UserServiceLibrary.Interfaces;
using UserStorageLibrary.Exceptions;

namespace UserStorageLibrary
{
    public class UserStorage : IUserStorage
    {
        public UserStorage(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException($"{nameof(filename)} is null or whitespace");
            }

            this.FileName = filename;
        }

        public string FileName { get; private set; }

        /// <summary>
        /// Stores users in xml-file
        /// </summary>
        /// <param name="users">Users to store in</param>
        /// <exception cref="CannotSerializeException">Throws if cannot serialize enumerable
        /// of users in xml-file</exception>
        public void StoreUsers(IEnumerable<User> users)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream(this.FileName, FileMode.Create))
            {
                serializer.Serialize(fs, users.ToList());
            }
        }

        /// <summary>
        /// Loads users from xml-file
        /// </summary>
        /// <returns>Collection of users</returns>
        /// <exception cref="CannotDeserializeException">Throws if cannot deserialize
        /// users from xml-file</exception>
        public ICollection<User> LoadUsers()
        {
            List<User> users;
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream(this.FileName, FileMode.Open))
            {
                users = (List<User>)serializer.Deserialize(fs);
            }

            return users;
        }
    }
}
