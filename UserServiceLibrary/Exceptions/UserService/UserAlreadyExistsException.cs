using System;
using System.Runtime.Serialization;

namespace UserServiceLibrary.Exceptions.UserService
{
    /// <summary>
    /// Indicates that user already exists
    /// </summary>
    [Serializable]
    public class UserAlreadyExistsException : UserServiceException
    {
        public UserAlreadyExistsException()
        {
        }

        public UserAlreadyExistsException(string message) : base(message)
        {
        }

        public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
