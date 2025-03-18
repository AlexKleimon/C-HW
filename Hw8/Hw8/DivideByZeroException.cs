using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw8
{
    class DivideByZeroException : Exception
    {
        public DivideByZeroException()
        {
        }
        public DivideByZeroException(string? message) : base(message)
        {
        }
    }
}
