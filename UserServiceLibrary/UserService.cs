using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary
{
    public class UserService : IUserService
    {
        private IEqualityComparer<User> userEqualityComparer;
        private Func<int> uniqueIdGenerator;

        /// <summary>
        /// Constructs an instance of <see cref="UserService"/>
        /// </summary>
        /// <param name="uniqueIdGenerator">Delegate that generates unique ides.
        /// If not specified, default idGenerator will be used</param>
        /// <param name="userEqualityComparer">Uses to determine if two users are same. 
        /// If not specified, <see cref="EqualityComparer{User}.Default"/> will be used</param>
        public UserService(
            Func<int> uniqueIdGenerator = null, 
            IEqualityComparer<User> userEqualityComparer = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IUserService.Add"/>
        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IUserService.Remove"/>
        public void Remove(User user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IUserService.Search"/>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
