using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using Stateless;
using Stateless.Graph;

namespace SE_Praktikum.Components.Sprites
{
    public class Player : Spaceship
    {
        private Input _input;
        private Logger _logger;
        private bool _shot = false;
        public Player(AnimationHandler animationHandler, Input input=null, int health=100, float speed = 5) 
            : base(animationHandler, health, speed)
        {
            _input = input;
            Health = health;
            Speed = speed;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override void Update(GameTime gameTime)
        {
            PreviousKey = CurrentKey;
            CurrentKey = Keyboard.GetState();

            #region Movement
            var velocity = Vector2.Zero;

            if (CurrentKey.IsKeyDown(_input.Up))
            {
                velocity.Y = -Speed;
            }
            else if (CurrentKey.IsKeyDown(_input.Down))
            {
                velocity.Y += Speed;
            }
            if (CurrentKey.IsKeyDown(_input.Left))
            {
                velocity.X -= Speed;
            }
            else if (CurrentKey.IsKeyDown(_input.Right))
            {
                velocity.X += Speed;
            }
            if (CurrentKey.IsKeyDown(_input.TurnLeft))
            {
                Rotation += 0.01f;
            }
            else if (CurrentKey.IsKeyDown(_input.TurnRight))
            {
                Rotation -= 0.01f;
            }
            Position += velocity;
            #endregion
            
            #region Weapon

            if (CurrentKey.IsKeyDown(_input.Shoot) && !_shot)
            {
                InvokeOnShoot(velocity);
                _shot = true;
            }
            else if (CurrentKey.IsKeyUp(_input.Shoot))
                _shot = false;
            #endregion
            
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
            Health -= damage;
            base.InvokeOnTakeDamage(damage);
        }
    }
}