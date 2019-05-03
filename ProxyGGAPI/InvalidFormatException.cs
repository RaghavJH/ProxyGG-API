using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGG
{
    class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message) : base(message) { }
    }
}
