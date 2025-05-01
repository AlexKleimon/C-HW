using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hw14
{
    class Message
    {
        public string? NamePerson { set; get; }
        public string? TextMessage { set; get; }
        public DateTime TimeMessage { get; set; }
        public Message()
        {

        }
        public Message(string? nickname, string? textMessage)
        {
            NamePerson = nickname;
            TextMessage = textMessage;
            TimeMessage = DateTime.Now;
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message? GetMessage(String message)
        {
            return JsonSerializer.Deserialize<Message>(message);
        }
        public override string ToString()
        {
            return $"{NamePerson}: {TextMessage} ({TimeMessage.ToShortTimeString()})";
        }
    }
}
