using System;

namespace SE_Praktikum.Extensions
{
    public static class MathExtensions
    {
        public static float Remap(float value, float oldLower, float oldHigher, float newLower, float newHigher)
        {
            if (oldLower > oldHigher) return Remap( value, oldHigher,  oldLower,  newLower,  newHigher);
            if (newLower > newHigher) return Remap( value,  oldLower,  oldHigher,   newHigher, newLower);
            return (value - oldLower) / (oldHigher - oldLower) * (newHigher - newLower) + newLower;
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