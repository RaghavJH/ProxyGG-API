using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGG
{
    class InvalidProxyTypeException : Exception
    {
        public InvalidProxyTypeException(string message) : base(message) { }
    }
}
