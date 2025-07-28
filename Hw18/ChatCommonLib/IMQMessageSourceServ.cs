using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommonLib
{
    public interface IMQMessageSourceServ
    {
        void Send(Message message, string clientId);
        Message Receive(ref string? clientId);
    }
}
