using System;
using System.Runtime.Serialization;

namespace UserServiceLibrary.Exceptions.UserService
{
    /// <summary>
    /// Indicates that user doesn't exists
    /// </summary>
    [Serializable]
    public class UserDoesNotExistException : UserServiceException
    {
        public UserDoesNotExistException()
        {
        }

        public UserDoesNotExistException(string message) : base(message)
        {
        }

        public UserDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
