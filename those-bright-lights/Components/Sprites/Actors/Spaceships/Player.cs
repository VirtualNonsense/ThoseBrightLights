using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using System;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Player : Spaceship
    {
        private Input _input;
        private Logger _logger;
        private bool _shot = false;
        private KeyboardState CurrentKey;
        private KeyboardState PreviousKey;
        private int _score;
        public Player(AnimationHandler animationHandler, AnimationHandler propulsion, Input input=null, float acceleration = 5, float maxSpeed = 30, int health=100, SoundEffect impactSound = null) 
            : base(animationHandler, maxSpeed, acceleration,health, impactSound)
        {
            _input = input;
            Health = health;
            MaxSpeed = maxSpeed;
            _logger = LogManager.GetCurrentClassLogger();
            Propulsion = propulsion;
            Propulsion.Origin += new Vector2(animationHandler.FrameWidth / 2 + Propulsion.FrameWidth/2 +2, 0);
        }

        public event EventHandler OnScoreChanged;

        public int Score 
        { 
            get => _score;
            set
            {
                if (value < 0)
                {
                    _score = 0;
                    return;
                }
                _score = value;
            }
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
                ShootCurrentWeapon();
                _shot = true;
            }
            else if (CurrentKey.IsKeyUp(_input.Shoot))
                _shot = false;
            #endregion
            
            base.Update(gameTime);
        }

        protected void InvokeOnScoreChanged()
        {
            OnScoreChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Enemy e:
                    var v = _impactPolygon.Position - e.Position;
                    v /= v.Length();
                    Position += v;
                    break;
            }
            base.ExecuteInteraction(other);
        }
    }
}