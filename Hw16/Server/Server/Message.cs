
namespace Server
{
    public enum Command
    {
        Register,
        Confirmation,
        Delete,
        List,
        CheckedOnline,
        Exit
    }
    public enum TypeSerialize
    {
        Json,
        Xml
    }
    public class Message : ICloneable
    {
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string? FromName { set; get; } // отправитель
        public string? ToName { set; get; } //получатель
        public string? TextMessage { set; get; }
        public DateTime TimeMessage { get; set; }
        public Message()
        {
            TextMessage = " ";
        }
        public Message(string? nickName, string? textMessage)
        {
            FromName = nickName;
            TextMessage = textMessage;
            TimeMessage = DateTime.Now;
        }
        public Message(string? nickName, string? recipient, string? textMessage, DateTime timeMessage, int? id)
        {
            FromName = nickName;
            ToName = recipient;
            TextMessage = textMessage;
            TimeMessage = timeMessage;
            Id = id;
        }
        public string SetMessage(TypeSerialize typeSerialize)
        {
            FactoryMethodMessage mes;
            if (typeSerialize == TypeSerialize.Json)
            {
                Creator creatJson = new CreatorJson();
                mes = creatJson.FactoryMethod();
            }
            else
            {
                Creator creatXml = new CreatorXml();
                mes = creatXml.FactoryMethod();
            }
            return mes.SetMessage(this);
        }

        public static Message? GetMessage(String message, TypeSerialize typeSerialize)
        {
            FactoryMethodMessage mes;
            if (typeSerialize == TypeSerialize.Json)
            {
                Creator creatJson = new CreatorJson();
                mes = creatJson.FactoryMethod();
            }
            else
            {
                Creator creatXml = new CreatorXml();
                mes = creatXml.FactoryMethod();
            }
            return mes.GetMessage(message);
        }
        public override string ToString()
        {
            return $"{FromName}: {TextMessage} ({TimeMessage.ToShortTimeString()})";
        }

        public object Clone()
        {
            return new Message(FromName, ToName, TextMessage, TimeMessage, Id);
        }
    }
}
