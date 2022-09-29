// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Net;
using System.Threading;
using System.Net.Sockets;
using NetStream; // 包含NetStream

namespace SampleMain
{
    public class MainClass
    {
        public static void Main(String[] args)
        {
            RunSocketServer.SetIP("127.0.0.1"); // ip服务器（本机）ip地址
            RunSocketServer.SetRunCommand(runCommand); // 设置运行指令函数
            RunSocketServer.run(); // 设置完成后跑起来
            {
                // 向所有客户机传递指令
                // RunSocketServer.SendCommandToClient("Command");
                // 可以新建一个线程来监听主机是否实行了某种操作，发现执行了操作时将当前执行完的指令传入客户机中
            }
        }
        public static int runCommand(String cmd)
        {
            // 在这里运行代码，并且在RunSocketServer中传入这个参数
            return 0;
        }
    }
}