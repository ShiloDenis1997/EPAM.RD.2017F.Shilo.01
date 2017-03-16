using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Exceptions.StatefulService
{
    /// <summary>
    /// Indicates that service state cannot be saved
    /// </summary>
    [Serializable]
    public class CannotSaveStateException : StatefulServiceException
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
