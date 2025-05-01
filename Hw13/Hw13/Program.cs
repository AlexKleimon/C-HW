namespace Hw13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Chat.Client(args[0]);

            }
            else
            {
                Chat.Server();
            }
        }
    }
}
