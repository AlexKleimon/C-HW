namespace Hw10
{
    internal class Program
    {
        //Объедините две предыдущих работы (практические работы 2 и 3): поиск файла и поиск текста в файле написав утилиту которая
        //ищет файлы определенного расширения с указанным текстом. Рекурсивно. Пример вызова утилиты: utility.exe txt текст.
        static void Main(string[] args)
        {
            List<string> listPathFiles = [];

            HomeWork.FindFiles("C:\\Users\\MSI\\Desktop\\C#\\", $"*.{args[0]}", listPathFiles);

            foreach (string file in listPathFiles)
            {
                if (HomeWork.FindWord(file, args[1]) == true)
                {
                    Console.WriteLine(file);
                }
            }
        }
    }
}
