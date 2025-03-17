using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw7
{
    class Calc : ICalc
    {
        public double Result { get; set; } = 0;
        private Stack<double> LastResult = new(); 

        public event EventHandler<EventArgs> MyEventsHandler;
        private void PrintResult()
        {
            MyEventsHandler?.Invoke(this, new EventArgs());
        }
        public void Div(int x)
        {
            Result /= x;
            PrintResult();
            LastResult.Push(Result);
        }

        public void Mult(int x)
        {
            Result *= x;
            PrintResult();
            LastResult.Push(Result);
        }

        public void Sub(int x)
        {
            Result -= x;
            PrintResult();
            LastResult.Push(Result);
        }

        public void Sum(int x)
        {
            Result += x;
            PrintResult();
            LastResult.Push(Result);
        }
        public void CancelLast()
        {
            if (LastResult.TryPop(out double res1))
            {
                LastResult.TryPop(out double res2);
                Result = res2;
                Console.WriteLine($"Отмененно последнее действие.Результат был: {res1}, стал: {res2}.");
            }
            else Console.WriteLine("Нельзя отменить последнее действие.");

        }
    }
}
