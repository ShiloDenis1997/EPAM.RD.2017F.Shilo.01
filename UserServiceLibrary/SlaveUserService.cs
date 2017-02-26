using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary
{
    public class SlaveUserService : MarshalByRefObject, ISlaveService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ICollection<User> users;
        
        public SlaveUserService()
        {
            logger.Log(LogLevel.Trace, $"{nameof(SlaveUserService)} ctor started");
            users = new HashSet<User>();
            logger.Log(LogLevel.Trace, $"{nameof(SlaveUserService)} ctor finished");
        }

        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            logger.Log(LogLevel.Trace, "Searching user by predicate started.");
            return users.Where(u => predicate(u))
                .Select(u => u.Clone()).ToList();
        }

        public void UserAddedHandler(object sender, UserEventArgs args)
        {
            logger.Log(LogLevel.Trace, $"Adding of \"{args.User}\" handled");
            User userToAdd = args.User?.Clone();
            if (userToAdd == null)
            {
                throw new ArgumentNullException($"{nameof(args.User)} is null");
            }

            users.Add(userToAdd);
            logger.Log(LogLevel.Trace, $"Adding of \"{args.User}\" finished");
        }

        public void UserRemovedHandler(object sender, UserEventArgs args)
        {
            logger.Log(LogLevel.Trace, $"Removing of \"{args.User}\" handled");
            if (args.User == null)
            {
                throw new ArgumentNullException($"{nameof(args.User)} is null");
            }
            
            users.Remove(args.User);
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
