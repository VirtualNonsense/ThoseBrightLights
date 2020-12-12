﻿using Microsoft.Xna.Framework;
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

        // Not sure if 5 and 10 are reasonable values... PLEASE CHECK
        int maxLevel = 5;
        int maxObjects = 10;        

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

        //TODO: retrieve function

        public int GetIndex(Rectangle rect) // DO NOT FORGET!!! : For collision I suspect all kind of forms (not just a rectangle)
        {
            // If it is not completely in a node then index must be -1
            int index = -1;
            double midPointForHorizontal = boundary.X + boundary.Width / 2;
            double midPointForVertical = boundary.Y - boundary.Height / 2;

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
            int index = GetIndex(rect);

            if (index != -1)
            {
                nodes[index].Insert(rect);

                return;
            }

            objects.Add(rect);

            // If added too many objects 
            if (objects.Count > maxObjects && level < maxLevel)
            {
                Divide();

                int i = 0;
                while (i < objects.Count)
                {
                    int index2 = GetIndex(objects[i]);

                    if (index2 != -1)
                    {
                        // Test if the index changes in node split
                        nodes[index2].Insert(objects[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        // then again: The actor could be something different than a Rectangle (Just for logic purposes)
        public List<Rectangle> Retrieve(Rectangle actor)
        {
            int index = GetIndex(actor);

            return nodes[index].objects;
        }
    }
}
