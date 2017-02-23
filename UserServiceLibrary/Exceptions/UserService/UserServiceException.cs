using System;
using System.Runtime.Serialization;

namespace UserServiceLibrary.Exceptions.UserService
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
