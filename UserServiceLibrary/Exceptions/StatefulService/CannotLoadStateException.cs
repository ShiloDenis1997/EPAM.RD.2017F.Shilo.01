using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Exceptions.StatefulService
{
    public class CannotLoadStateException : Exception
    {
        public CannotLoadStateException()
        {
        }

        public CannotLoadStateException(string message) 
            : base(message)
        {
        }

        public CannotLoadStateException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CannotLoadStateException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
