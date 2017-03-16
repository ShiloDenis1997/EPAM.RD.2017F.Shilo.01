using System;
using System.Runtime.Serialization;

namespace UserServiceLibrary.Exceptions.UserService
{
    /// <summary>
    /// Indicates that user is not initialized
    /// </summary>
    [Serializable]
    public class NotInitializedUserException : UserServiceException
    {
        public NotInitializedUserException()
        {
        }

        public NotInitializedUserException(string message) : base(message)
        {
        }

        public NotInitializedUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotInitializedUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
