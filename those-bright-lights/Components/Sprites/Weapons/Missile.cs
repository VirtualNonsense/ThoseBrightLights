using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Missile : Bullet
    {
        private float _damage;
        private float _speed;
        private Vector2 _direction;
        public Missile(AnimationHandler animationHandler, Vector2 direction, float damage = 10, float speed=2) : base(animationHandler)
        {
            _damage = damage;
            _speed = speed;
            _direction = direction;
        }

    }
}