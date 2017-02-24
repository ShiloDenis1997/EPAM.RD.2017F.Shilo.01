using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Exceptions.StatefulService;
using UserServiceLibrary.Exceptions.UserService;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary
{
    /// <summary>
    /// Implementation of <see cref="IUserService"/> to provide a simple
    /// functionallity of user service
    /// </summary>
    public class UserService : IUserService, IStatefulService
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
        /// <param name="userStorage">Implementation of <see cref="IUserStorage"/> interface.
        /// If not provided, <see cref="SaveState"/> and <see cref="LoadSavedState"/>
        /// will throw an <see cref="StatefulServiceException"/></param>
        public UserService(
            Func<int> uniqueIdGenerator = null, 
            IEqualityComparer<User> userEqualityComparer = null,
            IUserStorage userStorage = null)
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
            UserStorage = userStorage;
            users = new HashSet<User>();
        }

        /// <summary>
        /// If null, <see cref="LoadSavedState"/> and <see cref="SaveState"/>
        /// will throw <see cref="StatefulServiceException"/>
        /// </summary>
        private IUserStorage UserStorage { get; set; }

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

        /// <inheritdoc cref="IStatefulService.SaveState"/>
        /// <exception cref="StatefulServiceException">Throws when 
        /// <see cref="IUserStorage"/> is not provided</exception>
        public void SaveState()
        {
            if (UserStorage == null)
            {
                throw new StatefulServiceException("User storage is not provided");
            }

            try
            {
                UserStorage.StoreUsers(users);
            }
            catch (Exception ex)
            {
                throw new CannotSaveStateException("Cannot save state", ex);
            }
        }

        /// <inheritdoc cref="IStatefulService.LoadSavedState"/>
        /// /// <exception cref="StatefulServiceException">Throws when 
        /// <see cref="IUserStorage"/> is not provided</exception>
        public void LoadSavedState()
        {
            if (UserStorage == null)
            {
                throw new StatefulServiceException("User storage is not provided");
            }

            try
            {
                users = new HashSet<User>(UserStorage.LoadUsers());
            }
            catch (Exception ex)
            {
                throw new CannotLoadStateException("Cannot load state", ex);
            }
        }
    }
}
