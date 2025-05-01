using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hw14
{
    class Chat
    {
        //Добавьте использование Cancellationtoken в код сервера, чтобы можно было правильно останавливать работу сервера.
        public static async Task Server()
        {
            CancellationTokenSource cts = new();
            CancellationToken ct = cts.Token;
            using UdpClient udpCl = new(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента...");
            //Поток для отключения сервера
            _ = Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine("Для отключения сервера введите \"Off\"");
                    string? commandServ = Console.ReadLine();
                    if (!string.IsNullOrEmpty(commandServ))
                    {
                        if (commandServ.Equals("Off"))
                        {
                            cts.Cancel();
                            break;
                        }
                        else Console.WriteLine("Данная команда не найдена.");
                    }
                }
            });
            //Прием и передача данных клиенту
            await Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        IPEndPoint LocalIpPort = new(IPAddress.Any, 0);
                        var recAsync1 = await udpCl.ReceiveAsync(ct);
                        byte[] buffer = recAsync1.Buffer;
                        LocalIpPort = recAsync1.RemoteEndPoint;
                        string message = Encoding.UTF8.GetString(buffer);
                        Message? mesClient = Message.GetMessage(message);
                        Console.WriteLine(mesClient?.ToString());
                        _ = Task.Run(async () =>
                        {
                            Message mesServer = new("Уведомление от Server", "Сообщение получено!");
                            string JsonMes = mesServer.ToJson();
                            byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                            await udpCl.SendAsync(mesToByte, LocalIpPort, ct);

                            var recAsync2 = await udpCl.ReceiveAsync(ct);
                            byte[] bufferClient = recAsync2.Buffer;
                            LocalIpPort = recAsync2.RemoteEndPoint;
                            string messageClient = Encoding.UTF8.GetString(bufferClient);
                            Message? mesForServer = Message.GetMessage(messageClient);
                            Console.WriteLine(mesForServer?.ToString());
                        });
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Сервер выключен");
                    }
                }
            }, ct);
        }

        public static void Client(string? nickName)
        {
            bool flagExit = true;
            using UdpClient udpCl = new();
            IPEndPoint localIpPort = new(IPAddress.Parse("127.0.0.1"), 12345);
            while (flagExit)
            {
                try
                {
                    Console.WriteLine("Введите сообщение (для выхода используйте команду: \"root!Exit\"):");
                    string? text = Console.ReadLine();
                    if (!string.IsNullOrEmpty(text))
                    {
                        if (text.Equals("root!Exit"))
                        {
                            Message mesExit = new($"Уведомление от {nickName}", "Отключаюсь!");
                            string JsonMesExit = mesExit.ToJson();
                            byte[] mesToByteExit = Encoding.UTF8.GetBytes(JsonMesExit);
                            udpCl.Send(mesToByteExit, localIpPort);
                            flagExit = false;
                            Thread.Sleep(2000);
                            udpCl.Close();
                            udpCl.Dispose();
                            Console.WriteLine("Готово!");
                        }
                        else
                        {
                            Message mesClient = new(nickName, text);
                            string JsonMes = mesClient.ToJson();
                            byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                            udpCl.Send(mesToByte, localIpPort);

                            byte[] bufferServer = udpCl.Receive(ref localIpPort);
                            string message = Encoding.UTF8.GetString(bufferServer);
                            Message? mesServer = Message.GetMessage(message);
                            Console.WriteLine(mesServer);

                            Message mesForServer = new($"Уведомление от {nickName}", "Уведомление получено!");
                            string JsonMesForServer = mesForServer.ToJson();
                            byte[] mesToByteForServer = Encoding.UTF8.GetBytes(JsonMesForServer);
                            udpCl.Send(mesToByteForServer, localIpPort);
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
