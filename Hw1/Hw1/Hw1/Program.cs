namespace Hw1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //написать программу калькулятор, вычисляющий выражения вида a + b - c * d + e / f и прочие,
            //введенные из командной строки выводящую результат выполнения на экран
            List<string> experession = [.. args];
            if (experession[0].Equals("-"))
            {
                experession.Add(experession[0]);
                experession.RemoveAt(0);
            }
            int[] valuesQuant = QuantOperation(experession);
            string[] charOperat = ["/", "*", "+", "-"];
            for (int i = 0; i < valuesQuant.Length; i++)
            {
                for (int j = 0; j < valuesQuant[i]; j++)
                {
                    int index = experession.IndexOf(charOperat[i]);
                    experession[index] = MathOperations(experession[index - 1], experession[index + 1], charOperat[i]);
                    experession.RemoveAt(index + 1);
                    experession.RemoveAt(index - 1);
                }

            }
            foreach (var item in experession)
            {
                Console.WriteLine(item);
            }
        }

        private static string MathOperations(string num1, string num2, string opreation)
        {
            string result = string.Empty;
            switch (opreation)
            {
                case "+":
                    result = Convert.ToString(int.Parse(num1) + int.Parse(num2));
                    break;
                case "-":
                    result = Convert.ToString(int.Parse(num1) - int.Parse(num2));
                    break;
                case "*":
                    result = Convert.ToString(int.Parse(num1) * int.Parse(num2));
                    break;
                case "/":
                    result = Convert.ToString(int.Parse(num1) / int.Parse(num2));
                    break;
            }
            return result;

        }

        private static int[] QuantOperation(List<string> experession)
        {
            int[] valuesQuant = new int[4];
            foreach (var item in experession)
            {
                switch (item)
                {
                    case "/":
                        valuesQuant[0]++;
                        break;
                    case "*":
                        valuesQuant[1]++;
                        break;
                    case "+":
                        valuesQuant[2]++;
                        break;
                    case "-":
                        valuesQuant[3]++;
                        break;
                }
            }
            return valuesQuant;
        }
    }
}
