using System;
using System.IO;
using DNT.Engine.Core.Serialization;

namespace DNT.AsteroidChallenge.App
{
    public struct ScoreHistoryElement : IEquatable<ScoreHistoryElement>, IBinarySerializable<ScoreHistoryElement>
    {
        public String GamerTag;
        public Int32 Score;

        public override Boolean Equals(Object obj)
        {
            return (obj is ScoreHistoryElement) && Equals((ScoreHistoryElement)obj);
        }

        public Boolean Equals(ScoreHistoryElement other)
        {
            return Equals(other.GamerTag, GamerTag) && Equals(other.Score, Score);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = GetType().GetHashCode();
                hash ^= (GamerTag.GetHashCode() * HASH_MULTIPLIER);
                hash ^= (Score * HASH_MULTIPLIER);
                return hash;
            }
        }

        public static Boolean operator ==(ScoreHistoryElement left, ScoreHistoryElement right)
        {
            return Equals(left, right);
        }

        public static Boolean operator !=(ScoreHistoryElement left, ScoreHistoryElement right)
        {
            return !Equals(left, right);
        }

        private const Int32 HASH_MULTIPLIER = 33;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(GamerTag);
            writer.Write(Score);
        }

        public void Idratate(BinaryReader reader)
        {
            GamerTag = reader.ReadString();
            Score = reader.ReadInt32();
        }
    }
}