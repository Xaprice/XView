using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview.utils
{
    /// <summary>
    /// Window消息解析助手类
    /// </summary>
    public class WMMSGHelper
    {
        public static ushort LOWORD(uint value)
        {
            return (ushort)(value & 0xFFFF);
        }
        public static ushort HIWORD(uint value)
        {
            return (ushort)(value >> 16);
        }
        public static short HIWORD(int value)
        {
            return (short)(value >> 16);
        }

        public static byte LOWBYTE(ushort value)
        {
            return (byte)(value & 0xFF);
        }
        public static byte HIGHBYTE(ushort value)
        {
            return (byte)(value >> 8);
        }
    }
}
