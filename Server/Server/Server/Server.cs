using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Server.Controller;

namespace Server.Server
{
    class Server
    {
        private IPEndPoint ipEndPoint;

        private Socket serverSocket;

        private List<Client> clientList;

        private ControllerManager controllerManager;

        public Server ()
        {

        }

        public Server(string ipStr, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
        }

        public void SetIpAndPort (string ipStr,int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
        }
        /// <summary>
        /// 连接的回调
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket,this);
            client.Start();
            clientList.Add(client);
        }

        public void RemoveClient(Client client)
        {
            //加锁，防止错误
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }
    }
}
