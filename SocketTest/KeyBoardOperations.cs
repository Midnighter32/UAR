using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketTest
{
    class KeyboardOperations
    {
        [Flags]
        public enum KeyboardEventFlags
        {
            KEYEVENTF_KEYDOWN = 0x0000,
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        public static void KeyboardEvent(Keys key, KeyboardEventFlags value)
        {
            keybd_event((uint)key, 0, (uint)value, 0);
        }
    }
}
