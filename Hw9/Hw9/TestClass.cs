using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw9
{
    class TestClass
    {
        [CustomName("CustomFieldName")]
        public int I { get; set; }
        private string? S { get; set; }
        
        public decimal D { get; set; }
        public char[]? C { get; set; }

        public TestClass()
        { }
        public TestClass(int i)
        {
            this.I = i;
        }
        public TestClass(int i, string s, decimal d, char[] c) : this(i)
        {
            this.S = s;
            this.D = d;
            this.C = c;
        }
    }
}
