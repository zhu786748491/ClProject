using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server.Server
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
        public Client ()
        {

        }
        public Client (Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }

        public void Start()
        {
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None,ReceiveCallback,null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                //TODO 处理接收到的数据
                msg.ReadMessage(count);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
            
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        private void Close()
        {
            if(clientSocket !=null)
            {
                clientSocket.Close();
                server.RemoveClient(this);
            }
        }

    }
}
