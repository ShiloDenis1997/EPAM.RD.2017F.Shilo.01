using System;
using System.Runtime.Serialization;

namespace UserServiceLibrary.Exceptions.UserService
{
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
