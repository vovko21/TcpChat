using Server.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server.Network
{
    public class ServerHelper
    {
        private static Dictionary<int, TcpClient> allConnectedClients = new Dictionary<int, TcpClient>();
        public static Dictionary<int, TcpClient> AllConnectedClients => allConnectedClients;

        public void AddConnection(int key, TcpClient value)
        {
            allConnectedClients.Add(key, value);
        }
        public virtual void RemoveConnection(int key)
        {
            allConnectedClients.Remove(key);
        }
        public virtual TcpClient GetConnectedClientsByKey(int key)
        {
            return allConnectedClients[key];
        }
        public Client RecieveConnectedClient(NetworkStream stream)
        {
            byte[] data = new byte[2048];
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
            }
            while (stream.DataAvailable);
            return (Client)ByteArrayToObject(data);
        }
        public void SendConnectedClinet(Client client, NetworkStream stream)
        {
            byte[] data = ObjectToByteArray(client);
            stream.Write(data, 0, data.Length);
        }
        public virtual void SendCommand(Command command, NetworkStream stream)
        {
            string _command = command.ToString();
            byte[] data = Encoding.Unicode.GetBytes(_command);
            stream.Write(data, 0, data.Length);
        }
        public virtual byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public virtual object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
