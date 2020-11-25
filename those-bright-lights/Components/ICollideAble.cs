

using Microsoft.Xna.Framework;
using System;

namespace SE_Praktikum.Components
{
    public interface ICollideAble
    {
        bool Collides(ICollideAble other);
        float Layer { get; }
        Rectangle HitBox { get; }
        float Rotation { get; }

        public event EventHandler<EventArgs> OnCollide;


    }
}