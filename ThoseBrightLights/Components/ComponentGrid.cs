using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThoseBrightLights.Components
{
    public class ComponentGrid : IEnumerable<IComponent>
    {
        private Vector2 _center;
        private readonly float _width;
        private readonly float _height;
        private readonly uint _columns;
        private List<IComponent> _components;
        
        /// <summary>
        /// class to simplify the positioning process.
        /// auto arranges given buttons in a specified field
        /// please note that the height width adjustment is currently not supported 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="columns"></param>
        public ComponentGrid( Vector2 center, float width, float height, uint columns = 1)
        {
            _center = center;
            _width = width;
            _height = height;
            _columns = columns;
            _components = new List<IComponent>();
        }
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public Vector2 Position
        {
            get => _center;
            set
            {
                _center = value;
                AlignComponents();
            }
        }
        public int Count => _components.Count;
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public void Add(IComponent component)
        {
            _components.Add(component);
            AlignComponents();
        }

        private void AlignComponents()
        {
            var maxItemsPerColumn = (int) Math.Ceiling((float)_components.Count / _columns);
            var xStep = _width / (_columns);
            var yStep = _height / (maxItemsPerColumn);
            var column = 0;
            var row = 0;
            
            // calculating the first position
            var startPos = _center +
                           new Vector2(-_width/2 + xStep/2, -_height / 2 + yStep/2); // top left corner
            foreach (var component in _components)
            {
                // adjusting the position in a grid
                component.Position = startPos + new Vector2(xStep * column, yStep * row);
                row++;
                // checks if the column end is reached
                if (row >= maxItemsPerColumn)
                {
                    row = 0;
                    column++;
                }
            }
        }

        public void Remove(IComponent component)
        {
            _components.Remove(component);
            AlignComponents();
        }

        
        public IEnumerator<IComponent> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}