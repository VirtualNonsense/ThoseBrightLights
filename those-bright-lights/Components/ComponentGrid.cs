using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Components
{
    public class ComponentGrid : IEnumerable<IComponent>
    {
        private readonly Vector2 _center;
        private readonly float _width;
        private readonly float _height;
        private readonly uint _columns;
        private List<IComponent> _components;

        public ComponentGrid( Vector2 center, float width, float height, uint columns = 1)
        {
            _center = center;
            _width = width;
            _height = height;
            _columns = columns;
            _components = new List<IComponent>();
        }

        public void Add(IComponent component)
        {
            _components.Add(component);
            AlignComponents();
        }

        private void AlignComponents()
        {
            int maxItemsPerColumn = (int) Math.Ceiling((float)_components.Count / _columns);
            var xStep = _width / (_columns);
            var yStep = _height / (maxItemsPerColumn);
            var column = 0;
            var row = 0;
            var startPos = _center +
                           new Vector2(-_width/2 + xStep/2, -_height / 2 + yStep/2); // top left corner
            foreach (var component in _components)
            {
                component.Position = startPos + new Vector2(xStep * column, yStep * row);
                row++;
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

        public int Count => _components.Count;
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