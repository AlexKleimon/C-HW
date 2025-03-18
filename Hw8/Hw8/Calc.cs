using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw8
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

        public void Sum(double x)
        {
            Result += x;
            PrintResult();
            LastResult.Push(Result);
        }

        public void Div(double x)
        {
            if (x != 0)
            {
                Result /= x;
                PrintResult();
                LastResult.Push(Result);
            }
            else
            {
                throw new DivideByZeroException("Деление на ноль! Ошибка!");
            }
        }

        public void Sub(double x)
        {
            Result -= x;
            PrintResult();
            LastResult.Push(Result);
        }

        public void Mult(double x)
        {
            Result *= x;
            PrintResult();
            LastResult.Push(Result);
        }
    }
}
