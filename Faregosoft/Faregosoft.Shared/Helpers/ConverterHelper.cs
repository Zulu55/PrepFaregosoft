using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Faregosoft.Helpers
{
    public class ConverterHelper
    {
        public static async Task<byte[]> ToByteArray(IRandomAccessStream stream)
        {
            DataReader dataReader = new DataReader(stream.GetInputStreamAt(0));
            byte[] bytes = new byte[stream.Size];
            await dataReader.LoadAsync((uint)stream.Size);
            dataReader.ReadBytes(bytes);
            return bytes;
        }
    }
}
