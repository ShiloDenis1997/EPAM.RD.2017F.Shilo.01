using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IMasterService : IUserService, IStatefulService
    {
        /// <summary>
        /// Raises when new user addes to service
        /// </summary>
        event EventHandler<UserEventArgs> UserAdded;

        /// <summary>
        /// Raises when some user removes from service
        /// </summary>
        event EventHandler<UserEventArgs> UserRemoved;
    }

    [Serializable]
    public class UserEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
