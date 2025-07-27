using System.Text.Json;
using System.Xml.Serialization;

namespace Client
{
    internal abstract class FactoryMethodMessage
    {
        public abstract string SetMessage(Message message);
        public abstract Message? GetMessage(string message);
    }
    internal class JsonMethods : FactoryMethodMessage
    {
        public override string SetMessage(Message message)
        {
            return JsonSerializer.Serialize(message);
        }
        public override Message? GetMessage(string message)
        {
            return JsonSerializer.Deserialize<Message>(message);
        }
    }
    internal class XmlMethods : FactoryMethodMessage
    {
        public override string SetMessage(Message message)
        {
            XmlSerializer serializer = new(typeof(Message));
            using StringWriter sw = new();
            serializer.Serialize(sw, message);
            return sw.ToString();
        }
        public override Message? GetMessage(string message)
        {
            XmlSerializer serializer = new(typeof(Message));
            return (Message?)serializer.Deserialize(new StringReader(message));
        }
    }

    internal abstract class Creator
    {
        public abstract FactoryMethodMessage FactoryMethod();
    }
    internal class CreatorJson : Creator
    {
        public override FactoryMethodMessage FactoryMethod() => new JsonMethods();
    }
    internal class CreatorXml : Creator
    {
        public override FactoryMethodMessage FactoryMethod() => new XmlMethods();
    }
}
