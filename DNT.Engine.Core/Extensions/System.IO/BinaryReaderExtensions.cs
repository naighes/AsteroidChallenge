using System.Collections.Generic;
using DNT.Engine.Core.Serialization;

namespace System.IO
{
    public static class BinaryReaderExtensions
    {
        public static IList<T> ReadList<T>(this BinaryReader reader) where T : IBinarySerializable<T>
        {
            var list = new List<T>();

            var size = 0;

            try { size = reader.ReadInt32(); }
            catch (EndOfStreamException) { }

            for (var i = 0; i < size; i++)
            {
                var item = Activator.CreateInstance<T>();
                item.Idratate(reader);
                list.Add(item);
            }

            return list;
        }
    }
}