using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BufferUtilities
{
    public class BufferBuilder:IDisposable
    {
        private readonly MemoryStream _memStream;

        public BufferBuilder()
        {
            _memStream = new MemoryStream();
        }

        private BufferBuilder(BufferBuilder from)
        {
            this._memStream = from._memStream;
        }

        public BufferBuilder(byte[] data)
        {
            this.Append(data);
        }

        public  byte[] ToByteArray()
        {
            if (!_memStream.CanRead) return new byte[] { };
            _memStream.Position = 0;
            var buff = new byte[Length];
            for (var i = 0; i < Length; i++)
                buff[i] = (byte) _memStream.ReadByte();
            return buff;
        }
        public long Length=>  _memStream.Length;
        
    
        public BufferBuilder AppendAsBcd(int value)
        {
            var unit = value.ToBcd();
            Append(unit);
            return this;
        }

        public BufferBuilder Append(byte value)
        {
            _memStream.WriteByte(value);
            return this;
        }
        public BufferBuilder Append(ushort value, int byteCount = 2)
        { ;
            Append(GetAppendBytes(value, byteCount), 0, byteCount);
            return this;
        }
        public  BufferBuilder Append(uint value, int byteCount = 4)
        {
            Append(GetAppendBytes(value, byteCount), 0, byteCount);
            return this;
        }
        public  BufferBuilder Append(ulong value, int byteCount = 8)
        {
            Append(GetAppendBytes(value, byteCount), 0, byteCount);
            return this;
        }
        public  BufferBuilder AppendAsBigEndian(ushort value, int byteCount = 2)
        {
            Append(GetAppendBytes(value, byteCount).ToggleEndian(), 0, byteCount);
            return this;
        }
        
        public BufferBuilder AppendAsBigEndian(uint value, int byteCount = 4)
        {
            Append(GetAppendBytes(value, byteCount).ToggleEndian(), 0, byteCount);
            return this;
        }

        public BufferBuilder AppendAsBigEndian(ulong value, int byteCount = 8)
        {
            Append(GetAppendBytes(value, byteCount).ToggleEndian(), 0, byteCount);
            return this;
        }

        private byte[] GetAppendBytes(ushort num, int byteCount)
        {
            ValidateSize(num, byteCount);
            var bytes = BitConverter.GetBytes(num);
            return bytes.Trim(byteCount);
        }

        private void ValidateSize(ulong num, int byteCount)
        {
            var throwError = false;
            switch (byteCount)
            {
                case 1:
                    throwError = num > byte.MaxValue;
                    break;
                case 2:
                    throwError = num > ushort.MaxValue;
                    break;
                case 3:
                    throwError = num > 0xFFFFFF;
                    break;
                case 4:
                    throwError = num > ulong.MaxValue/16;
                    break;
                case 5:
                    throwError = num > ulong.MaxValue/8;
                    break;
                case 6:
                    throwError = num > ulong.MaxValue/4;
                    break;
                case 7:
                    throwError = num > ulong.MaxValue/2;
                    break;
                case 8:
                    throwError = num > ulong.MaxValue;
                    break;
                default:
                    throwError = true;
                    break;
            }
            if (throwError)
                throw new IndexOutOfRangeException("Length of array is too small for the type.");

        }

        private byte[] GetAppendBytes(ulong num, int byteCount)
        {
            
            ValidateSize(num, byteCount);
            var bytes = BitConverter.GetBytes(num);
            return bytes.Trim(byteCount);
        }

        private byte[] GetAppendBytes(uint num, int byteCount)
        {
            
            ValidateSize(num, byteCount);
            var bytes = BitConverter.GetBytes(num);
            return bytes.Trim(byteCount);
        }

        public BufferBuilder Append(byte[] value, int startIndex = 0, int byteCount = 0)
        {
            if (value == null)
            {
                if (startIndex == 0)
                    return this;
                throw new ArgumentNullException(nameof (value));
            }
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof (startIndex), "Start index is out of range");
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException("count",  "Byte count is less than zero");
            if (byteCount == 0 && value.Length == 0)
                return this;
            if (byteCount == 0 && value.Length > 0)
                byteCount = value.Length;

            if (byteCount > value.Length - startIndex)
                throw new ArgumentOutOfRangeException("count", "Byte count would put array out of range");
            _memStream.Write(value, startIndex, byteCount);
            return this;
        }

        public void AppendDate(DateTime dte, bool milliseconds = false, bool reverse = false)
        {
            var dateNums = new List<int>
            {
                dte.Year % 100,
                dte.Month,
                dte.Day,
                dte.Hour,
                dte.Minute,
                dte.Second
            };
            if (milliseconds)
                dateNums.Add(dte.Millisecond);
            var arr = dateNums.ToArray();
            if (reverse)
                arr = arr.Reverse().ToArray();
            foreach (var num in arr)
            {
                AppendAsBcd(num);
            }
        }

        public void AppendZeroes(int num)
        {
            for (int i = 0; i < num; i++)
                Append((byte) 0);
        }
        public void Append(string str)
        {
            Append(str.StringToByteArray());
        }

        public void Clear()
        {
            _memStream.Position = 0;
            _memStream.SetLength(0);
        }

        public void Dispose()
        {
            _memStream?.Dispose();
        }

        public int ReadFrom(Stream stream, int position, int len)
        {
            var buff = new byte[len];
            var bytesRead = stream.Read(buff, position, len);
            Append(buff, 0, bytesRead);
            return bytesRead;
        }

        public async Task<int> ReadFromAsync(Stream stream, int position, int len)
        {           
            var buff = new byte[len];
            var bytesRead = await stream.ReadAsync(buff, position, len);
            Append(buff, 0, bytesRead);
            return bytesRead;
        }
    }
}