
namespace Server.Model
{
    public class MessageChat
    {
        public int Id { get; set; }
        public string? TextMessage { get; set; }
        public bool Received { get; set; }
        public int? ToUserId { get; set; }
        public int? FromUserId { get; set; }
        public virtual User? ToUser { get; set; } // получатель
        public virtual User? FromUser { get; set; } // отправитель
    }
}
