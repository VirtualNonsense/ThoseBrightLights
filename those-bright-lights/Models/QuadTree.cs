using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models
{
    public class QuadTree<T>
    {
        List<T> objects;
        int level;
        Rectangle boundary;
        QuadTree<T> nodes;

        public QuadTree(int level, Rectangle boundary)
        {
            this.level = level;
            objects = new List<T>();
            this.boundary = boundary;
        }
    }
}
