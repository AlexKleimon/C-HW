using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server
{
    enum TypeSerialize
    {
        Json,
        Xml
    }
    internal class Message
    {
        public string? FromName { set; get; } // отправитель
        public string? ToName { set; get; } //получатель
        public string TextMessage { set; get; }
        public DateTime TimeMessage { get; set; }
        public Message()
        {
            TextMessage = " ";
        }
        public Message(string? nickName, string textMessage)
        {
            FromName = nickName;
            TextMessage = textMessage;
            TimeMessage = DateTime.Now;
        }
        public Message(string? nickName, string? recipient, string textMessage)
        {
            FromName = nickName;
            ToName = recipient;
            TextMessage = textMessage;
            TimeMessage = DateTime.Now;
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
    }
}
