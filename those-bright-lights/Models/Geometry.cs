using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SE_Praktikum.Models
{
    public abstract class Geometry
    {
        public float Rotation { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Origin { get; set; }

        //public abstract bool IntersectBool(Geometry other)
        //{

        //}

        //public abstract Geometry IntersectGeometry(Geometry other)
        //{

        //}

        //public override Geometry Intersect(Geometry other)
        //{
        //    switch (other)
        //    {
        //        case Rectangle r:
        //            // TODO: return intersection between rectangle and this.
        //            break;
        //        case Circle c:
        //            // TODO: ....
        //            break;
        //        case Polygon p:
        //            // TODO: ....
        //            break;
        //        default:
        //            // in case we forgot something
        //            throw new NotImplementedException();
        //    }
        //}




    }
}
