using Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    class MainServerProgram
    {
        private const int port = 8888;
        private static TcpListener tcpListener;
        private static ServerHelper serverHelper = new ServerHelper();
        static readonly object _lock = new object();
        static void Main()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8888);
            tcpListener.Start();
            Console.WriteLine("Server started and waiting...");

            while (true)
            {
                try
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    var strm = tcpClient.GetStream();

                    //Recieve id client who connecting and check
                    var str = GetCommandFromStream(strm);
                    int id = int.Parse(str);
                    var connected = CheckIsConnected(id);

                    if (!connected)
                    {
                        lock (_lock) serverHelper.AddConnection(id, tcpClient);
                    }
                    Console.WriteLine($"Client [{id}] - connected.");

                    Thread newClientThread = new Thread(ClientHandler);
                    newClientThread.Start(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Disconnect();
                    break;
                }
            }
        }
        private static void ClientHandler(object o)
        {
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = serverHelper.GetConnectedClientsByKey(id);
            while (true)
            {
                NetworkStream stream = client.GetStream();

                string stringCommand = GetCommandFromStream(stream);

                Command command;
                bool isParsed = Enum.TryParse<Command>(stringCommand, out command);
                if (isParsed)
                {
                    if (command == Command.MessageSend)
                    {
                        TransitionObject packge = (TransitionObject)GetObjectFromStream(stream);
                        WriteToScreen(packge, id);
                        SendPackage(packge);
                    }
                }
            }
        }
        private static void WriteToScreen(TransitionObject packge, int id)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Clinet [{id}]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{packge.Message}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" to Client [{packge.ToClientId}]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
        private static bool CheckIsConnected(int id)
        {
            foreach (var client in ServerHelper.AllConnectedClients)
            {
                if (client.Key == id)
                {
                    return true;
                }
            }
            return false;
        }
        private static object GetObjectFromStream(NetworkStream _stream)
        {
            byte[] data = new byte[2048];
            int bytes = 0;
            do
            {
                bytes = _stream.Read(data, 0, data.Length);
            }
            while (_stream.DataAvailable);
            return serverHelper.ByteArrayToObject(data);
        }
        private static void SendObjectToStream(NetworkStream _stream, TransitionObject o)
        {
            byte[] data = serverHelper.ObjectToByteArray(o);
            _stream.Write(data, 0, data.Length);
        }

        private static string GetCommandFromStream(NetworkStream _stream)
        {
            byte[] data = new byte[1024];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = _stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (_stream.DataAvailable);

            return builder.ToString();
        }
        private static void SendPackage(TransitionObject package)
        {
            foreach (var c in ServerHelper.AllConnectedClients)
            {
                if (c.Key == package.ToClientId || c.Key == package.FromClientId)
                {
                    var _stream = c.Value.GetStream();
                    SendObjectToStream(_stream, package);
                }
            }
        }
        private static void Disconnect()
        {
            tcpListener.Stop();
            //Environment.Exit(0);
        }
    }
}
