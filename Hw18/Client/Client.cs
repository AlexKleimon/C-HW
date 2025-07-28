using ChatCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        string name;
        IMQMessageSourceClient client;
        public Client(string n, IMQMessageSourceClient cl)
        {
            this.name = n;
            client = cl;
        }
        void ClientListener()
        {
            while (true)
            {
                try
                {
                    var messageReceived = client.Receive();
                    Console.WriteLine($"Получено сообщение от{messageReceived.FromName}:");
                    Console.WriteLine(messageReceived.Text);
                    Confirm(messageReceived);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении сообщения: " +
                    ex.Message);
                }
            }
        }
        void Confirm(Message m)
        {
            var message = new Message()
            {
                FromName = name,
                ToName = null,
                Text = null,
                Id = m.Id,
                Command = Command.Confirmation
            };
            client.Send(message);
        }
        void Register()
        {
            var chatMessage = new Message()
            {
                FromName = name,
                ToName = null,
                Text = null,
                Command = Command.Register
            };
            client.Send(chatMessage);
        }
        void ClientSender()
        {
            Register();
            while (true)
            {
                try
                {
                    Console.WriteLine("UDP Клиент ожидает ввода сообщения");
                    Console.Write("Введите имя получателя и сообщение и нажмите Enter: ");
                    var messages = Console.ReadLine().Split(' ');
                    var message = new Message()
                    {
                        Command = Command.Message,
                        FromName = name,
                        ToName = messages[0],
                        Text = messages[1]
                    };
                    client.Send(message);
                    Console.WriteLine("Сообщение отправлено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " +
                    ex.Message);
                }
            }
        }
        public void Start()
        {
            new Thread(() => ClientListener()).Start();
            ClientSender();
        }
    }
}

