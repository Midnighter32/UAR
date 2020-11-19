using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SocketTest
{
    class Program
    {
        static int Main(string[] args)
        {
            StartServer();
            return 0;
        }

        private static void EmulateEvent(string flag, List<string> Args)
        {
            switch (flag)
            {
                case "WM_MOUSEMOVE":
                    Point pos = new Point(
                        int.Parse(Args[0]),
                        int.Parse(Args[1])
                        );
                    Console.WriteLine(pos.ToString());
                    Cursor.Position = pos;
                    break;
                case "WM_MOUSEWHEEL":
                    int delta = int.Parse(Args[0]);
                    MouseOperations.MouseScrollEvent(delta);
                    break;
                case "WM_LBUTTONDOWN":
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                    break;
                case "WM_LBUTTONUP":
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                    break;
                case "WM_RBUTTONDOWN":
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightDown);
                    break;
                case "WM_RBUTTONUP":
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightUp);
                    break;
                case "WM_KEYBOARD":
                    KeyboardOperations.KeyboardEventFlags value = int.Parse(Args[1]) == 0x100 ? 
                        KeyboardOperations.KeyboardEventFlags.KEYEVENTF_KEYDOWN : 
                        KeyboardOperations.KeyboardEventFlags.KEYEVENTF_KEYUP;

                    KeyboardOperations.KeyboardEvent((Keys)int.Parse(Args[0]), value);
                    break;
                default:
                    break;
            }
        }

        public static void StartServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("0.0.0.0"), 11666);

            listener.Start();

            Console.WriteLine("Waiting for a connection...");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Connected");

            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[1024];
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    var str = Encoding.ASCII.GetString(data, 0, bytes);
                    string[] arr = str.Split('\n');
                    foreach (string item in arr)
                    {
                        if(item != "")
                        {
                            Console.WriteLine("Text received : {0}", item);
                            string[] args = item.Split(' ');
                            List<string> arg = new List<string>();
                            for(int i = 1; i < args.Length; i++)
                                arg.Add(args[i]);
                            EmulateEvent(args[0], arg);
                        }
                            
                    }
                }
                while (stream.DataAvailable);
            }
        }
    }
}
