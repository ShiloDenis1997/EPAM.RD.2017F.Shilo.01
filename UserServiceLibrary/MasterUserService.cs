using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
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
    public class MasterUserService : MarshalByRefObject, IMasterService, IStatefulService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IEqualityComparer<User> userEqualityComparer;
        private Func<int> uniqueIdGenerator;
        private ICollection<User> users;

        private object usersLockObject = new object();

        /// <summary>
        /// Constructs an instance of <see cref="MasterUserService"/>
        /// </summary>
        /// <param name="uniqueIdGenerator">Delegate that generates unique ides.
        /// If not specified, default idGenerator will be used</param>
        /// <param name="userEqualityComparer">Uses to determine if two users are same. 
        /// If not specified, <see cref="EqualityComparer{User}.Default"/> will be used</param>
        /// <param name="userStorage">Implementation of <see cref="IUserStorage"/> interface.
        /// If not provided, <see cref="SaveState"/> and <see cref="LoadSavedState"/>
        /// will throw an <see cref="StatefulServiceException"/></param>
        public MasterUserService(
            Func<int> uniqueIdGenerator = null, 
            IEqualityComparer<User> userEqualityComparer = null,
            IUserStorage userStorage = null)
        {
            logger.Log(LogLevel.Trace, $"{nameof(MasterUserService)} ctor started");

            if (uniqueIdGenerator == null)
            {
                int counter = 1;
                this.uniqueIdGenerator = () => counter++;
            }
            else
            {
                this.uniqueIdGenerator = uniqueIdGenerator;
            }
            
            this.userEqualityComparer = userEqualityComparer 
                ?? EqualityComparer<User>.Default;
            UserStorage = userStorage;
            users = new HashSet<User>();
            logger.Log(LogLevel.Trace, $"{nameof(MasterUserService)} ctor finished");
        }

        public event EventHandler<UserEventArgs> UserAdded;

        public event EventHandler<UserEventArgs> UserRemoved;

        /// <summary>
        /// If null, <see cref="LoadSavedState"/> and <see cref="SaveState"/>
        /// will throw <see cref="StatefulServiceException"/>
        /// </summary>
        public IUserStorage UserStorage { get; set; }

        /// <inheritdoc cref="IUserService.Add"/>
        public void Add(User user)
        {
            logger.Log(LogLevel.Trace, $"{user} adding...");
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

            User userToAdd = user.Clone();
            lock (usersLockObject)
            {
                users.Add(userToAdd);
            }

            logger.Log(LogLevel.Trace, $"{user} added");
            StartUserAdded(userToAdd);
        }

        /// <inheritdoc cref="IUserService.Remove"/>
        public void Remove(User user)
        {
            logger.Log(LogLevel.Trace, $"{user} removing...");
            if (user == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(user)} is null");
            }

            User removingUser;
            lock (usersLockObject)
            {
                 removingUser = users.FirstOrDefault(
                    u => userEqualityComparer.Equals(u, user));
                if (removingUser == null)
                {
                    throw new UserDoesNotExistException(
                        $"{nameof(user)} does not exists");
                }

                users.Remove(removingUser);
            }

            logger.Log(LogLevel.Trace, $"{user} removed");
            StartUserRemoved(removingUser);
        }

        /// <inheritdoc cref="IUserService.Search"/>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            logger.Log(LogLevel.Trace, $"Searching user by predicate...");
            if (predicate == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(predicate)} is null");
            }

            List<User> usersList;
            lock (usersLockObject)
            {
                usersList = users.Where(u => predicate(u))
                    .Select(u => u.Clone()).ToList();
            }

            return usersList;
        }

        /// <inheritdoc cref="IStatefulService.SaveState"/>
        /// <exception cref="StatefulServiceException">Throws when 
        /// <see cref="IUserStorage"/> is not provided</exception>
        public void SaveState()
        {
            logger.Log(LogLevel.Trace, "Saving state...");
            if (UserStorage == null)
            {
                throw new StatefulServiceException("User storage is not provided");
            }

            try
            {
                List<User> usersList;
                lock (usersLockObject)
                {
                    usersList = users.ToList();
                }

                UserStorage.StoreUsers(usersList);
            }
            catch (Exception ex)
            {
                throw new CannotSaveStateException("Cannot save state", ex);
            }

            logger.Log(LogLevel.Trace, "State saved.");
        }

        /// <inheritdoc cref="IStatefulService.LoadSavedState"/>
        /// /// <exception cref="StatefulServiceException">Throws when 
        /// <see cref="IUserStorage"/> is not provided</exception>
        public void LoadSavedState()
        {
            logger.Log(LogLevel.Trace, "Loading state...");
            if (UserStorage == null)
            {
                throw new StatefulServiceException("User storage is not provided");
            }

            try
            {
                lock (usersLockObject)
                {
                    users = new HashSet<User>(UserStorage.LoadUsers());
                }

                SendUsersLoadedNotifications();
            }
            catch (Exception ex)
            {
                throw new CannotLoadStateException("Cannot load state", ex);
            }

            logger.Log(LogLevel.Trace, "State loaded...");
        }

        private void SendUsersLoadedNotifications()
        {
            logger.Log(LogLevel.Trace, "Notificating all subscribers...");
            List<User> usersList;
            lock (usersLockObject)
            {
                usersList = users.ToList();
            }

            foreach (var user in usersList)
            {
                StartUserAdded(user);
            }
        }

        private void StartUserAdded(User user)
        {
            UserAdded?.Invoke(this, new UserEventArgs { User = user });
        }

        private void StartUserRemoved(User user)
        {
            UserRemoved?.Invoke(this, new UserEventArgs { User = user });
        }
    }
}
