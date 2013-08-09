using System.Collections.Generic;
using System.Linq;
using DNT.Engine.Core.Serialization;

namespace System.IO
{
    public static class BinaryWriterExtensions
    {
        public static void Write<T>(this BinaryWriter writer, IEnumerable<T> list) where T : IBinarySerializable<T>
        {
            writer.Write(list.Count());

            foreach (var item in list)
                item.Serialize(writer);
        }
    }
}