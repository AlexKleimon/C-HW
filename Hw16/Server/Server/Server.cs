using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Model;

namespace Server
{
    internal class Server
    {
        //Реализуйте тип сообщений List, при котором клиент будет получать все непрочитанные сообщения с сервера.
        readonly Dictionary<string, IPEndPoint> LsRegisteredClients = new();
        readonly List<string> LsOnlineUsers = new();
        readonly List<Message> LsMissMessages = new(); //дз
        public async Task Run()
        {
            CancellationTokenSource cts = new();
            CancellationToken ct = cts.Token;
            using UdpClient udpCl = new(12345);
            using ContextDbChat ctx = new();
            ctx.Users?.ToList().ForEach(x => LsRegisteredClients.Add(x.Name ?? string.Empty, new IPEndPoint(IPAddress.Parse(x.IpAdress ?? "127.0.0.1"), x.Port)));// заполняю словарь с пользователями данными из бд
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
                        if (!string.IsNullOrEmpty(mesClient?.FromName) && mesClient.Command == Command.Exit)
                        {
                            Console.WriteLine(mesClient);
                        }
                        else Console.WriteLine($"Получено сообщение от {mesClient?.FromName} для {mesClient?.ToName ?? "всех пользователей"}.\n{mesClient?.ToString()}");
                        _ = Task.Run(async () =>
                        {
                            IPEndPoint tempIpPort = clientIpPort; // если вдруг подключится новый клиент и перезапишется LocalIpPort
                            if (!string.IsNullOrEmpty(mesClient?.ToName) && !string.IsNullOrEmpty(mesClient?.FromName)) // если указано имя получателя
                            {
                                if (mesClient.ToName.ToLower().Equals("server")) // если пришло на имя сервера
                                {
                                    string mesFromServer = string.Empty;
                                    switch (mesClient?.Command)
                                    {
                                        case Command.Register:
                                            mesFromServer = RegisterUser(mesClient, tempIpPort, ctx);
                                            break;
                                        case Command.Delete:
                                            mesFromServer = DeleteUser(mesClient, ctx);
                                            break;
                                        case Command.List:
                                            mesFromServer = ListUsers();
                                            break;
                                        case Command.CheckedOnline:
                                            await CheckLsOnlineAndMessage(udpCl, tempIpPort, mesClient, ctx, ct);
                                            mesFromServer = "Вы подключились к серверу!";
                                            break;
                                        case Command.Exit:
                                            mesFromServer = ExitUser(mesClient); // что бы при отправке сообщения всем, сервер не уходил в ексепшен, при попытке отправить сообщение пользователю который отключился
                                            break;
                                        default:
                                            mesFromServer = $"Команда {mesClient?.TextMessage} не найдена.";
                                            break;
                                    }
                                    Console.WriteLine($"Отправлен ответ на команду пользователю {mesClient?.FromName}.");
                                    await SendMessage(udpCl, tempIpPort, new("Уведомление от Server", mesFromServer), ct);
                                }
                                else if (LsRegisteredClients.TryGetValue(mesClient.ToName, out IPEndPoint? value)) //если пришло на имя пользователя
                                {
                                    if (!string.IsNullOrEmpty(mesClient.FromName) && !LsRegisteredClients.ContainsKey(mesClient.FromName))//блокировка для не зарегестрированных
                                    {
                                        Console.WriteLine("Пользователь не зарегестрирован. Рассылка отменена.");
                                        Message mesServer = new("Уведомление от Server", "Вам необходимо зарегестрироватсья, чтобы получить возможность отправлять сообщения.");
                                        await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                    }
                                    else
                                    {
                                        User? fromUser = ctx.Users?.First(x => x.Name == mesClient.FromName);
                                        User? toUser = ctx.Users?.First(x => x.Name == mesClient.ToName);
                                        MessageChat mesForDb = new()
                                        {
                                            FromUser = fromUser,
                                            ToUser = toUser,
                                            Received = false,
                                            TextMessage = mesClient.TextMessage
                                        };
                                        ctx.Add(mesForDb);
                                        ctx.SaveChanges();
                                        mesClient.Id = mesForDb.Id; // так понимаю после сохранения в базу данных получаю Id (автономерованное)
                                        if (LsOnlineUsers.Contains(mesClient.ToName))
                                        {
                                            //отправка сообщения получателю
                                            await SendMessage(udpCl, value, mesClient, ct);
                                            Console.WriteLine($"Отправленно сообщение от пользователя {mesClient.FromName} пользователю{mesClient.ToName}");
                                            //отправка уведомления отправителю
                                            Message mesServer = new("Уведомление от Server", ConfirmationMes(mesClient.Id, ctx));
                                            await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                            Console.WriteLine($"Отправлено уведомление пользователю {mesClient.FromName}");
                                        }
                                        else
                                        {
                                            LsMissMessages.Add(mesClient);//дз
                                            Message mesServer = new("Уведомление от Server", $"Пользователь {mesClient.ToName} не в сети!");
                                            await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                            Console.WriteLine($"Отправлено уведомление пользователю {mesClient.FromName}");
                                        }
                                    }
                                }
                                else
                                {
                                    Message mesServer = new("Уведомление от Server", "Пользователь не найден!");
                                    await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                }
                            }
                            else if (string.IsNullOrEmpty(mesClient?.ToName) && mesClient != null) // если не указано имя получателя
                            {
                                if (!string.IsNullOrEmpty(mesClient.FromName) && !LsRegisteredClients.ContainsKey(mesClient.FromName))//блокировка для не зарегестрированных
                                {
                                    Console.WriteLine("Пользователь не зарегестрирован. Рассылка отменена.");
                                    Message mesServer = new("Уведомление от Server", "Вам необходимо зарегестрироватсья, чтобы получить возможность отправлять сообщения.");
                                    await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                }
                                else
                                {
                                    Console.WriteLine("Рассылка уведомления пользователям...");
                                    if (LsRegisteredClients.Count == 0)
                                    {
                                        Console.WriteLine("На данный момент количество зарегестрированных пользоваталей равно 0. Рассылка отменена.");
                                        Message mesServer = new("Уведомление от Server", "На данный момент количество зарегестрированных пользоваталей: 0...=(");
                                        await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                    }
                                    else if (LsOnlineUsers.Count == 0)
                                    {
                                        Console.WriteLine("На данный момент пользователей онлайн 0. Рассылка отменена.");
                                        Message mesServer = new("Уведомление от Server", "На данный момент пользователей онлайн: 0...=(");
                                        await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                    }
                                    else
                                    {
                                        foreach (var item in LsRegisteredClients)
                                        {
                                            if (!item.Key.Equals(mesClient.FromName))
                                            {
                                                mesClient.ToName = item.Key;
                                                User? fromUser = ctx.Users?.First(x => x.Name == mesClient.FromName);
                                                User? toUser = ctx.Users?.First(x => x.Name == mesClient.ToName);
                                                MessageChat mesForDb = new()
                                                {
                                                    FromUser = fromUser,
                                                    ToUser = toUser,
                                                    Received = false,
                                                    TextMessage = mesClient.TextMessage
                                                };
                                                ctx.Add(mesForDb);
                                                ctx.SaveChanges();
                                                mesClient.Id = mesForDb.Id;
                                                if (LsOnlineUsers.Contains(item.Key))
                                                {
                                                    await SendMessage(udpCl, item.Value, mesClient, ct);
                                                    Console.WriteLine($"Отправленно сообщение от пользователя {mesClient.FromName} пользователю: {item.Key}, {item.Value.Address}:{item.Value.Port}");
                                                    Message mesServer = new("Уведомление от Server", ConfirmationMes(mesClient.Id, ctx));
                                                    await SendMessage(udpCl, tempIpPort, mesServer, ct);
                                                }
                                                else
                                                {
                                                    LsMissMessages.Add((Message)mesClient.Clone());//дз
                                                    await SendMessage(udpCl, tempIpPort, new Message("Уведомление от Server", $"Пользователь {item.Key} не в сети!"), ct);
                                                    Console.WriteLine($"Отправлено уведомление пользователю {mesClient.FromName}");
                                                }
                                            }
                                        }
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
        private string RegisterUser(Message mesClient, IPEndPoint clientIpPort, ContextDbChat ctx)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(mesClient?.FromName))
            {
                if (LsRegisteredClients.TryAdd(mesClient.FromName, new IPEndPoint(clientIpPort.Address, clientIpPort.Port)))
                {
                    if (ctx.Users?.FirstOrDefault(x => x.Name == mesClient.FromName) == null)
                    {
                        ctx.Users?.Add(new User { Name = mesClient.FromName, IpAdress = LsRegisteredClients[mesClient.FromName].Address.ToString(), Port = LsRegisteredClients[mesClient.FromName].Port });//???? косячок
                        ctx.SaveChanges();
                        msg = $"Регистрация {mesClient.FromName} прошла успешно.";
                    }
                    else
                    {
                        msg = $"Пользователь с именем {mesClient.FromName} уже существует.";
                    }
                }
                else
                {
                    msg = $"Пользователь с именем {mesClient.FromName} уже существует.";
                }
            }
            return msg;
        }
        private string DeleteUser(Message mesClient, ContextDbChat ctx)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(mesClient?.FromName))
            {
                if (LsRegisteredClients.Remove(mesClient.FromName))
                {
                    if (ctx.Users?.FirstOrDefault(x => x.Name == mesClient.FromName) != null)
                    {
                        ctx.Users?.Remove(new User { Name = mesClient.FromName });
                        ctx.SaveChanges();
                        msg = $"Пользователь {mesClient.FromName} успешно удален.";
                    }
                    else
                    {
                        msg = $"Пользователь с именем {mesClient.FromName} не зарегестрирован.";
                    }
                }
                else
                {
                    msg = $"Пользователь с именем {mesClient.FromName} не зарегестрирован.";
                }
            }
            return msg;
        }
        private string ListUsers()
        {
            string msg = "Список пользователей: ";
            if (LsRegisteredClients.Count > 0)
            {
                foreach (var item in LsRegisteredClients)
                {
                    msg += $"| {item.Key} :: {item.Value.Address}:{item.Value.Port} |";
                }
            }
            else
                msg = "На данный момент количество зарегестрированных пользоваталей равно 0";
            return msg;
        }
        private static string ConfirmationMes(int? idMessage, ContextDbChat ctx) // дз
        {
            string msg = string.Empty;
            MessageChat? messConf = ctx.Messages?.FirstOrDefault(x => x.Id == idMessage);
            if (messConf != null)
            {
                messConf.Received = true;
                msg = $"Сообщение пользователем {messConf?.ToUser?.Name} прочитано!";
                ctx.SaveChanges();
            }
            else
            {
                msg = "Внутреняя ошибка: Сообщения не существует.";
            }
            return msg;
        }
        private async Task CheckLsOnlineAndMessage(UdpClient udpCl, IPEndPoint sendIPPort, Message mesClient, ContextDbChat ctx, CancellationToken ct) //дз
        {
            if (!string.IsNullOrEmpty(mesClient?.FromName) && !LsOnlineUsers.Contains(mesClient.FromName))
            {
                LsOnlineUsers.Add(mesClient.FromName);
                foreach (Message missMessage in LsMissMessages)
                {
                    if (missMessage.ToName == mesClient.FromName)
                    {
                        await SendMessage(udpCl, sendIPPort, missMessage, ct);
                        string msg = ConfirmationMes(missMessage.Id, ctx);
                        if (!string.IsNullOrEmpty(missMessage.FromName))
                            if (LsRegisteredClients.TryGetValue(missMessage.FromName, out IPEndPoint? IpFromUser))
                            {
                                await SendMessage(udpCl, IpFromUser, new Message("Уведомление от Server", msg), ct);
                                LsMissMessages.Remove(missMessage);
                            }
                    }
                }
            }
        }
        private string ExitUser(Message mesClient)
        {
            if (!string.IsNullOrEmpty(mesClient?.FromName))
                LsOnlineUsers.Remove(mesClient.FromName);
            return "Вы отключились от сервера!";
        }
        private static async Task SendMessage(UdpClient udpCl, IPEndPoint sendIPPort, Message mesClient, CancellationToken ct)
        {
            byte[] mesToByte = Encoding.UTF8.GetBytes(mesClient.SetMessage(TypeSerialize.Json));
            await udpCl.SendAsync(mesToByte, sendIPPort, ct);
        }
    }
}

