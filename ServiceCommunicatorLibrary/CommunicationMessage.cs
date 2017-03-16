using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;

namespace ServiceCommunicatorLibrary
{
    /// <summary>
    /// Enum of possible operations in notifications
    /// </summary>
    public enum UserOperation
    {
        Add = 1,
        Remove = 2,
    }

    /// <summary>
    /// Incapsulates data about operation
    /// </summary>
    [Serializable]
    public class CommunicationMessage
    {
        /// <summary>
        /// Operations type
        /// </summary>
        public UserOperation Operation { get; set; }

        /// <summary>
        /// User, which is under the operation
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Creates a string representation of message
        /// </summary>
        /// <returns>String representation of message</returns>
        public override string ToString()
        {
            return (Operation == UserOperation.Add ? "Add" : "Remove") + User;
        }
    }
}
