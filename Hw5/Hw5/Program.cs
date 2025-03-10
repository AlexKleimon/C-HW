namespace Hw5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] labirynth = new int[,]{
            {1, 1, 1, 1, 1, 1, 1 },
            {1, 0, 0, 0, 0, 0, 1 },
            {1, 0, 1, 1, 1, 0, 1 },
            {2, 0, 0, 0, 1, 0, 2 },
            {1, 1, 0, 0, 1, 1, 1 },
            {1, 1, 1, 0, 1, 1, 1 },
            {1, 1, 1, 2, 1, 1, 1 }};
            /*Доработайте приложение поиска пути в лабиринте, но на этот раз вам нужно определить сколько всего выходов имеется в лабиринте:*/
            Console.WriteLine("Выходов в лабиринте: " + HasExix(5, 1, labirynth));

        }
        static public int HasExix(int startI, int startJ, int[,] l)
        {
            int count = 0;

            var stack = new Stack<Tuple<int, int>>();
            if (l[startI, startJ] != 1)
                stack.Push(new(startI, startJ));
            else stack.Push(new(3, 1)); // точка входа по умолчанию (если попали в стену)

            while (stack.Count > 0)
            {
                (int i,int j) = stack.Pop();

                if (l[i, j] == 2)
                    count++;

                l[i, j] = 1;

                if (j-1 >= 0 && l[i, j - 1] != 1)
                    stack.Push(new(i, j-1));

                if (j + 1 < l.GetLength(1) && l[i, j + 1] != 1)
                    stack.Push(new(i, j + 1));

                if (i-1 >= 0 && l[i - 1, j] != 1)
                    stack.Push(new(i - 1, j));

                if (i + 1 < l.GetLength(0) && l[i + 1, j] != 1)
                    stack.Push(new(i + 1, j));
            }
            return count;

        }
    }
}
