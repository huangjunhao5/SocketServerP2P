// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace NetStream
{
    class pair
    {
        public Socket client;
        public int pos;
        public static pair make_pair(Socket c,int p)
        {
            pair pi = new pair();
            pi.pos = p;
            pi.client = c;
            return pi;
        }
    }
    class SocketServer
    {
        private Socket serverSocket;
        private int port = 12345;
        private byte[] result = new byte[1024];
        private IPAddress ip;
        public static int ConnectNum = 0;
        private static Socket[] clientLink = new Socket[10];
        public int SendPos = 0;
        private bool HasCommandToClient = false;
        private String SendToClientCommand;
        public void SendCommandToAllClient(String cmd)
        {
            for(int i = 1;i <= ConnectNum;i++)
            {
                clientLink[i].Send(Encoding.ASCII.GetBytes(cmd));
            }
        }
        public String getResult()
        {
            return Encoding.ASCII.GetString(result);    
        }
        public void setIPAddress(String IP)
        {
            ip = IPAddress.Parse(IP);
        }
        public void run()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, port));
            serverSocket.Listen(10);                        //设定最多10个排队链接请求
            //
            Thread clientConnectListen = new Thread(ListenClientConnect);
            clientConnectListen.Start();
        }
        private void ListenClientConnect()
        {
            while (true)
            {
                int num = ConnectNum + 1;
                clientLink[ConnectNum + 1] = serverSocket.Accept();
                if(num != ConnectNum + 1)
                {
                    clientLink[ConnectNum + 1] = clientLink[num];
                    clientLink[num] = null;
                }
                ConnectNum++;
                clientLink[ConnectNum].Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                Thread clientLinkThread = new Thread(Link);
                clientLinkThread.Start(pair.make_pair(clientLink[ConnectNum], ConnectNum));
                ShowClientState();
            }
        }
        public void SendToOtherClient(String cmd,int pos)
        {
            for(int i = 1;i <= ConnectNum; i++)
            {
                if (i == pos) continue;
                clientLink[i].Send(Encoding.ASCII.GetBytes(cmd));
            }
        }
        private void ShowClientState()
        {
            Console.WriteLine(ConnectNum+" Clients Linked");
        }
        private void Link(object client)
        {
            pair p = (pair)client;
            Socket clientSocket = p.client;
            int pos = p.pos;
            while (true)
            {
                try
                {
                    int returnNum = clientSocket.Receive(result);
                    Console.WriteLine(pos + " : " + Encoding.ASCII.GetString(result));
                    SendPos = pos;
                }
                catch
                {
                    Console.WriteLine("ConnctError");
                    //clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    for(int i = pos;i < ConnectNum; i++)
                    {
                        clientLink[i] = clientLink[i + 1];
                    }
                    clientLink[ConnectNum] = null;
                    ConnectNum--;
                    Console.WriteLine(pos + " : " + "DisConnect");
                    ShowClientState();
                    break;
                }
            }
        }
    }
}