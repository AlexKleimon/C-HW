namespace Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {

            await new Server().Run();
        }
    }
}
