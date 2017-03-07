using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary
{
    public class SlaveUserService : MarshalByRefObject, ISlaveService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ICollection<User> users;

        private ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();
        
        public SlaveUserService()
        {
            logger.Log(LogLevel.Trace, $"{nameof(SlaveUserService)} ctor started");
            users = new HashSet<User>();
            logger.Log(LogLevel.Trace, $"{nameof(SlaveUserService)} ctor finished");
        }

        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            logger.Log(LogLevel.Trace, "Searching user by predicate started.");
            List<User> usersList;
            readWriteLock.EnterReadLock();
            try
            {
                usersList = users.ToList();
            }
            finally
            {
                readWriteLock.ExitReadLock();
            }

            return usersList.Where(u => predicate(u)).ToList();
        }

        public void UserAddedHandler(object sender, UserEventArgs args)
        {
            logger.Log(LogLevel.Trace, $"Adding of \"{args.User}\" handled");
            User userToAdd = args.User;
            if (userToAdd == null)
            {
                throw new ArgumentNullException($"{nameof(args.User)} is null");
            }

            readWriteLock.EnterWriteLock();
            try
            {
                users.Add(userToAdd);
            }
            finally
            {
                readWriteLock.ExitWriteLock();
            }

            logger.Log(LogLevel.Trace, $"Adding of \"{userToAdd}\" finished");
        }

        public void UserRemovedHandler(object sender, UserEventArgs args)
        {
            logger.Log(LogLevel.Trace, $"Removing of \"{args.User}\" handled");
            if (args.User == null)
            {
                throw new ArgumentNullException($"{nameof(args.User)} is null");
            }

            readWriteLock.EnterWriteLock();
            try
            {
                users.Remove(args.User);
            }
            finally
            {
                readWriteLock.ExitWriteLock();
            }

            logger.Log(LogLevel.Trace, $"Removing of \"{args.User}\" finished");
        }

        public void Add(User user)
        {
            throw new NotSupportedException(
                "Add user operation is not supported for slave");
        }

        public void Remove(User user)
        {
            throw new NotSupportedException(
                "Remove user operation is not supported for slave");
        }
    }
}
