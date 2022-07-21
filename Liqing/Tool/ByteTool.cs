using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csharp.Liqing.Tool
{
    public class ByteTool
    {
        public static byte[] Int32ToByte(int value)
        {
            byte[] buffer = new byte[4];
            buffer[0] = (byte)(value  & 0xff);
            buffer[1] = (byte)((value >> 8) & 0xff);
            buffer[2] = (byte)((value >> 16) & 0xff);
            buffer[3] = (byte)((value >> 24) & 0xff);
            return buffer;
        }
        public static int BytesToInt32(byte[] data)
        {
            int value = 0;
            value += data[0];
            value += data[1] << 8;
            value += data[2] << 16;
            value += data[3] << 24;
            return value;
        }
    }
}
