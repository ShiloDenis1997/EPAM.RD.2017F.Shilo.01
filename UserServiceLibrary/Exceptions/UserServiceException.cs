using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Exceptions
{
    /// <summary>
    /// Root of the exception hierarcy for user services
    /// </summary>
    public class UserServiceException : Exception
    {
        public UserServiceException()
        {
        }

        public UserServiceException(string message) : base(message)
        {
        }

        public UserServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
