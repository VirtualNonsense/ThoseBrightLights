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
                var nextI = i+1 % _points.Count;
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
            //initialize minMax
            minMax[1] = float.PositiveInfinity;
            minMax[2] = float.NegativeInfinity;
            //dp == dotprodukt (Skalarprodukt)
            for (var j = 0; j < _points.Count; j++)
            {
                var dp = (normal.X * _points[j].X + normal.Y * _points[j].Y);
                //is dp smaller than current minimum
                if (dp < minMax[1])
                    minMax[1] = dp;
                //dp greater than current max?
                if (dp > minMax[2])
                    minMax[2] = dp;
            }
            return minMax;
        }

        public bool Overlap(Polygon other)
        {
            for (var repetition = 0; repetition < 2; repetition++)
            {
                Polygon p1;
                Polygon p2;
                if (repetition == 0)
                {
                    p1 = this;
                    p2 = other;
                }
                else
                {
                    p1 = other;
                    p2 = this;
                }
                var normals = p1.GetNormals();

                foreach (var normal in normals)
                {
                    var minMax = p1.VectorProjection(normal);
                    var otherMinMax = p2.VectorProjection(normal);

                    if (!(otherMinMax[2] > minMax[1] && minMax[2] > otherMinMax[1]))
                        return false;
                }
            }

            return true;
        }

    }
}
