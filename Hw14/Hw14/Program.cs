namespace Hw14
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                Chat.Client(args[0]);

            }
            else
            {
                await Chat.Server();
            }
        }
    }
}
