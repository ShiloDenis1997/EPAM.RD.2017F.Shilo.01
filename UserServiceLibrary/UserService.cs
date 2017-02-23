using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary
{
    /// <summary>
    /// Implementation of <see cref="IUserService"/> to provide a simple
    /// functionallity of user service
    /// </summary>
    public class UserService : IUserService
    {
        private IEqualityComparer<User> userEqualityComparer;
        private Func<int> uniqueIdGenerator;
        private ICollection<User> users;

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
            if (uniqueIdGenerator == null)
            {
                int counter = 1;
                this.uniqueIdGenerator = () => counter++;
            }
            else
            {
                this.uniqueIdGenerator = uniqueIdGenerator;
            }
            
            this.userEqualityComparer = userEqualityComparer ?? EqualityComparer<User>.Default;
            users = new LinkedList<User>();
        }

        /// <inheritdoc cref="IUserService.Add"/>
        public void Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(user)} is null");
            }

            if (user.Firstname == null || user.Secondname == null ||
                user.DateOfBirth == null)
            {
                throw new NotInitializedUserException(
                    $"{nameof(user)} is not fully initialized");
            }

            if (users.Contains(user, userEqualityComparer))
            {
                throw new UserAlreadyExistsException(
                    $"{nameof(user)} is already exists");
            }

            user.Id = uniqueIdGenerator();
            users.Add(user);
        }

        /// <inheritdoc cref="IUserService.Remove"/>
        public void Remove(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(user)} is null");
            }

            User removingUser = users.FirstOrDefault(
                u => userEqualityComparer.Equals(u, user));
            if (removingUser == null)
            {
                throw new UserDoesNotExistException(
                    $"{nameof(user)} does not exists");
            }

            users.Remove(removingUser);
        }

        /// <inheritdoc cref="IUserService.Search"/>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(predicate)} is null");
            }

            return users.Where(u => predicate(u));
        }
    }
}
