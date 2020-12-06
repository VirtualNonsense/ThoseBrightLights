using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public abstract class Weapon
    {
        protected SoundEffect _shoot;

        public Weapon(SoundEffect shoot)
        {
            _shoot = shoot;
        }

        public abstract Bullet GetBullet(Vector2 velocitySpaceship, Vector2 positionSpacehsip,float Rotation, Actor parent);
        
    }
}