namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Введите свой никнейм:");
            string? nickName = Console.ReadLine();
            Console.WriteLine("Введите порт:");
            int port = int.Parse(Console.ReadLine() ?? "12346");
            Client.Run(nickName, port);
        }
    }
}
