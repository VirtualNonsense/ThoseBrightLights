using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public abstract class Weapon
    {
        protected AnimationHandler AnimationHandler;

        protected event EventHandler<EventArgs> OnHit;
        

        public abstract void Shoot(Vector2 direction);

        public abstract void Update(GameTime gameTime, List<Actor> actors);
        
        protected virtual void OnOnHit()
        {
            OnHit?.Invoke(this, EventArgs.Empty);
        }
        
        
        

    }
}