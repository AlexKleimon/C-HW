using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatCommonLib;
using NetMQ.Sockets;
using NetMQ;

namespace ChatNetworkLib
{
    public class MQMessageSourceClient : IMQMessageSourceClient
    {
        private readonly DealerSocket ds;
        public MQMessageSourceClient(int portClient, string ip, int portServ)
        {
            ds = new DealerSocket();
            ds.Bind($"tcp://{ip}:{portClient}");
            ds.Options.Identity = Encoding.UTF8.GetBytes(portClient.ToString());
            ds.Connect($"tcp://{ip}:{portServ}");
        }

        public Message Receive()
        {
            string mqMes = ds.ReceiveFrameString();
            return Message.FromJson(mqMes ?? new Message().ToJson());
        }

        public void Send(Message message)
        {
            string jsonMessage = message.ToJson();
            byte[] sendBytes = Encoding.UTF8.GetBytes(jsonMessage);
            ds.SendFrame(sendBytes, sendBytes.Length);
        }
    }
}

