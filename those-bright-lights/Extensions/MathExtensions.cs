using System;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Extensions
{
    public static class MathExtensions
    {
        public static float Remap(float value, float oldLower, float oldHigher, float newLower, float newHigher)
        {
            if (oldLower > oldHigher) return Remap( value, oldHigher,  oldLower,  newLower,  newHigher);
            if (newLower > newHigher) return Remap( value,  oldLower,  oldHigher,   newHigher, newLower);
            if (value > oldHigher)
                value = oldHigher;
            else if (value < oldLower)
                value = oldLower;
            return (value - oldLower) / (oldHigher - oldLower) * (newHigher - newLower) + newLower;
        }
        public static Vector2 Rotate(this Vector2 vector, float rad)
        {
            var x = (float) (Math.Cos(rad) * vector.X - Math.Sin(rad) * vector.Y);
            var y = (float) (Math.Sin(rad) * vector.X + Math.Cos(rad) * vector.Y);
            return new Vector2(x, y);
        }
        public static Vector3 RotateZ(this Vector3 vector, float rad)
        {
            var x = (float) (Math.Cos(rad) * vector.X - Math.Sin(rad) * vector.Y);
            var y = (float) (Math.Sin(rad) * vector.X + Math.Cos(rad) * vector.Y);
            return new Vector3(x, y, vector.Z);
        }

        public static float DegToRad(float deg)
        {
            return (float) (deg * Math.PI / 180);
        }

        public static float RadToDeg(float rad)
        {
            return (float) (rad * 180 / Math.PI);
        }
    }
}