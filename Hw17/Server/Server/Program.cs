namespace Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var s = new Server(new UdpMessageSource());
            s.Work();
        }
    }
}
