using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary
{
    /// <summary>
    /// Contains data about user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique user id
        /// </summary>
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Secondname { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
