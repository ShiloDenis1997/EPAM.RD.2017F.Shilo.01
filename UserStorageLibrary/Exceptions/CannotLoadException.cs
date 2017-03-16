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
    /// Indicates that state cannot be loaded
    /// </summary>
    [Serializable]
    public class CannotLoadException : UserStorageException
    {
        public CannotLoadException()
        {
        }

        public CannotLoadException(string message) : base(message)
        {
        }

        public CannotLoadException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CannotLoadException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
