using System;
using System.Net.Sockets;
using System.Text;

namespace SocketTest
{
    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[1024];
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    Console.WriteLine(data.Length + "and" + bytes);
                    var str = Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Text received : {0}", str);
                }
                while (stream.DataAvailable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
                GC.Collect();
                GC.WaitForFullGCComplete();
            }
        }
    }
}
