
using System.Net.Sockets;
using System.Net;
using System.Text;


namespace Client
{
    public class Client
    {
        IMessageSource messageSource;
        string name;
        string address;
        int port;
        bool flag = true;
        public Client(string n, string a, int p)
        {
            name = n;
            address = a;
            port = p;
        }
        void ClientListener()
        {
            IPEndPoint remoteEndPoint = new
            IPEndPoint(IPAddress.Parse(address), 12345);
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(address), 12345);
            while (true)
            {
                try
                {
                    var messageReceived = messageSource.Receive(ref
                    remoteEndPoint);
                    Console.WriteLine($"Получено сообщение от {messageReceived.FromName}:");
                    Console.WriteLine(messageReceived.Text);
                    Confirm(messageReceived, remoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении сообщения: " +
                    ex.Message);
                }
            }
        }
        void Confirm(Message m, IPEndPoint remoteEndPoint)
        {
            var messageJson = new Message()
            {
                FromName = name,
                ToName = null,
                Text = null,
                Id = m.Id,
                Command = Command.Confirmation
            };
            messageSource.Send(messageJson, remoteEndPoint);
        }
        void Register(IPEndPoint remoteEndPoint)
        {
            var messageJson = new Message()
            {
                FromName = name,
                ToName = null,
                Text = null,
                Command = Command.Register
            };
            messageSource.Send(messageJson, remoteEndPoint);
        }
        void ClientSender()
        {
            IPEndPoint remoteEndPoint = new
            IPEndPoint(IPAddress.Parse(address), 12345);
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(address), 12345);
            Register(remoteEndPoint);
            while (true)
            {
                try
                {
                    Console.WriteLine("UDP Клиент ожидает ввода сообщения");
                    Console.Write("Введите имя получателя и сообщение и нажмите Enter: ");
                    var messages = Console.ReadLine().Split(' ');
                    var messageJson = new Message()
                    {
                        Command =
                    Command.Message,
                        FromName = name,
                        ToName = messages[0],
                        Text = messages[1]
                    };
                    messageSource.Send(messageJson, remoteEndPoint);
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
            if (flag)
            {
                messageSource = new UdpMessageSource(port);
                new Thread(() => ClientListener()).Start();
                ClientSender();
            }
        }
        public void Stop()
        {
            flag = false;
        }
    }
}
