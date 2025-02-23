using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw4
{
    interface IBitNumber
    {
        public void SetBit(int index, bool bit);
        public bool GetBit(int index);
    }
}
