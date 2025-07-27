using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Client
    {
        public static void Run(string? nickName, int port)
        {
            CancellationTokenSource cts = new();
            CancellationToken ct = cts.Token;
            using UdpClient udpCl = new(port);
            IPEndPoint localIpPort = new(IPAddress.Parse("127.0.0.1"), 12345);
            udpCl.Client.Connect(localIpPort);
            _ = Task.Run(() => ReceiveMessage(nickName, udpCl, localIpPort, ct), ct);
            SendMessage(nickName, udpCl, localIpPort, cts);
        }
        private static void ReceiveMessage(string? nickName, UdpClient udpCl, IPEndPoint ipPortServer, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    byte[] buffer = udpCl.Receive(ref ipPortServer);
                    Message? mesServer = Message.GetMessage(Encoding.UTF8.GetString(buffer), TypeSerialize.Json);
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
            Message mesOnline = new(nickName, "Server", "Я онлайн!", Command.CheckedOnline);
            string JsonMesOnline = mesOnline.SetMessage(TypeSerialize.Json);
            byte[] mesOnlToByte = Encoding.UTF8.GetBytes(JsonMesOnline);
            udpCl.Send(mesOnlToByte, ipPortServer);
            while (flagExit)
            {
                try
                {
                    string textHelp = "Список команд:\n\troot!Exit - выход из приложения.\n\tmessage-написать сообщение." +
                                        "\nСписок отправляемых команд для сервера (никнейм: \"Server\"):\n\t" +
                                        "register - регистрация пользователя.\n\tdelete - удаление пользователя.\n\tlist-список всех пользователей.\n\tonline-список пользователей в сети.";
                    Console.WriteLine(textHelp);
                    string? command = Console.ReadLine();
                    if (!string.IsNullOrEmpty(command))
                    {
                        if (command.Equals("root!Exit"))
                        {
                            Message mesExit = new(nickName, "Server", "Я оффлайн!");
                            mesExit.Command = Command.Exit;
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
                        else if (command.Equals("message"))
                        {
                            Console.WriteLine("Введите никнейм получателя или никнейм \"Server\" для отправки команды" +
                                "(если нажать \"Enter\" сообщение отправится всем пользователям):");
                            string? recipient = Console.ReadLine();
                            Console.WriteLine("Введите сообщение:");
                            string? textMes = Console.ReadLine();
                            if (string.IsNullOrEmpty(recipient))
                            {
                                Message mesClient = new(nickName, recipient, textMes);
                                string JsonMes = mesClient.SetMessage(TypeSerialize.Json);
                                byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                                udpCl.Send(mesToByte, ipPortServer);
                            }
                            else
                            {
                                if (recipient.ToLower().Equals("server"))
                                {
                                    Message mesClient = SendCommandServer(nickName, recipient, textMes);
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
                        else
                        {
                            Console.WriteLine($"Команда {command} не найдена.");
                        }
                    }
                    else { Console.WriteLine("Вы не написали команду. Повторите ввод."); }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private static Message SendCommandServer(string? nickName, string? recipient, string? textMes)
        {
            Message message = new();
            bool flag = true;
            while (flag)
            {
                switch (textMes?.ToLower())
                {
                    case "register":
                        message = new(nickName, recipient, textMes, Command.Register);
                        flag = false;
                        break;
                    case "delete":
                        message = new(nickName, recipient, textMes, Command.Delete);
                        flag = false;
                        break;
                    case "list":
                        message = new(nickName, recipient, textMes, Command.List);
                        flag = false;
                        break;
                    default:
                        Console.WriteLine($"Команда {textMes?.ToLower()} не найдена.");
                        break;
                }
            }
            return message;
        }
    }
}