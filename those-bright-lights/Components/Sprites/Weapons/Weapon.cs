using Microsoft.Xna.Framework;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public abstract class Weapon
    {
        

        public abstract void Shoot(Vector2 direction);

        public void Update(GameTime gameTime)
        {
            
        }
    }
}