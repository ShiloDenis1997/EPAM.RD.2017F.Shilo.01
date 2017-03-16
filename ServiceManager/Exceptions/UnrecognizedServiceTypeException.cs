using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Exceptions
{
    /// <summary>
    /// Exception to indicate that type of service in configs does not recognized
    /// </summary>
    public class UnrecognizedServiceTypeException : Exception
    {
        public UnrecognizedServiceTypeException()
        {
        }

        public UnrecognizedServiceTypeException(string message) : base(message)
        {
        }

        public UnrecognizedServiceTypeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected UnrecognizedServiceTypeException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
