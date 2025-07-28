using ChatCommonLib;
using ChatDbLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        Dictionary<string, string> clients = new();
        IMQMessageSourceServ messageSource;
        public Server(IMQMessageSourceServ source)
        {
            messageSource = source;
        }
        void Register(Message message, string clientId)
        {
            Console.WriteLine("Message Register, name = " + message.FromName);
            clients.Add(message.FromName, clientId);
            using (var ctx = new ContextDb())
            {
                if (ctx.Users.FirstOrDefault(x => x.Name == message.FromName) !=
                null) return;
                ctx.Add(new User { Name = message.FromName });
                ctx.SaveChanges();
            }
        }
        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);
            using (var ctx = new ContextDb())
            {
                var msg = ctx.Messages.FirstOrDefault(x => x.Id == id);
                if (msg != null)
                {
                    msg.IsReceived = true;
                    ctx.SaveChanges();
                }
            }
        }
        void RelyMessage(Message message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName ?? " ", out string? clientId))
            {
                using (var ctx = new ContextDb())
                {
                    var fromUser = ctx.Users.First(x => x.Name ==
                    message.FromName);
                    var toUser = ctx.Users.First(x => x.Name == message.ToName);
                    var msg = new MessageDb
                    {
                        FromUser = fromUser,
                        ToUser = toUser,
                        IsReceived = false,
                        Text = message.Text
                    };
                    ctx.Messages.Add(msg);
                    ctx.SaveChanges();
                    id = msg.Id;
                }
                var forwardMessage = new Message()
                {
                    Id = id,
                    Command =
                Command.Message,
                    ToName = message.ToName,
                    FromName = message.FromName,
                    Text =
                message.Text
                };
                messageSource.Send(forwardMessage, clientId);
                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }
        void ProcessMessage(Message message, string fromId)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой {message.Command}:");
            Console.WriteLine(message.Text);
            if (message.Command == Command.Register)
            {
                Register(message, fromId);
            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }
        }
        bool work = true;
        public void Stop()
        {
            work = false;
        }
        public void Work()
        {
            Console.WriteLine("UDP Клиент ожидает сообщений...");
            while (work)
            {
                try
                {
                    string? clientId = string.Empty;
                    var message = messageSource.Receive(ref clientId);
                    if (message == null || string.IsNullOrEmpty(clientId))
                        return;
                    else
                        ProcessMessage(message, clientId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " +
                    ex.Message);
                }
            }
        }
    }
}

