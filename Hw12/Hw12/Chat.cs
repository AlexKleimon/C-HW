using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hw12
{
    class Chat
    {
        //Попробуйте переработать приложение, добавив подтверждение об отправке сообщений как в сервер, так и в клиент.
        public static void Server()
        {
            IPEndPoint LocalIpPort = new(IPAddress.Any, 0);
            UdpClient udpCl = new(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента");
            while (true)
            {
                try
                {
                    //считываем сообщение от клиента
                    byte[] buffer = udpCl.Receive(ref LocalIpPort);
                    string message = Encoding.UTF8.GetString(buffer);
                    Message? mesClient = Message.GetMessage(message);
                    if (mesClient == null)
                    {
                        Console.WriteLine("Некорректное сообщение");
                    }
                    else
                    {
                        Console.WriteLine(mesClient.ToString());
                        //отвечаем клиенту что сообщение получено
                        Message mesServer = new("Увидомление от Server", "Сообщение получено!");
                        string JsonMes = mesServer.ToJson();
                        byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                        udpCl.Send(mesToByte, LocalIpPort);
                        //считываем сообщение от клиента (уведомление от сервер получено)
                        byte[] bufferClient = udpCl.Receive(ref LocalIpPort);
                        string messageClient = Encoding.UTF8.GetString(bufferClient);
                        Message? mesForServer = Message.GetMessage(messageClient);
                        Console.WriteLine(mesForServer?.ToString());
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }
        }
        public static void Client(string? nickName)
        {
            IPEndPoint localIpPort = new(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient udpCl = new();
            while (true)
            {
                try
                {
                    //формируем и отправляем сообщение серверу
                    Console.WriteLine("Введите сообщение:");
                    string? text = Console.ReadLine();
                    if (string.IsNullOrEmpty(text))
                    {
                        break;
                    }
                    Message mesClient = new(nickName, text);
                    string JsonMes = mesClient.ToJson();
                    byte[] mesToByte = Encoding.UTF8.GetBytes(JsonMes);
                    udpCl.Send(mesToByte, localIpPort);
                    // считываем уведомление от сервреа
                    byte[] bufferServer = udpCl.Receive(ref localIpPort);
                    string message = Encoding.UTF8.GetString(bufferServer);
                    Message? mesServer = Message.GetMessage(message);
                    Console.WriteLine(mesServer);
                    // отправляем сообщение что уведомление от сервера получено
                    Message mesForServer = new($"Увидомление от {nickName}", "Уведомление получено!");
                    string JsonMesForServer = mesForServer.ToJson();
                    byte[] mesToByteForServer = Encoding.UTF8.GetBytes(JsonMesForServer);
                    udpCl.Send(mesToByteForServer, localIpPort);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
