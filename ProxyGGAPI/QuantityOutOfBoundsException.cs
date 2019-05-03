using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGG
{
    class QuantityOutOfBoundsException : Exception
    {
        public QuantityOutOfBoundsException(string message) : base(message) { }
    }
}
