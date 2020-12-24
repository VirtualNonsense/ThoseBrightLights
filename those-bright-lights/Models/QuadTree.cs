using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SE_Praktikum.Models
{
    public class QuadTree<T>
    {
        // NOTICE: All Tests for this class are in a separate project - "CheckTrees"
        // TODO: Test specific case of Division
        List<(Rectangle, T)> objects; // changed from T to (Rectangle, T)
        int level;
        Rectangle boundary;
        QuadTree<T>[] nodes;

        // Deleted maxLevel, cuz the tile must be added no matter what (maybe other idea someday)
        int maxObjects = 10;
        bool wasDivided = false;

        public QuadTree(int level, Rectangle boundary)
        {
            this.level = level;
            objects = new List<(Rectangle, T)>();
            this.boundary = boundary;
            nodes = new QuadTree<T>[4];
        }

        // TODO: If one branch is completely empty, should be cleared.
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

        // PROBLEM(1): The -1's even if they are close won't be in this List - Need ideas...
        public int GetIndex(Rectangle rect) // DO NOT FORGET!!! : For collision I suspect all kind of forms (not just a rectangle)
        {
            // If it is not completely in a node then index must be -1
            int index = -1;
            double midPointForHorizontal = boundary.X + boundary.Width / 2;
            double midPointForVertical = boundary.Y - boundary.Height / 2;

            // Could be more beautiful with switch-case... and declare a variable for rect.Something (after checking utility) 
            // IDEA for PROBLEM(1): Check the center of one rectangle or do "soft" bounds like: int silken = 10; rect.Height-silken e.g.
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

        public void Insert(Rectangle rect, T payload)
        {
            if (wasDivided == false)
            {
                objects.Add((rect, payload));
            }

            if (wasDivided == true && objects.Count <= maxObjects)
            {
                int index = GetIndex(rect);
                if (index != -1)
                {
                    nodes[index].Insert(rect, payload);
                }
            }

            // else
            if (objects.Count > maxObjects)
            {
                Divide();
                wasDivided = true;

                int indipendentCounter = objects.Count;

                // Reposition
                List<(Rectangle, T)> dropouts = new List<(Rectangle, T)>();
                for (int i = 0; i < indipendentCounter; i++)
                {
                    (Rectangle, T) currentObject = objects[i];
                    int index = GetIndex(currentObject.Item1);

                    if (index != -1)
                    {
                        nodes[index].Insert(currentObject.Item1, currentObject.Item2);
                        dropouts.Add(currentObject);
                    }
                }

                // List won't be doubled
                int dropoutPos = 0;
                while (objects.Any() && dropouts.Any())
                {
                    objects.Remove(dropouts[dropoutPos]);
                    dropoutPos++;
                }
                dropouts.Clear();
            }
        }

        // then again: The actor could be something different than a Rectangle (Just for logic purposes)
        // TODO Retrieve new
        //public List<T> Retrieve(Rectangle actor)
        //{
        //    int index = GetIndex(actor);
        //    List<Rectangle> thinker = objects;

        //    if (wasDivided == true)
        //    {
        //        thinker = nodes[index].Retrieve(actor);
        //    }
        //    return thinker;
        //}
    }
}
