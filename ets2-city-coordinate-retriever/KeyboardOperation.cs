using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ets2_city_coordinate_retriever
{
    class KeyboardOperation
    {
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void KeyDown(Keys key)
        {
            keybd_event((byte)key, 0, 0, 0);
        }

        public static void KeyUp(Keys key)
        {
            keybd_event((byte)key, 0, 2, 0);
        }

        public static void KeyPress(Keys key)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            KeyDown(key);
            Thread.Sleep(TimeSpan.FromMilliseconds(2));
            KeyUp(key);
            Thread.Sleep(TimeSpan.FromMilliseconds(2));
        }
    }
}
