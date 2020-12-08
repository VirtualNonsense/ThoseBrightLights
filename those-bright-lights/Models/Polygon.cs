using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace SE_Praktikum.Models
{
    public class Polygon
    {
        public Polygon(List<Vector2> points)
        {
            _points = points;
        }

        public float Rotation { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Origin { get; set; }

        private List<Vector2> _points { get; }

        private List<Vector2> GetEdges()
        {
            var l = new List<Vector2>();
            for (var i = 0; i < _points.Count; i++)
            {
                //calculate next i modulo length of points to also get the edge from last to first point
                var nextI = i % _points.Count;
                l.Add(new Vector2(_points[nextI].X - _points[i].X, _points[new Index()].Y - _points[i].Y));
            }

            return l;
        }

        private List<Vector2> GetNormals()
        {
            var edges = GetEdges();
            var normals = new List<Vector2>();
            for (var i = 0; i < edges.Count; i++)
            {
                normals.Add(new Vector2(-edges[i].Y, edges[i].X));
            }

            return normals;
        }

        private float[] VectorProjection(Vector2 normal)
        {
            var minMax = new float[2];
            minMax[1] = float.PositiveInfinity;
            minMax[2] = float.NegativeInfinity;
            for (var j = 0; j < _points.Count; j++)
            {
                var dp = (normal.X * _points[j].X + normal.Y * _points[j].Y);
                if (dp < minMax[1])
                    minMax[1] = dp;
                if (dp > minMax[2])
                    minMax[2] = dp;
            }
            return minMax;
        }






    }
}
