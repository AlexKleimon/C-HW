using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatCommonLib;
using NetMQ.Sockets;
using NetMQ;

namespace ChatNetworkLib
{
    public class MQMessageSourceServ : IMQMessageSourceServ
    {
        private readonly RouterSocket rs = new();
        public MQMessageSourceServ()
        {
            rs.Bind($"tcp://*:{12345}");
        }
        public Message Receive(ref string? clientId)
        {
            NetMQMessage mqMes = rs.ReceiveMultipartMessage();
            clientId = mqMes.First().ConvertToString();
            string? msg = mqMes.Last().ConvertToString();
            return Message.FromJson(msg ?? new Message().ToJson());

        }

        public void Send(Message message, string clientId)
        {
            NetMQMessage mqMes = new NetMQMessage();
            mqMes.Append(clientId);
            mqMes.Append(Encoding.UTF8.GetBytes(message.ToJson()));
            rs.SendMultipartMessage(mqMes);
        }
    }
}
