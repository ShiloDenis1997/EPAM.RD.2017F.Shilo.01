using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary
{
    public class SlaveUserService : ISlaveService
    {
        private ICollection<User> users;
        
        public SlaveUserService()
        {
            users = new HashSet<User>();
        }

        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            return users.Where(u => predicate(u))
                .Select(u => u.Clone()).ToList();
        }

        public void UserAddedHandler(object sender, UserEventArgs args)
        {
            User userToAdd = args.User?.Clone();
            if (userToAdd == null)
            {
                throw new ArgumentNullException($"{nameof(args.User)} is null");
            }

            users.Add(userToAdd);
        }

        public void UserRemovedHandler(object sender, UserEventArgs args)
        {
            if (args.User == null)
            {
                throw new ArgumentNullException($"{nameof(args.User)} is null");
            }
            
            users.Remove(args.User);
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
