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

        /// <summary>
        /// First name of user
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Second name of user
        /// </summary>
        public string Secondname { get; set; }

        /// <summary>
        /// User's day of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
    }
}
