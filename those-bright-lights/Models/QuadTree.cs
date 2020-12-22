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

        // Deleted maxLevel, cuz the tile must be added no matter what (maybe other idea someday)
        int maxObjects = 10;
        bool wasDivided = false;

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
            nodes[3] = new QuadTree<T>(level + 1, new Rectangle(x + newWidth, y - newHeight, newWidth, newHeight));
        }

        public int GetIndex(Rectangle rect) // DO NOT FORGET!!! : For collision I suspect all kind of forms (not just a rectangle)
        {
            // If it is not completely in a node then index must be -1
            int index = -1;
            double midPointForHorizontal = boundary.X + boundary.Width / 2;
            double midPointForVertical = boundary.Y - boundary.Height / 2;

            // Could be more beautiful with switch-case... and declare a variable for rect.Something (after checking utility) 
            bool top = (rect.Y - rect.Height) > midPointForVertical;
            bool bottom = rect.Y < midPointForHorizontal;

            if ((rect.X + rect.Width) < midPointForHorizontal)
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
            else if (rect.X > midPointForHorizontal)
            {
                if (top)
                {
                    index = 0;
                }
                else if (bottom)
                {
                    index = 3;
                }
            }
            return index;
        }

        public void Insert(Rectangle rect)
        {
            if (wasDivided == false)
            {
                objects.Add(rect);
            }

            if (objects.Count > maxObjects)
            {
                Divide();

                for (int i = 0; i < objects.Count; i++)
                {
                    int index = GetIndex(objects[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(objects[i]);
                        // objects won't be doubled
                        objects.Remove(objects[i]);
                    }
                }
            }
        }

        // then again: The actor could be something different than a Rectangle (Just for logic purposes)
        public List<Rectangle> Retrieve(Rectangle actor)
        {
            int index = GetIndex(actor);

            if (wasDivided == true)
            {
                nodes[index].Retrieve(actor);
            }
            return objects;
        }
    }
}
