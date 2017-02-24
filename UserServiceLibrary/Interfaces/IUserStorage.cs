using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions.UserStorage;

namespace UserServiceLibrary.Interfaces
{
    /// <summary>
    /// Interface of abstract <see cref="User"/> storage
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        /// Stores <paramref name="users"/> in some storage
        /// </summary>
        /// <param name="users">Users to store</param>
        /// <exception cref="UserStorageException">Throws if there are some errors with
        /// storing users</exception>
        void StoreUsers(IEnumerable<User> users);

        /// <summary>
        /// Loads users from some storage
        /// </summary>
        /// <returns>Collection of users loaded from some storage</returns>
        /// <exception cref="UserStorageException">Throws if there are some errors
        /// with loading users</exception>
        ICollection<User> LoadUsers();
    }
}
