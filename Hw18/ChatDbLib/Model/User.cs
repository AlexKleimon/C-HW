using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDbLib.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<MessageDb> ToMessages { get; set; }

        public virtual ICollection<MessageDb> FromMessages { get; set; }
    }
}
