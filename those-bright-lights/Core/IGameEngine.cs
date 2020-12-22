using System.Collections.Generic;
using SE_Praktikum.Components;
using SE_Praktikum.Models;

namespace SE_Praktikum.Core
{
    public interface IGameEngine
    {
        void Render(IEnumerable<IComponent> components);
        void Render(IComponent component);
        void Render(IEnumerable<Polygon> polygons);
        void Render(Polygon polygon);
    }
}