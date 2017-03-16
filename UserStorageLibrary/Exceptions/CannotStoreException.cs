using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions.UserStorage;

namespace UserStorageLibrary.Exceptions
{
    /// <summary>
    /// Indicates that state cannot be stored
    /// </summary>
    [Serializable]
    public class CannotStoreException : UserStorageException
    {
        public CannotStoreException()
        {
        }

        public CannotStoreException(string message) : base(message)
        {
        }

        public CannotStoreException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CannotStoreException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
