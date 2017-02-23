using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Exceptions.StatefulService
{
    public class CannotSaveStateException : Exception
    {
        public CannotSaveStateException()
        {
        }

        public CannotSaveStateException(string message) : base(message)
        {
        }

        public CannotSaveStateException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CannotSaveStateException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
