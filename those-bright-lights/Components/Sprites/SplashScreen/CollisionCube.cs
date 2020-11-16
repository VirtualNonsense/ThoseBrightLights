using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.SplashScreen
{
    public class CollisionCube : Actor
    {
        private readonly Input _input;
        private readonly IScreen _screen;
        private KeyboardState _currentKey;
        private KeyboardState _previousKey;
        private float _speed;
        private Logger _logger;
        bool pressed = false;

        public CollisionCube(AnimationHandler animationHandler, Input input, IScreen screen) : base(animationHandler)
        {
            _input = input;
            _screen = screen;
            _speed = 1;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override void Update(GameTime gameTime)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

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
            else if (_currentKey.IsKeyDown(_input.Right))
            {
                velocity.X += _speed;
            }


            Position += velocity;

            //Position = Vector2.Clamp(Position, new Vector2(80, 0), new Vector2(_screen.ScreenWidth / 4, _screen.ScreenHeight));
            
            
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                if(!pressed)
                    _animationHandler.CurrentIndex++;
                pressed = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                if(!pressed)
                    _animationHandler.CurrentIndex--;
                pressed = true;
            }
            else if(pressed)
                pressed = false;
            
            //base.Update(gameTime);
        }


        public override void BaseCollide(Actor actor)
        {
            if (this == actor)
                return;
            
            _logger.Warn("Collission");
        }
    }
    
    
}