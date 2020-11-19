using System;
using System.Runtime.InteropServices;

namespace SocketTest
{
    class MouseOperations
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown    = 0x0002,
            LeftUp      = 0x0004,
            MiddleDown  = 0x0020,
            MiddleUp    = 0x0040,
            Move        = 0x0001,
            Absolute    = 0x8000,
            RightDown   = 0x0008,
            RightUp     = 0x0010,
            WheelScroll = 0x0800,
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SetCursorPosition(MousePoint point)
        {
            SetCursorPos(point.X, point.Y);
        }

        public static MousePoint GetCursorPosition()
        {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
            return currentMousePoint;
        }

        public static void MouseScrollEvent(int delta)
        {
            MousePoint position = GetCursorPosition();

            mouse_event
                ((int)MouseEventFlags.WheelScroll,
                 position.X,
                 position.Y,
                 delta,
                 0)
                ;
        }

        public static void MouseEvent(MouseEventFlags value)
        {
            MousePoint position = GetCursorPosition();

            mouse_event
                ((int)value,
                 position.X,
                 position.Y,
                 0,
                 0)
                ;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
