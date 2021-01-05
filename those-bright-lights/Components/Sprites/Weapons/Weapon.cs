using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Bullets;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public abstract class Weapon
    {
        protected SoundEffect _shoot;
        
        /// <summary>
        /// Milliseconds
        /// </summary>
        protected int CoolDown;
        
        /// <summary>
        /// Milliseconds
        /// </summary>
        protected int ElapsedTimeSinceLastShoot;

        public bool CanShoot => ElapsedTimeSinceLastShoot > CoolDown;

        public Weapon(SoundEffect shoot)
        {
            _shoot = shoot;
        }
        public abstract Bullet GetBullet(Vector2 velocitySpaceship, Vector2 positionSpacehsip,float Rotation, Actor parent);

        public void Update(GameTime gameTime)
        {
            ElapsedTimeSinceLastShoot += gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool TryShoot()
        {
            if (!CanShoot)
                return false;
            ElapsedTimeSinceLastShoot = 0;
            return true;
        }
        
    }
}