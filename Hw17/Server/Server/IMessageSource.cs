using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IMessageSource
    {
        void Send(Message message, IPEndPoint ep);
        Message Receive(ref IPEndPoint ep);
    }
}
