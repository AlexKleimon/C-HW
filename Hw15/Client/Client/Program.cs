namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Введите свой никнейм:");
            Client.Run(Console.ReadLine());
        }
    }
}
