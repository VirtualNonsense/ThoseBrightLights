using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites.Actors.PowerUps;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using System;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Player : Spaceship
    {
        private Input _input;
        private Logger _logger;
        private KeyboardState CurrentKey;
        private KeyboardState PreviousKey;
        private int _score;
        public Player(AnimationHandler animationHandler, AnimationHandler propulsion, Input input=null, float acceleration = 5, float maxSpeed = 30, int health=100, SoundEffect impactSound = null) 
            : base(animationHandler, maxSpeed, acceleration,health, impactSound)
        {
            Score = 0;
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
                    InvokeOnScoreChanged();
                    return;
                }
                _score = value;
                InvokeOnScoreChanged();
            }
        }

        public override void Update(GameTime gameTime)
        {
            PreviousKey = CurrentKey;
            CurrentKey = Keyboard.GetState();
            var t = gameTime.ElapsedGameTime.Milliseconds/10f;

            #region Movement
            var velocity = Velocity;

            if (CurrentKey.IsKeyDown(_input.Up))
            {
                velocity.Y = -Acceleration * t * t;
            }
            else if (CurrentKey.IsKeyDown(_input.Down))
            {
                velocity.Y += Acceleration * t * t;
            }
            if (CurrentKey.IsKeyDown(_input.Left))
            {
                velocity.X -= Acceleration * t * t;
            }
            else if (CurrentKey.IsKeyDown(_input.Right))
            {
                velocity.X += Acceleration * t * t;
            }
            if (CurrentKey.IsKeyDown(_input.TurnLeft))
            {
                Rotation += 0.01f;
            }
            else if (CurrentKey.IsKeyDown(_input.TurnRight))
            {
                Rotation -= 0.01f;
            }

            var newVelocity = velocity.Length();
            if (newVelocity > 0)
            {
                velocity /= newVelocity;
                newVelocity = .95f * newVelocity * (1 - newVelocity / MaxSpeed);
                if(newVelocity < 0)
                    _logger.Warn($"newVelocity < 0 consider rising MaxSpeed or decreasing acceleration");
                
                Velocity = Math.Abs(newVelocity) * velocity;
            }

            DeltaPosition = Velocity * t;
            Position += DeltaPosition;
            #endregion
            
            #region Weapon

            if (CurrentKey.IsKeyDown(_input.Shoot))
            {
                ShootCurrentWeapon();
            }
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
                case ScoreBonusPowerUp sp:
                    Score += sp.BonusScore;
                    break;
            }
            base.ExecuteInteraction(other);
        }
    }
}