using ChatNetworkLib;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя:");
            string name = Console.ReadLine() ?? "unknown";
            Console.WriteLine("Введите port:");
            string port = Console.ReadLine() ?? "12346";
            var c = new Client(name, new
               MQMessageSourceClient(int.Parse(port), "127.0.0.1", 12345));
            c.Start();
        }
    }
}
