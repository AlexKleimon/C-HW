using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw8
{
    interface ICalc
    {
        double Result { get; set; }
        void Sum(double x);
        void Div(double x);
        void Sub(double x);
        void Mult(double x);
        event EventHandler<EventArgs> MyEventsHandler;
    }
}
