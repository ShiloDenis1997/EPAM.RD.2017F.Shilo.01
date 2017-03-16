using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary.Exceptions.StatefulService
{
    /// <summary>
    /// Common exception that can be thrown by <see cref="IStatefulService"/>
    /// in case of any errors
    /// </summary>
    [Serializable]
    public class StatefulServiceException : Exception
    {
        public StatefulServiceException()
        {
        }

        public StatefulServiceException(string message) : base(message)
        {
        }

        public StatefulServiceException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected StatefulServiceException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
