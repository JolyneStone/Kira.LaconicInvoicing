using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    public class BussinessException: Exception
    {
        public BussinessException() : base() { }
        public BussinessException(string message): base(message) { }
        public BussinessException(string message, Exception innerException):base(message, innerException) { }
    }
}
