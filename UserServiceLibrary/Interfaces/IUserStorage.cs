using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void StoreUsers(IEnumerable<User> users);

        /// <summary>
        /// Loads users from some storage
        /// </summary>
        /// <returns>Collection of users loaded from some storage</returns>
        ICollection<User> LoadUsers();
    }
}
