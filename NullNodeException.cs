using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ColiSys
{
    [Serializable]
    public class NullNodeException : Exception
    {
        public NullNodeException()
            : base() { }

        public NullNodeException(string message)
            : base(message) { }

        public NullNodeException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NullNodeException(string message, Exception innerException)
            : base(message, innerException) { }

        public NullNodeException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected NullNodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
