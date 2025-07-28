using ChatNetworkLib;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            var s = new Server(new MQMessageSourceServ());
            s.Work();
        }
    }
}
