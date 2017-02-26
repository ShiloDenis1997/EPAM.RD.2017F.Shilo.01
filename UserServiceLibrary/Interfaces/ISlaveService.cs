using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface ISlaveService : IUserService
    {
        void UserAddedHandler(object sender, UserEventArgs args);

        void UserRemovedHandler(object sender, UserEventArgs args);
    }
}
