using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Client
{
    internal class Client
    {
        public static void Run(string? nickName)
        {
            CancellationTokenSource cts = new();
            CancellationToken ct = cts.Token;
            using UdpClient udpCl = new();
            IPEndPoint localIpPort = new(IPAddress.Parse("127.0.0.1"), 12345);
            udpCl.Client.Connect(localIpPort);
            _ = Task.Run(() => ReceiveMessage(udpCl, localIpPort, ct), ct);
            SendMessage(nickName, udpCl, localIpPort, cts);
        }
        private static void ReceiveMessage(UdpClient udpCl, IPEndPoint ipPortServer, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    byte[] buffer = udpCl.Receive(ref ipPortServer);
                    Message? mesServer = Message.GetMessage(Encoding.UTF8.GetString(buffer),TypeSerialize.Json);
                    Console.WriteLine(mesServer);
                    Console.WriteLine("Введите сообщение (используйте команду \"!help\" для вызова списка команд):");
                }
                catch (Exception)
                {
                    Console.WriteLine("Завершаю запущенные процессы...");
                }
            }
        }
        private static void SendMessage(string? nickName, UdpClient udpCl, IPEndPoint ipPortServer, CancellationTokenSource cts)
        {
            bool flagExit = true;
            while (flagExit)
            {
                try
                {
                    Console.WriteLine("Введите сообщение (используйте команду \"!help\" для вызова списка команд):");
                    string? textMes = Console.ReadLine();
                    if (!string.IsNullOrEmpty(textMes))
                    {
                        if (textMes.Equals("root!Exit"))
                        {
                            Message mesExit = new(nickName, "Уведомление: Отключаюсь!");
                            string JsonMesExit = mesExit.SetMessage(TypeSerialize.Json);
                            byte[] mesToByteExit = Encoding.UTF8.GetBytes(JsonMesExit);
                            udpCl.Send(mesToByteExit, ipPortServer);
                            flagExit = false;
                            cts.Cancel();
                            Thread.Sleep(2000);
                            udpCl.Close();
                            udpCl.Dispose();
                            Console.WriteLine("Готово!");
                        }
                        else if (textMes.Equals("!help"))
                        {
                            string textCommand = "Список команд:\n\troot!Exit - выход из приложения.\nСписок отправляемых команд для сервера (никнейм: \"Server\"):\n\t" +
                            "register - регистрация пользователя.\n\tdelete - удаление пользователя.\n\tlist-список всех пользователей.";
                            Console.WriteLine(textCommand);
                        }
                        else
                        {
                            Console.WriteLine("Введите никнейм получателя (если нажать \"Enter\" сообщение отправится всем пользователям):");
                            string? recipient = Console.ReadLine();
                            if (string.IsNullOrEmpty(recipient))
                            {
                                Message mesClient = new(nickName, textMes);
                                string JsonMes = mesClient.SetMessage(TypeSerialize.Json);
                                byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                                udpCl.Send(mesToByte, ipPortServer);
                            }
                            else
                            {
                                Message mesClient = new(nickName, recipient, textMes);
                                string JsonMes = mesClient.SetMessage(TypeSerialize.Json);
                                byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                                udpCl.Send(mesToByte, ipPortServer);
                            }
                        }
                    }
                    else { Console.WriteLine("Вы не написали сообщение. Повторите ввод."); }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
