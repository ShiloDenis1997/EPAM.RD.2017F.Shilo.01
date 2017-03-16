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
    [Serializable]
    public class UserStorage : IUserServiceStorage
    {
        /// <summary>
        /// Creates an instance of user storage with
        /// specific filename
        /// </summary>
        /// <param name="filename">name of the storage file</param>
        /// <exception cref="ArgumentException">Throws if 
        /// <paramref name="filename"/> is null, empty or whitespace</exception>
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
        /// Stores <paramref name="state"/> in xml-file
        /// </summary>
        /// <param name="state">State to store in</param>
        /// <exception cref="CannotSerializeException">Throws if cannot serialize the
        /// <paramref name="state"/> in xml-file</exception>
        public void StoreServiceState(UserServiceState state)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserServiceState));
            using (FileStream fs = new FileStream(this.FileName, FileMode.Create))
            {
                serializer.Serialize(fs, state);
            }
        }

        /// <summary>
        /// Loads state from xml-file
        /// </summary>
        /// <returns>Loaded state of the service</returns>
        /// <exception cref="CannotDeserializeException">Throws if cannot deserialize
        /// state from xml-file</exception>
        public UserServiceState LoadServiceState()
        {
            UserServiceState state;
            XmlSerializer serializer = new XmlSerializer(typeof(UserServiceState));
            using (FileStream fs = new FileStream(this.FileName, FileMode.Open))
            {
                state = (UserServiceState)serializer.Deserialize(fs);
            }

            return state;
        }
    }
}
