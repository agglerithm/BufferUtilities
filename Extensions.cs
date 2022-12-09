using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace BufferUtilities
{
    public static class Extensions
    {
        private static Dictionary<Type, int> _typeSizes = new Dictionary<Type, int>()
        {
            { typeof(byte), 1 },
            { typeof(System.Byte), 1 },
            { typeof(bool), 1 },
            { typeof(Boolean), 1 },
            { typeof(sbyte), 1 },
            { typeof(SByte), 1 },
            { typeof(short), 2 },
            { typeof(Int16), 2 },
            { typeof(ushort), 2 },
            { typeof(UInt16), 2 },
            { typeof(char), 2 },
            { typeof(Char), 2 },
            { typeof(int), 4 },
            { typeof(Int32), 4 },
            { typeof(uint), 4 },
            { typeof(UInt32), 4 },
            { typeof(float), 4 },
            { typeof(Single), 4 },
            { typeof(double), 8 },
            { typeof(Double), 8 },
            { typeof(long), 8 },
            { typeof(Int64), 8 },
            { typeof(ulong), 8 },
            { typeof(UInt64), 8 },

        };

        public static byte[] StringToByteArray(this string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        public static string ByteArrayToString(this byte[] buffer)
        {
            return System.Text.Encoding.ASCII.GetString(buffer);
        }

        public static int ByteArrayToInt(this byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 0);
        }
        public static uint ByteArrayToUint(this byte[] buffer)
        {
            return BitConverter.ToUInt32(buffer, 0);
        }
        public static byte[] UshortToBytes(this ushort num)
        {
            return BitConverter.GetBytes(num);
        }

        public static ushort ByteArrayToUshort(this byte[] buffer)
        {
            return BitConverter.ToUInt16(buffer, 0);
        }
        public static byte[] Trim(this byte[] buff, int len)
        {
            var retVal = new byte[len];
            if (buff.Length == len)
                return buff;
            for (int i = 0; i < len; i++)
            {
                retVal[i] = buff[i];
            }

            return retVal;
        }

        public static byte[] ToggleEndian(this byte[] rep)
        {
            return rep.Reverse().ToArray();
        }

        public static int FromBcd(this byte byteData)
        {
            int result = 10 * (byteData >> 4);
            return result + byteData & 0xf;
        }

        public static byte ToBcd(this int num)
        {
            int bcd = 0;
            for (int digit = 0; digit < 2; ++digit)
            {
                int nibble = num % 10;
                bcd |= nibble << (digit * 4);
                num /= 10;
            }

            return (byte)(bcd);

        }

        public static byte[] ToByteArray(this char[] carray)
        {
            return carray.Select(c => (byte)c).ToArray();
        }

        public static byte[] Slice(this byte[] buff, int startNdx, int endNdx = 0)
        {
            if (endNdx == 0)
                endNdx = buff.Length - 1;
            if (startNdx >= endNdx)
                throw new Exception("Start index must be smaller than end index");
            if (endNdx > buff.Length)
                throw new Exception($"The buffer is not long enough to go from {startNdx} to {endNdx}");
            var retBuff = new byte[buff.Length - startNdx];
            var pos = 0;
            for (var i = startNdx; i <= endNdx; i++)
            {
                retBuff[pos] = buff[i];
                pos++;
            }

            return retBuff;
        }

       
    }
}