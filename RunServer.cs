// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace NetStream
{
    public class RunSocketServer
    {
        private static String GetCommand;
        private static SocketServer server;
        private static Func<String,int> runCommand;
        public static String ip = "127.0.0.1";
        public static void SendCommandToClient(String cmd)
        {
            server.SendCommandToAllClient(cmd);
        }
        public static void SetRunCommand(Func<String, int> fuc)
        {
            runCommand = fuc;
        }
        public static void SetIP(String IP)
        {
            ip = IP;
        }
        public static void run()
        {
            server = new SocketServer();
            server.setIPAddress(ip);
            server.run();
            Thread Listen = new Thread(ListenServer);
            Listen.Start(server);
        }
        public static void ListenServer(object server)
        {
            SocketServer socketServer = new SocketServer();
            while (true)
            {
                if (socketServer.SendPos != 0)
                {
                    GetCommand = socketServer.getResult();
                    socketServer.SendToOtherClient(GetCommand,socketServer.SendPos);
                    socketServer.SendPos = 0;
                    int returnNum = runCommand(GetCommand);
                    Console.WriteLine("RunCommand result's returnNum is:" + returnNum);
                }
            }
        }
    }
}