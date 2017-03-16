using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface ISlaveService : IUserService
    {
        /// <summary>
        /// Uses to handle user added event from master service
        /// </summary>
        /// <param name="sender">Master</param>
        /// <param name="args">Contains added user</param>
        void UserAddedHandler(object sender, UserEventArgs args);

        /// <summary>
        /// Uses to handle user removed event from master service
        /// </summary>
        /// <param name="sender">Master</param>
        /// <param name="args">Contains removed user</param>
        void UserRemovedHandler(object sender, UserEventArgs args);
    }
}
