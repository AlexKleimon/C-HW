

namespace Server.Model
{
    public class UserDb
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<MessageDb> ToMessages { get; set; } = new
        List<MessageDb>();
        public virtual ICollection<MessageDb> FromMessages { get; set; } = new
        List<MessageDb>();
    }
}
