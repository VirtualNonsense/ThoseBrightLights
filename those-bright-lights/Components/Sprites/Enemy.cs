using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Enemy : Spaceship
    {
        private Logger _logger;
        private bool _shot = false;
        private bool _hasToShoot;

        public Enemy(AnimationHandler animationHandler, float speed = 3, float health = 50) : base(animationHandler, speed, health)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Health = health;
        }
        
        
        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;
            if (_hasToShoot)
            {
                InvokeOnShoot(velocity);
            }
            
           
            
            base.Update(gameTime);
        }

        protected override void InvokeOnCollide()
        {
            Health -= 1;
            if (Health <= 0)
            {
                _logger.Error("Dead!");
                IsRemoveAble = true;
            }
            else
            {
                _logger.Info(Health);
            }
            base.InvokeOnCollide();
        }

        protected override void InvokeOnTakeDamage(float damage)
        {
            _logger.Info(Health);
            Health -= damage;
            base.InvokeOnTakeDamage(damage);
        }
    }
}