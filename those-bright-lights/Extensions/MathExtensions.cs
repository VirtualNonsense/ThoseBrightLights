using System;
using System.Reactive.Concurrency;
using Microsoft.Xna.Framework;
using NLog;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;

namespace SE_Praktikum.Extensions
{
    public static class MathExtensions
    {
        
        /// <summary>
        /// remaps a given value from an old interval to a new one
        /// <example>50, 0 100 -> 25, 0 50</example>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldLower"></param>
        /// <param name="oldHigher"></param>
        /// <param name="newLower"></param>
        /// <param name="newHigher"></param>
        /// <returns></returns>
        public static float Remap(float value, float oldLower, float oldHigher, float newLower, float newHigher)
        {
            // correct false usage
            if (oldLower > oldHigher) return Remap( value, oldHigher,  oldLower,  newLower,  newHigher);
            if (newLower > newHigher) return Remap( value,  oldLower,  oldHigher,   newHigher, newLower);
            if (value > oldHigher)
                value = oldHigher;
            else if (value < oldLower)
                value = oldLower;
            return (value - oldLower) / (oldHigher - oldLower) * (newHigher - newLower) + newLower;
        }

        /// <summary>
        /// Converts degree to rad
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static float DegToRad(float deg)
        {
            return (float) (deg * Math.PI / 180);
        }
        
        /// <summary>
        /// Converts rad to rad degree
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static float RadToDeg(float rad)
        {
            return (float) (rad * 180 / Math.PI);
        }
        
        public static float RotationToTarget(Actor target, Actor self)
        {
            var vector = target.Position - self.Position;
            return GetVectorRotation(vector);
        }
        
        public static float RotationToVector(Vector2 other, Vector2 self)
        {
            return GetVectorRotation(other - self);
        }

        public static float GetVectorRotation(Vector2 vector2)
        {
            var oneZeroVector = Vector2.UnitX;
            var numerator = Vector2.Dot(vector2, oneZeroVector);
            var denominator = vector2.Length() * oneZeroVector.Length();
            var rotation = (float) Math.Acos(numerator / denominator);
            if (vector2.Y < 0)
                rotation = Modulo2PiPositive(2 * (float) Math.PI - rotation);
            return rotation;
        }

        private static float Modulo2PiPositive(float value)
        {
            var newValue = value;
            while (newValue >= 2 * Math.PI || newValue < 0)
            {
                if (newValue > 0)
                    newValue -= 2 * (float) Math.PI;
                else
                    newValue += 2 * (float) Math.PI;
            }

            return newValue;
        }
        
        public static float Modulo2PiAlsoNegative(float value)
        {
            var newValue = value;
            while (Math.Abs(newValue) > 2*Math.PI)
            {
                if (newValue > 0)
                    newValue -= (float) (2*Math.PI);
                else
                    newValue += (float) (2*Math.PI);
            }

            return newValue;
        }
    }
}