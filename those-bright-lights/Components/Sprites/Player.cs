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
        public Player(AnimationHandler animationHandler, Input input=null, int health=100, float speed = 1) 
            : base(animationHandler, health, speed)
        {
            _input = input;
            _health = health;
            _speed = speed;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override void Update(GameTime gameTime)
        {
            
            PreviousKey = CurrentKey;
            CurrentKey = Keyboard.GetState();
            
            
            #region Movement
            
            var velocity = Vector2.Zero;

            if (_currentKey.IsKeyDown(_input.Up))
            {
                velocity.Y = -_speed;
            }
            else if (_currentKey.IsKeyDown(_input.Down))
            {
                velocity.Y += _speed;
            }

            if (_currentKey.IsKeyDown(_input.Left))
            {
                velocity.X -= _speed;
            }
            if (_currentKey.IsKeyDown(_input.TurnLeft))
            {
                Rotation += 0.01f;
            }if (_currentKey.IsKeyDown(_input.TurnRight))
            {
                Rotation -= 0.01f;
            }
            
            else if (_currentKey.IsKeyDown(_input.Right))
            {
                velocity.X += _speed;
            }

            Position += velocity * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            
            #endregion
            
            #region Shoot

            if (CurrentKey.IsKeyDown(_input.Shoot))
            {
                _weapons[_currentWeapon].Shoot(_direction);
            }
            
            #endregion
            
            base.Update(gameTime);
        }

        protected override void OnOnCollide()
        {
            _health -= 1;
            if (_health <= 0)
            {
                _logger.Error("Dead!");
                IsRemoveAble = true;
            }
            else
            {
                _logger.Info(_health);
            }

            base.OnOnCollide();
        }
    }
}