using System.Collections.Generic;
using SE_Praktikum.Components;
using SE_Praktikum.Models;

namespace SE_Praktikum.Core
{
    /// <summary>
    /// Everything a class needs to know when rendering components
    /// </summary>
    public interface IGameEngine
    {
        /// <summary>
        /// draws a list off IComponents.
        /// </summary>
        /// <param name="components"></param>
        void Render(IEnumerable<IComponent> components);
        
        /// <summary>
        /// draws IComponent.
        /// </summary>
        /// <param name="component"></param>
        void Render(IComponent component);
        
        /// <summary>
        /// draws a list off polygon.
        /// </summary>
        /// <param name="polygons"></param>
        void Render(IEnumerable<Polygon> polygons);
        
        /// <summary>
        /// draws a polygon.
        /// </summary>
        /// <param name="polygon"></param>
        void Render(Polygon polygon);
    }
}