using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Exceptions
{
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
