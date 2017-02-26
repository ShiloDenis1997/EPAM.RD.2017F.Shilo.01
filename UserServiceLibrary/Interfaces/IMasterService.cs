using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IMasterService : IUserService
    {
        event EventHandler<UserEventArgs> UserAdded;

        event EventHandler<UserEventArgs> UserRemoved;
    }

    public class UserEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
