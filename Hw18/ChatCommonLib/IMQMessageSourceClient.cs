using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommonLib
{
    public interface IMQMessageSourceClient
    {
        public void Send(Message message);
        public Message Receive();
    }
}
