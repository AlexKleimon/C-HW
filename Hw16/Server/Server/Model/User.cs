using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Port { get; set; }
        public string? IpAdress { get; set; }
        public virtual ICollection<MessageChat> ToMessages { get; set; } = new List<MessageChat>(); // полученные сообщения
        public virtual ICollection<MessageChat> FromMessages { get; set; } = new List<MessageChat>(); // отправленные сообщения
    }
}
