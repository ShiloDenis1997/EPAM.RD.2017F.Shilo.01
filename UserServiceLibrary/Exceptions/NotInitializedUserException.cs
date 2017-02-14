using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Exceptions
{
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
