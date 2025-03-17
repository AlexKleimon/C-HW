using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw7
{
    interface ICalc
    {
        double Result { get; set; }
        void Sum(int x);
        void Div(int x);
        void Sub(int x);
        void Mult(int x);
        event EventHandler<EventArgs> MyEventsHandler;
    }
}
