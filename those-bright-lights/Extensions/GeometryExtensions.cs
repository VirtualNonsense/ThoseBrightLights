using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SE_Praktikum.Models;
using System.Linq;
using Rectangle = System.Drawing.Rectangle;

namespace SE_Praktikum.Extensions
{
    public static class GeometryExtensions
    {
        /// <summary>
        /// Converts Monogame rectangle into a Polygon
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Polygon ToPolygon(this Rectangle rectangle)
        {
            Polygon r = new Polygon(
                new Vector2(rectangle.X, rectangle.Y),
                new Vector2(rectangle.Width/2f, rectangle.Height/2),
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
        /// Returns the Minimal Rectangle
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
    }
}