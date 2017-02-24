using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions.UserStorage;

namespace UserStorageLibrary.Exceptions
{
    public class CannotDeserializeException : UserStorageException
    {
        public CannotDeserializeException()
        {
        }

        public CannotDeserializeException(string message) : base(message)
        {
        }

        public CannotDeserializeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CannotDeserializeException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
