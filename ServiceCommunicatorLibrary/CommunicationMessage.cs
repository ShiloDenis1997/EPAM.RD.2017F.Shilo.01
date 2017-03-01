using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;

namespace ServiceCommunicatorLibrary
{
    public enum UserOperation
    {
        Add = 1,
        Remove = 2,
    }

    [Serializable]
    public class CommunicationMessage
    {
        public UserOperation Operation { get; set; }

        public User User { get; set; }

        public override string ToString()
        {
            return (Operation == UserOperation.Add ? "Add" : "Remove") + User;
        }
    }
}
