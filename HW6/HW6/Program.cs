namespace HW6
{
    internal class Program
    {
        //Дан массив и число. Найдите три числа в массиве сумма которых равна искомому числу.
        //Подсказка: если взять первое число в массиве, можно ли найти в оставшейся его части два числа равных по сумме первому.
        static void Main(string[] args)
        {
            int[] arrayNumbers = { -1, 5, 10, 13, 2, -3, 4, 6 };
            int target = 13;
            for (int i = 0; i < arrayNumbers.Length; i++)
            {
                for(int j = 0; j < arrayNumbers.Length; j++)
                {
                    for (int k = 0; k < arrayNumbers.Length; k++)
                    {
                        if(i==j && j == k || j==k || i==k || i==j) { continue; }
                        if (arrayNumbers[i] + arrayNumbers[j] + arrayNumbers[k] == target)
                        {
                            Console.WriteLine($"Число {target} = {arrayNumbers[i]} + {arrayNumbers[j]} + {arrayNumbers[k]}");
                            return;
                        }
                    }
                }
            }
            Console.WriteLine("Числа не найдены");

        }
    }
}
