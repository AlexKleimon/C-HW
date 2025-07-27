using Client;
using System.Net;

namespace ClientTest
{
    public class MockMessageSource : IMessageSource
    {
        private Queue<Message> messages = new();
        public MockMessageSource()
        {

            messages.Enqueue(new Message { Command = Command.Register, FromName = "Вася" });
        }

        public Message Receive(ref IPEndPoint ep)
        {
            return messages.Peek();
        }

        public void Send(Message message, IPEndPoint ep)
        {
            messages.Dequeue();
            messages.Enqueue(message);
        }
    }
    public class Tests
    {
        IMessageSource _source;
        IPEndPoint _endPoint;
        [SetUp]
        public void Setup()
        {

            _endPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        [Test]
        public void TeatRecieveMessage()
        {
            _source = new MockMessageSource();
            var result = _source.Receive(ref _endPoint);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Text, Is.Null);
            Assert.That(result.FromName, Is.Not.Null);
            Assert.That(result.FromName, Is.EqualTo("Вася"));
            Assert.That(result.Command, Is.EqualTo(Command.Register));
        }
        [Test]
        public void TestSendMessage()
        {
            _source = new MockMessageSource();
            Message mes = new Message { Command = Command.Message, FromName = "Катя", ToName = "Вася", Text = "От Кати" };
            _source.Send(mes, _endPoint);
            var result = _source.Receive(ref _endPoint);
            Assert.That(mes.Text, Is.EqualTo(result.Text));
            Assert.That(mes.FromName, Is.EqualTo(result.FromName));
            Assert.That(mes.ToName, Is.EqualTo(result.ToName));
            Assert.That(result.Command, Is.EqualTo(Command.Message));
        }
    }
}
