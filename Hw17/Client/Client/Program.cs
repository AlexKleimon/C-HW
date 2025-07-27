namespace Client
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Введите имя:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите IP:");
            string adres = Console.ReadLine();
            Console.WriteLine("Введите Port:");
            int port = int.Parse(Console.ReadLine());
            Client c = new Client(name, adres, port);
            c.Start();
        }
    }
}
