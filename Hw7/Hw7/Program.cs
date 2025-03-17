using System.Diagnostics.CodeAnalysis;

namespace Hw7
{
    internal class Program
    { //Доработайте программу калькулятор реализовав выбор действий и вывод результатов на экран в цикле
      //так чтобы калькулятор мог работать до тех пор пока пользователь не нажмет отмена или введёт пустую строку.
        static void Main(string[] args)
        {
            var calc = new Calc();
            calc.MyEventsHandler += Calc_MyEventsHandler;
            bool flag = true;
            while (flag)
            {
                Console.Write("Введите операцию (+ ,- ,/ ,* ,cancel, exit, клавиша \"Enter\"): ");
                int number;
                switch (Console.ReadLine())
                {
                    case "+":
                        Console.Write($"Введите число для сложения c {calc.Result}: ");
                        if (int.TryParse(Console.ReadLine(), out number))
                            calc.Sum(number);
                        else Console.WriteLine("Ошибка! Введенно не число!");
                        break;
                    case "-":
                        Console.Write($"Введите число для вычитания от числа {calc.Result}: ");
                        if (int.TryParse(Console.ReadLine(), out number))
                            calc.Sub(number);
                        else Console.WriteLine("Ошибка! Введенно не число!");
                        break;
                    case "/":
                        Console.Write($"Введите число для деления числа {calc.Result}: ");
                        if (int.TryParse(Console.ReadLine(), out number))
                            calc.Div(number);
                        else Console.WriteLine("Ошибка! Введенно не число!");
                        break;
                    case "*":
                        Console.Write($"Введите число для умножения числа {calc.Result}: ");
                        if (int.TryParse(Console.ReadLine(), out number))
                            calc.Mult(number);
                        else Console.WriteLine("Ошибка! Введенно не число!");
                        break;
                    case "cancel": calc.CancelLast(); break;
                    case "exit":
                        flag = false;
                        break;
                    case "":
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Данная операция не предусмотренна программой. Повторите ввод оператора.");
                        break;
                }
            }
        }

        private static void Calc_MyEventsHandler(object? sender, EventArgs e)
        {
            if (sender is Calc)
                Console.WriteLine("Результат: " + ((Calc)sender).Result);
        }
    }
}
