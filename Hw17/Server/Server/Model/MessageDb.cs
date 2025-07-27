
namespace Server.Model
{
    public class MessageDb
    {
        public int Id { get; set; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public string Text { get; set; } = null!;
        public virtual UserDb? FromUser { get; set; }
        public virtual UserDb? ToUser { get; set; }
        public bool Received { get; set; }
    }
}
