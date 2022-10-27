using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ThoseBrightLights.Models
{
    public class QuadTree<T>
    {
        // NOTICE: All Tests for this class are in a separate project - "CheckTrees"
        // Fields
        List<(Rectangle, T)> objects; // changed from T to (Rectangle, T)
        int level;
        Rectangle boundary;
        QuadTree<T>[] nodes;

        // Deleted maxLevel, cuz the tile must be added no matter what
        int maxObjects = 150;
        bool wasDivided = false;

        // Constructor
        public QuadTree(Rectangle boundary, int level = 0)
        {
            this.level = level;
            objects = new List<(Rectangle, T)>();
            this.boundary = boundary;
            nodes = new QuadTree<T>[4];
        }

        // Clear all
        public void Clearing()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = null;
            }
        }

        // The dividing process (e.g. too much items in a branch)
        public void Divide()
        {
            int x = boundary.X;
            int y = boundary.Y;
            int newWidth = boundary.Width / 2;
            int newHeight = boundary.Height / 2;

            nodes[0] = new QuadTree<T>(new Rectangle(x + newWidth, y, newWidth, newHeight), level + 1);
            nodes[1] = new QuadTree<T>(new Rectangle(x, y, newWidth, newHeight), level + 1);
            nodes[2] = new QuadTree<T>(new Rectangle(x, y + newHeight, newWidth, newHeight), level + 1);
            nodes[3] = new QuadTree<T>(new Rectangle(x + newWidth, y + newHeight, newWidth, newHeight), level + 1);
        }

        // Getting the index - helper class
        public int GetIndex(Rectangle actual)
        {
            int index = -1;

            // Boundary
            double boundMiddleX = boundary.X + boundary.Width / 2;
            double boundMiddleY = boundary.Y + boundary.Height / 2;

            // Actual rectangle
            double actualMiddleX = actual.X + actual.Width / 2;
            double actualMiddleY = actual.Y + actual.Height / 2;
            
            // Checks where the items belongs to
            bool top = actualMiddleY <= boundMiddleY;
            bool bottom = actualMiddleY > boundMiddleY;
            bool left = actualMiddleX < boundMiddleX;
            bool right = actualMiddleX >= boundMiddleX;

            if (left)
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
            else if (right)
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

        // Remove one object
        public void Remove(T obj)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Item2.Equals(obj))
                { 
                    objects.RemoveAt(i); 
                    return; 
                }             
            }
            
            if (wasDivided)
            {
                foreach (var node in nodes)
                {
                    if (node == null) continue;
                    node.Remove(obj);
                    
                }
            }           
        }

        // Insert one object
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

        // Gives only payload back. Ultimately the function for Collision-List
        public List<T> Retrieve(Rectangle actor)
        {
            // int index = GetIndex(actor);
            if (!boundary.Intersects(actor)) return new List<T>();

            List<T> thinker = objects.Select(o => o.Item2).ToList();
            if (wasDivided)
            {
                foreach (var node in nodes)
                {
                    if (node == null) continue;
                    var rect = Rectangle.Intersect(actor, node.boundary);
                    if (rect.Height > 0 && rect.Width > 0)
                        thinker.AddRange(node.Retrieve(rect));
                }
            }
            return thinker;
        }
    }
}
