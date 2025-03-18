namespace Hw8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var calc = new Calc();
            calc.MyEventsHandler += Calc_MyEventsHandler;
            bool flag = true;
            while (flag)
            {
                Console.Write("Введите операцию (+ ,- ,/ ,* ,cancel, exit, клавиша \"Enter\"): ");
                double number;
                switch (Console.ReadLine())
                {
                    case "+":
                        Console.Write($"Введите число для сложения c {calc.Result}: ");
                        if (double.TryParse(Console.ReadLine(), out number))
                            calc.Sum(number);
                        else Console.WriteLine("Ошибка! Введенно не число!");
                        break;
                    case "-":
                        Console.Write($"Введите число для вычитания от числа {calc.Result}: ");
                        if (double.TryParse(Console.ReadLine(), out number))
                            calc.Sub(number);
                        else Console.WriteLine("Ошибка! Введенно не число!");
                        break;
                    case "/":
                        Console.Write($"Введите число для деления числа {calc.Result}: ");
                        if (double.TryParse(Console.ReadLine(), out number))
                        {
                            try
                            {
                                calc.Div(number);
                            }
                            catch (DivideByZeroException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }
                        else Console.WriteLine("Ошибка! Введенно не число!");
                        break;
                    case "*":
                        Console.Write($"Введите число для умножения числа {calc.Result}: ");
                        if (double.TryParse(Console.ReadLine(), out number))
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
            if (sender is Calc calc)
                Console.WriteLine("Результат: " + calc.Result);
        }
    }
}
