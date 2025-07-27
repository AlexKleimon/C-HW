using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Server
{
    internal class Server
    {
        //Factory method/Abstract factory - реализуйте несколько типов протокола например использующий XML/JSON сериализацию.
        //Первая версия чата может работать с XML, тогда как вторая JSON.
        //Реализуйте конструирование сообщений посредством указанных шаблонов.
        public static async Task Run()
        {
            CancellationTokenSource cts = new();
            CancellationToken ct = cts.Token;
            using UdpClient udpCl = new(12345);
            Dictionary<string, IPEndPoint> listClients = []; // поменять местами. удаление по IP
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
                        var recAsync1 = await udpCl.ReceiveAsync(ct);
                        byte[] buffer = recAsync1.Buffer;
                        IPEndPoint clientIpPort = recAsync1.RemoteEndPoint;
                        string message = Encoding.UTF8.GetString(buffer);
                        Message? mesClient = Message.GetMessage(message, TypeSerialize.Json);
                        if (!string.IsNullOrEmpty(mesClient?.FromName) && mesClient.TextMessage.Equals("Уведомление: Отключаюсь!"))
                        {
                            listClients.Remove(mesClient.FromName); // что бы при отправке сообщения всем, сервер не уходил в ексепшен, при попытке отправить сообщение пользователю который отключился (временное решение)
                            Console.WriteLine(mesClient);
                        }
                        else Console.WriteLine($"Получено сообщение от {mesClient?.FromName} для {mesClient?.ToName ?? "всех пользователей"}.\n{mesClient?.ToString()}");
                        _ = Task.Run(async () =>
                        {
                            IPEndPoint tempIpPort = clientIpPort; // если вдруг подключится новый клиент и перезапишется LocalIpPort
                            if (!string.IsNullOrEmpty(mesClient?.ToName) && !string.IsNullOrEmpty(mesClient?.FromName)) // если указано имя получателя
                            {
                                if (mesClient.ToName.Equals("Server"))
                                {
                                    string mesFromServer = string.Empty;
                                    switch (mesClient?.TextMessage)
                                    {
                                        case "register":
                                            if (listClients.TryAdd(mesClient.FromName, new IPEndPoint(clientIpPort.Address, clientIpPort.Port)))
                                                mesFromServer = $"Регистрация {mesClient.FromName} прошла успешно.";
                                            else mesFromServer = $"Пользователь с именем {mesClient.FromName} уже существует";
                                            break;
                                        case "delete":
                                            if (listClients.Remove(mesClient.FromName))
                                                mesFromServer = $"Пользователь {mesClient.FromName} успешно удален.";
                                            else mesFromServer = $"Пользователь с именем {mesClient.FromName} не зарегестрирован.";
                                            break;
                                        case "list":
                                            mesFromServer = "Список пользователей: ";
                                            foreach (var item in listClients)
                                            {
                                                mesFromServer += $"| {item.Key} :: {item.Value.Address}:{item.Value.Port} |";
                                            }
                                            break;
                                        default:
                                            mesFromServer = $"Команда {mesClient?.TextMessage} не найдена.";
                                            break;
                                    }
                                    Message mesServer = new("Уведомление от Server", mesFromServer);
                                    Console.WriteLine(mesFromServer);
                                    byte[] mesToByte = Encoding.UTF8.GetBytes(mesServer.SetMessage(TypeSerialize.Json));
                                    await udpCl.SendAsync(mesToByte, tempIpPort, ct);
                                }
                                else if (listClients.TryGetValue(mesClient.ToName, out IPEndPoint? value))
                                {
                                    //отправка сообщения получателю
                                    byte[] mesToByteClient = Encoding.UTF8.GetBytes(mesClient.SetMessage(TypeSerialize.Json));
                                    await udpCl.SendAsync(mesToByteClient, value, ct);
                                    Console.WriteLine($"Отправлено сообщение пользователю {mesClient.ToName}");
                                    //отправка уведомления отправителю
                                    Message mesServer = new("Уведомление от Server", "Сообщение отправлено!");
                                    byte[] mesToByte = Encoding.UTF8.GetBytes(mesServer.SetMessage(TypeSerialize.Json));
                                    await udpCl.SendAsync(mesToByte, tempIpPort, ct);
                                    Console.WriteLine($"Отправлено уведомление пользователю {mesClient.FromName}");
                                }
                                else
                                {
                                    Message mesServer = new("Уведомление от Server", "Пользователь не найден!");
                                    byte[] mesToByte = Encoding.UTF8.GetBytes(mesServer.SetMessage(TypeSerialize.Json));
                                    await udpCl.SendAsync(mesToByte, tempIpPort, ct);
                                }
                            }
                            else if (string.IsNullOrEmpty(mesClient?.ToName) && mesClient != null) // если не указано имя получателя
                            {
                                Console.WriteLine("Рассылка уведомления пользователям...");
                                if (listClients.Count == 0)
                                {
                                    Console.WriteLine("На данный момент количество зарегестрированных пользоваталей равно 0. Рассылка отменена.");
                                }
                                foreach (var item in listClients)
                                {
                                    if (!item.Key.Equals(mesClient.FromName))
                                    {
                                        byte[] mesToByteClients = Encoding.UTF8.GetBytes(mesClient.SetMessage(TypeSerialize.Json));
                                        Console.WriteLine($"Отправленно: {item.Key}, {item.Value.Address}:{item.Value.Port}");
                                        await udpCl.SendAsync(mesToByteClients, item.Value, ct);
                                    }
                                }
                            }
                        });
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Сервер выключен!");
                    }
                }
            }, ct);
        }
    }
}