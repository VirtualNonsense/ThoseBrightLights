using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Extensions
{
    public static class GeometryExtensions
    {
        /// <summary>
        /// Turns a vector around Vector2.Zero by a defined angle
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 vector, float rad)
        {
            var x = (float) (Math.Cos(rad) * vector.X - Math.Sin(rad) * vector.Y);
            var y = (float) (Math.Sin(rad) * vector.X + Math.Cos(rad) * vector.Y);
            return new Vector2(x, y);
        }
        
        /// <summary>
        /// Turns a vector around Vector3.Zero (z - axis) by a specified angle
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector3 RotateZ(this Vector3 vector, float rad)
        {
            var x = (float) (Math.Cos(rad) * vector.X - Math.Sin(rad) * vector.Y);
            var y = (float) (Math.Sin(rad) * vector.X + Math.Cos(rad) * vector.Y);
            return new Vector3(x, y, vector.Z);
        }
        
        public static Vector2 RotateAroundPoint(this Vector2 vector, float rad, Vector2 rotationPoint)
        {
            var temp = vector;
            temp -= rotationPoint;
            temp.Rotate(rad);
            temp += rotationPoint;
            return temp;
        }
        
        /// <summary>
        /// Converts Monogame rectangle into a Polygon
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Polygon ToPolygon(this Rectangle rectangle)
        {
            var r = new Polygon(
                new Vector2(rectangle.X, rectangle.Y),
                new Vector2(rectangle.Width/2f, rectangle.Height/2f),
                0,
                new List<Vector2>
                {
                    new Vector2(rectangle.X, rectangle.Y),
                    new Vector2(rectangle.X + rectangle.Width, rectangle.Y),
                    new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
                    new Vector2(rectangle.X, rectangle.Y + rectangle.Height),
                });
            return r;
        }

        /// <summary>
        /// Returns the Minimal Rectangle that encloses a polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static Rectangle GetBoundingRectangle(this Polygon polygon)
        {
            var minX = polygon.Vertices2D.Min(vector2 => vector2.X);
            var minY = polygon.Vertices2D.Min(vector2 => vector2.Y);
            var maxX = polygon.Vertices2D.Max(vector2 => vector2.X);
            var maxY = polygon.Vertices2D.Max(vector2 => vector2.Y);
            return new Rectangle((int)minX, (int)minY, (int) (maxX-minX), (int) (maxY-minY));
        }
        
        /// <summary>
        /// returns the minimal rectangle that fully encloses a group of rectangles
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static Rectangle GetBoundingRectangle(this Polygon[] polygon)
        {
            var t = polygon.Select(t => t.Vertices2D).ToArray();
            var minX = t.Min(vector2 => vector2.Min(v => v.X));
            var minY = t.Min(vector2 => vector2.Min(v => v.Y));
            var maxX = t.Max(vector2 => vector2.Max(v => v.X));
            var maxY = t.Max(vector2 => vector2.Max(v => v.Y));
            return new Rectangle((int)minX, (int)minY, (int) (maxX-minX), (int) (maxY-minY));
        }

        public static Polygon[] MirrorHorizontal(this Polygon[] polygonArray, Vector2 mirrorPoint)
        {
            var newPolygonArray = new Polygon[polygonArray.Length];
            for (var i = 0; i < newPolygonArray.Length; i++)
            {
                newPolygonArray[i] = polygonArray[i].MirrorSingleHorizontal(mirrorPoint);
            }

            return newPolygonArray;
        }
        
        public static Polygon[] MirrorVertical(this Polygon[] polygonArray, Vector2 mirrorPoint)
        {
            var newPolygonArray = new Polygon[polygonArray.Length];
            for (var i = 0; i < newPolygonArray.Length; i++)
            {
                newPolygonArray[i] = polygonArray[i].MirrorSingleVertical(mirrorPoint);
            }

            return newPolygonArray;
        }


        private static Polygon MirrorSingleHorizontal(this Polygon p, Vector2 mirrorPoint)
        {
            var mirroredPolygon = (Polygon) p.Clone();
            var newVertices = mirroredPolygon.Vertices.Select(point => new Vector2(-point.X, point.Y)).ToList();
            mirroredPolygon.Vertices = newVertices;
            mirroredPolygon.Position =
                new Vector2(mirroredPolygon.Position.X - 2 * (mirroredPolygon.Position.X - mirrorPoint.X),
                    mirroredPolygon.Position.Y);
            return mirroredPolygon;
        }
        public static Polygon MirrorSingleVertical(this Polygon p, Vector2 mirrorPoint)
        {
            var mirroredPolygon = (Polygon) p.Clone();
            var newVertices = mirroredPolygon.Vertices.Select(point => new Vector2(point.X, -point.Y)).ToList();
            mirroredPolygon.Vertices = newVertices;
            mirroredPolygon.Position =
                new Vector2(mirroredPolygon.Position.X,
                    mirroredPolygon.Position.Y - 2 * (mirroredPolygon.Position.Y - mirrorPoint.Y));
            return mirroredPolygon;
        }
    }
}