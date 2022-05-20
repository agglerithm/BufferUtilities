using System;
using System.Linq;
using System.Text;

namespace BufferUtilities
{
    public static class Extensions
    {

        public static byte[] StringToByteArray(this string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }
        public static string ByteArrayToString(this byte[] buffer)
        {
            return System.Text.Encoding.ASCII.GetString(buffer);
        }

        public static byte[] UshortToBytes(this ushort num)
        {
            return BitConverter.GetBytes(num);
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

        public static byte[] ToggleEndian(this byte [] rep)
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
            for (int digit = 0; digit < 2; ++digit) {
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
                endNdx = buff.Length -1;
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