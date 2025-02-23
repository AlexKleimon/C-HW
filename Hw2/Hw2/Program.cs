namespace Hw2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int[,] dataArray = { { 7, 3, 2 }, { 4, 9, 6 }, { 1, 8, 5 } };
            PrintArray(dataArray);
            Console.WriteLine("======================");
            int[] tempArray = new int[dataArray.GetLength(0) * dataArray.GetLength(1)];
            TransformDimensionArrays(dataArray, tempArray, true);
            SortArray(tempArray);
            TransformDimensionArrays(dataArray, tempArray, false);
            PrintArray(dataArray);
        }
        private static void PrintArray(int[,] dataArray)
        {
            for (int i = 0; i < dataArray.GetLength(0); i++)
            {
                for (int j = 0; j < dataArray.GetLength(1); j++)
                {
                    Console.Write(dataArray[i, j]);
                }
                Console.WriteLine();
            }
        }

        private static void SortArray(int[] sortArray)
        {
            int temp;
            for (int i = 0; i < sortArray.Length; i++)
            {
                for (int j = 0; j < sortArray.Length - i - 1; j++)
                {
                    if (sortArray[j] > sortArray[j + 1])
                    {
                        temp = sortArray[j];
                        sortArray[j] = sortArray[j + 1];
                        sortArray[j + 1] = temp;
                    }
                }
            }
        }

        private static void TransformDimensionArrays(int[,] twoDimensArray, int[] oneDimensArray, bool flag)
        {
            int indexArraySort = 0;
            if (flag)
            {
                for (int i = 0; i < twoDimensArray.GetLength(0); i++)
                {
                    for (int j = 0; j < twoDimensArray.GetLength(1); j++)
                    {
                        oneDimensArray[indexArraySort++] = twoDimensArray[i, j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < twoDimensArray.GetLength(0); i++)
                {
                    for (int j = 0; j < twoDimensArray.GetLength(1); j++)
                    {
                        twoDimensArray[i, j] = oneDimensArray[indexArraySort++];
                    }
                }
            }
        }
    }
}
