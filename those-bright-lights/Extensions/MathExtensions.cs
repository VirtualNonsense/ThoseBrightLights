using System;
using System.Reactive.Concurrency;
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

        public static float DegToRad(float deg)
        {
            return (float) (deg * Math.PI / 180);
        }

        public static float RadToDeg(float rad)
        {
            return (float) (rad * 180 / Math.PI);
        }

        public static float RotationToPlayer(Vector2 vector, bool flipped)
        {
            var (x, y) = vector;
            var rotation = (float)Math.Asin(y / vector.Length());
            if (x < 0)
                rotation = MathExtensions.Modulo2Pi( (float) Math.PI -  rotation); 
            // else
            //     rotation *= -1;
            return rotation;
        }

        public static float Modulo2Pi(float value)
        {
            var newValue = value;
            while (Math.Abs(value) > 2 * Math.PI)
            {
                if (value > 0)
                    newValue -= 2 * (float) Math.PI;
                else
                    newValue += 2 * (float) Math.PI;
            }

            return newValue;
        }
    }
}