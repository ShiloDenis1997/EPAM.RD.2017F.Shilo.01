using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions.UserStorage;

namespace UserStorageLibrary.Exceptions
{
    [Serializable]
    public class CannotSerializeException : UserStorageException
    {
        public CannotSerializeException()
        {
        }

        public CannotSerializeException(string message) : base(message)
        {
        }

        public CannotSerializeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CannotSerializeException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
