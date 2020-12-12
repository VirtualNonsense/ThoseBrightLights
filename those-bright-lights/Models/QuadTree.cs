using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models
{
    public class QuadTree<T>
    {
        List<Rectangle> objects; // changed from T to Rectangle
        int level;
        Rectangle boundary;
        QuadTree<T>[] nodes;

        public QuadTree(int level, Rectangle boundary)
        {
            this.level = level;
            objects = new List<Rectangle>();
            this.boundary = boundary;
            nodes = new QuadTree<T>[4];
        }

        public void Clearing()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = null;
            }
        }

        public void Divide()
        {
            int x = boundary.X;
            int y = boundary.Y;
            int newWidth = boundary.Width / 2;
            int newHeight = boundary.Height / 2;

            nodes[0] = new QuadTree<T>(level + 1, new Rectangle(x + newWidth, y, newWidth, newHeight));
            nodes[1] = new QuadTree<T>(level + 1, new Rectangle(x, y, newWidth, newHeight));
            nodes[2] = new QuadTree<T>(level + 1, new Rectangle(x, y + newHeight, newWidth, newHeight));
            nodes[3] = new QuadTree<T>(level + 1, new Rectangle(x + newWidth, y + newHeight, newWidth, newHeight));
        }

        //TODO: insert function, collide function

        public int getIndex(Rectangle rect) // DO NOT FORGET!!! : For collision I suspect all kind of forms (not just a rectangle)
        {
            // If it is not completely in a node then index must be -1
            int index = -1;
            double vertMiddle = boundary.X + boundary.Width / 2;
            double horiMiddle = boundary.Y + boundary.Height / 2;

            bool top = (rect.Y - rect.Height) > horiMiddle;
            bool bottom = rect.Y < vertMiddle;

            if ((rect.X + rect.Width) < vertMiddle)
            {
                if (top)
                {
                    index = 1;
                }
                else if (bottom)
                {
                    index = 2;
                }
            }
            else if (rect.X > vertMiddle)
            {
                if (top)
                {
                    index = 3;
                }
                else if (bottom)
                {
                    index = 0;
                }
            }
            return index;
        }

        public void Insert(Rectangle rect)
        {
            int index = getIndex(rect);

            if (index != -1)
            {
                nodes[index].Insert(rect);

                return;
            }

            objects.Add(rect);
        }
    }
}
