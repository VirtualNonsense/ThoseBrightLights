using Microsoft.Xna.Framework;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public abstract class Weapon
    {
        public abstract Bullet GetBullet(Vector2 velocitySpaceship, Vector2 positionSpacehsip,float Rotation, Actor parent);
        
    }
}