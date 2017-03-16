using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary
{
    /// <summary>
    /// Incapsulates a state of user service
    /// </summary>
    [Serializable]
    public class UserServiceState
    {
        public List<User> Users { get; set; }

        public int LastGeneratedId { get; set; }
    }
}
