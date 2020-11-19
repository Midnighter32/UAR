using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace MouseEvent_Hook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            StartClient();

            MouseHook.Start();
            MouseHook.MouseAction += new EventHandler(Event);

            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += _globalKeyboardHook_KeyboardPressed;
        }

        private GlobalKeyboardHook _globalKeyboardHook;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private int counter = 0;

        private void StartClient()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse("0.0.0.0"), 11666);

            stream = tcpClient.GetStream();
        }

        private void _globalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            byte[] msg = null;

            msg = Encoding.UTF8.GetBytes($"WM_KEYBOARD {e.KeyboardData.VirtualCode} {(int)e.KeyboardState}\n");
            stream.Write(msg, 0, msg.Length);
        }

        private void Event(object sender, EventArgs e) {
            byte[] msg = null;
            MouseHook.HookData hookData = (MouseHook.HookData)sender;
            switch (hookData.MouseMessage)
            {
                case MouseHook.MouseMessages.WM_MOUSEMOVE:
                    Point pos = MouseHook.GetCursorPosition();
                    float x_max = 19.2f;
                    float y_max = 10.8f;
                    float x = (pos.X / x_max) / 100 * Size.Width;
                    float y = (pos.Y / y_max) / 100 * Size.Height;
                    panel1.Location = new Point((int)x, (int)y);
                    if (counter % 20 == 0)
                        msg = Encoding.ASCII.GetBytes($"WM_MOUSEMOVE {pos.X} {pos.Y}\n");
                    counter++;
                    break;
                case MouseHook.MouseMessages.WM_MOUSEWHEEL:
                    msg = Encoding.UTF8.GetBytes($"WM_MOUSEWHEEL {hookData.WheelDelta}\n");
                    break;
                case MouseHook.MouseMessages.WM_LBUTTONDOWN:
                    msg = Encoding.UTF8.GetBytes("WM_LBUTTONDOWN\n");
                    break;
                case MouseHook.MouseMessages.WM_RBUTTONDOWN:
                    msg = Encoding.UTF8.GetBytes("WM_RBUTTONDOWN\n");
                    break;
                case MouseHook.MouseMessages.WM_RBUTTONUP:
                    msg = Encoding.UTF8.GetBytes("WM_RBUTTONUP\n");
                    break;
                case MouseHook.MouseMessages.WM_LBUTTONUP:
                    msg = Encoding.UTF8.GetBytes("WM_LBUTTONUP\n");
                    break;
                default:

                    break;
            }
            if(msg != null)
                stream.Write(msg, 0, msg.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] msg = Encoding.ASCII.GetBytes("<EOF>");
            stream.Write(msg, 0, msg.Length);

            stream.Close();
            tcpClient.Close();
        }
    }
}
