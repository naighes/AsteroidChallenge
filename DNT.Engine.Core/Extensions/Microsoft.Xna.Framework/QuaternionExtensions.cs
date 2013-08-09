using System;

namespace Microsoft.Xna.Framework
{
    public static class QuaternionExtensions
    {
        public static Quaternion Yaw(this Quaternion quaternion, Single value)
        {
            return quaternion * Quaternion.CreateFromYawPitchRoll(value, 0.0f, 0.0f);
        }

        public static void Yaw(this Quaternion quaternion, Single value, out Quaternion result)
        {
            result = quaternion * Quaternion.CreateFromYawPitchRoll(value, 0.0f, 0.0f);
        }

        public static Quaternion Pitch(this Quaternion quaternion, Single value)
        {
            return quaternion * Quaternion.CreateFromYawPitchRoll(0.0f, value, 0.0f);
        }

        public static void Pitch(this Quaternion quaternion, Single value, out Quaternion result)
        {
            result = quaternion * Quaternion.CreateFromYawPitchRoll(0.0f, value, 0.0f);
        }

        public static Quaternion Roll(this Quaternion quaternion, Single value)
        {
            return quaternion * Quaternion.CreateFromYawPitchRoll(0.0f, 0.0f, value);
        }

        public static void Roll(this Quaternion quaternion, Single value, out Quaternion result)
        {
            result = quaternion * Quaternion.CreateFromYawPitchRoll(0.0f, 0.0f, value);
        }
    }
}