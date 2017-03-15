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
    [Serializable]
    public class User : IEquatable<User>, ICloneable
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

        /// <summary>
        /// Performs equality comparing with other <see cref="User"/>'s instance
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if objects are equal, false another</returns>
        public bool Equals(User other)
        {
            return this.Equals((object)other);
        }

        /// <summary>
        /// Performs equality comparing with other object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if objects are equal, false another</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            User other = obj as User;
            return Id == other.Id && 
                string.Equals(Firstname, other.Firstname, StringComparison.OrdinalIgnoreCase) && 
                string.Equals(Secondname, other.Secondname, StringComparison.OrdinalIgnoreCase) && 
                DateOfBirth.Equals(other.DateOfBirth);
        }

        /// <summary>
        /// Returns a hash code of <see cref="User"/>. 
        /// Id field mustn't changing if <see cref="User"/> is
        /// in a hashtable
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = Id;
            return hashCode;
        }

        public override string ToString() => $"{Firstname} {Secondname}";

        public User Clone()
        {
            return new User
            {
                DateOfBirth = this.DateOfBirth,
                Firstname = this.Firstname,
                Id = this.Id,
                Secondname = this.Secondname,
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
