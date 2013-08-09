using System.IO;

namespace DNT.Engine.Core.Serialization
{
    public interface IBinarySerializable<T>
    {
        void Serialize(BinaryWriter writer);
        void Idratate(BinaryReader reader);
    }
}