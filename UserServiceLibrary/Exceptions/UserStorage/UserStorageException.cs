using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Exceptions.UserStorage
{
    [Serializable]
    public class UserStorageException : Exception
    {
        public UserStorageException()
        {
        }

        public UserStorageException(string message) : base(message)
        {
        }

        public UserStorageException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected UserStorageException(
            SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
