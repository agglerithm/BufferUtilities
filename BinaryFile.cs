using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BufferUtilities
{
    public abstract class BinaryFile
    {
        protected BufferBuilder _data = new BufferBuilder();

        public BufferBuilder Data => _data;

        public void Read(string path)
        {
            ReadData(() => File.OpenRead(path)); 
        }

        protected abstract void ReadData(Func<FileStream> streamFactory);


        public void Write(string path)
        {
            using (var stream = File.OpenWrite(path))
            {
                var dataBuffer = _data.ToByteArray();
                stream.Write(dataBuffer, 0, dataBuffer.Length);
                stream.Flush();
            }
        }

    }
}
