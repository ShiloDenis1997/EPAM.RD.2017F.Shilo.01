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
    public interface IUserServiceStorage
    {
        /// <summary>
        /// Stores <paramref name="state"/> in some storage
        /// </summary>
        /// <param name="state">State of service</param>
        /// <exception cref="UserStorageException">Throws if there are some errors with
        /// storing <paramref name="state"/></exception>
        void StoreServiceState(UserServiceState state);

        /// <summary>
        /// Loads state from some storage
        /// </summary>
        /// <returns>State of the service loaded from some storage</returns>
        /// <exception cref="UserStorageException">Throws if there are some errors
        /// with loading state</exception>
        UserServiceState LoadServiceState();
    }
}
