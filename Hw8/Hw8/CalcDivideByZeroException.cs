using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw8
{
    class CalcDivideByZeroException : Exception
    {
        public CalcDivideByZeroException()
        {
        }
        public CalcDivideByZeroException(string? message) : base(message)
        {
        }
    }
}
