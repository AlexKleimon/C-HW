using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hw13
{
    class Chat
    {
        //Добавьте возможность ввести слово Exit в чате клиента, чтобы можно было завершить его работу.
        //В коде сервера добавьте ожидание нажатия клавиши, чтобы также прекратить его работу.
        public static void Server()
        {
            bool flag = true;
            IPEndPoint LocalIpPort = new(IPAddress.Any, 0);
            using UdpClient udpCl = new(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента...");
            //Поток для отключения сервера
            new Thread(() =>
            {
                while (flag)
                {
                    Console.WriteLine("Для отключения сервера введите \"Off\"");
                    string? commandServ = Console.ReadLine();
                    if (!string.IsNullOrEmpty(commandServ))
                    {
                        if (commandServ.Equals("Off"))
                        {
                            flag = false;
                            udpCl.Close();
                            udpCl.Dispose();
                        }
                        else Console.WriteLine("Данная команда не найдена.");
                    }
                }
            }).Start();
            //Прием и передача данных клиенту
            while (flag)
            {
                try
                {
                    byte[] buffer = udpCl.Receive(ref LocalIpPort);
                    string message = Encoding.UTF8.GetString(buffer);
                    new Thread(() =>
                    {
                        try
                        {
                            Message? mesClient = Message.GetMessage(message);
                            if (mesClient == null)
                            {
                                Console.WriteLine("Некорректное сообщение");
                            }
                            else
                            {
                                Console.WriteLine(mesClient.ToString());

                                Message mesServer = new("Уведомление от Server", "Сообщение получено!");
                                string JsonMes = mesServer.ToJson();
                                byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                                udpCl.Send(mesToByte, LocalIpPort);

                                byte[] bufferClient = udpCl.Receive(ref LocalIpPort);
                                string messageClient = Encoding.UTF8.GetString(bufferClient);
                                Message? mesForServer = Message.GetMessage(messageClient);
                                Console.WriteLine(mesForServer?.ToString());
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Подключение закрыто");
                        }
                    }).Start();
                }
                catch (Exception)
                {
                    Thread.Sleep(300);
                    Console.WriteLine("Сервер выключен");
                }
            }
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
                    Console.WriteLine("Введите сообщение:");
                    string? text = Console.ReadLine();
                    if (!string.IsNullOrEmpty(text))
                    {
                        // Выход с клиентского приложения
                        if (text.Equals("Exit"))
                        {
                            Message mesExit = new($"Уведомление от {nickName}", "Отключаюсь!");
                            string JsonMesExit = mesExit.ToJson();
                            byte[] mesToByteExit = Encoding.UTF8.GetBytes(JsonMesExit);
                            udpCl.Send(mesToByteExit, localIpPort);
                            flagExit = false;
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
}
