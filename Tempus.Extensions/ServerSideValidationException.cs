namespace Tempus.Utils
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	
    public class ServerSideValidationException : Exception
    {
        public ServerSideValidationException() : base() { }

        public ServerSideValidationException(string message) : base(message) { }

        public ServerSideValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
