using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites.Actors.PowerUps;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using System;
using NLog.Fluent;
using SE_Praktikum.Extensions;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Player : Spaceship
    {
        private Input _input;
        private Logger _logger;
        private KeyboardState CurrentKey;
        private KeyboardState PreviousKey;
        private int _score;
        private float omega;
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public Player(AnimationHandler animationHandler, 
                      AnimationHandler propulsion,
                      Input input=null,
                      float acceleration = .1f,
                      float maxSpeed = 30,
                      float rotationAcceleration = .2f,
                      float maxRotationSpeed = 10,
                      int health=100,
                      SoundEffect impactSound = null) 
            : base(animationHandler, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed,health, impactSound)
        {
            Score = 0;
            _input = input;
            Health = health;
            MaxSpeed = maxSpeed;
            _logger = LogManager.GetCurrentClassLogger();
            Components.Add(
                new Propulsion(
                    propulsion,
                    this,
                    new Vector2(-75,-1),
                    0,
                    null));
        }
        // #############################################################################################################
        // Events
        // #############################################################################################################

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
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        
        
        // #############################################################################################################
        // public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            PreviousKey = CurrentKey;
            CurrentKey = Keyboard.GetState();
            var t = gameTime.ElapsedGameTime.Milliseconds/10f;

            #region Movement
            var velocity = Velocity;
            if (CurrentKey.IsKeyDown(_input.Left))
            {
                omega -= 0.5f * RotationAcceleration * t * t;
            }
            else if (CurrentKey.IsKeyDown(_input.Right))
            {
                omega += 0.5f * RotationAcceleration * t * t;
            }
            else
            {
                if (Math.Abs(Rotation) >= MathExtensions.DegToRad(2))
                {
                    var p = 0;
                    // first quadrant
                    if (Rotation >= 4.72124f || Rotation <= 1.5708f)
                        p = Rotation > Math.PI ? 1 : -1;
                    else
                        p = Rotation < Math.PI ? 1 : -1;
                    
                    omega += p * 0.25f * RotationAcceleration * t * t;
                }
            }

            omega *= 0.85f * (1 - Math.Abs(omega / MaxRotationSpeed));
            DeltaRotation = MathExtensions.DegToRad(omega*t);
            Rotation += DeltaRotation;
            if (CurrentKey.IsKeyDown(_input.Down))
            {
                velocity.X -= Acceleration * t * t;
            }
            if (CurrentKey.IsKeyDown(_input.Up))
            {
                velocity.X += Acceleration * t * t;
            }

            var newVelocity = velocity.Length();
            
            if (newVelocity > 0)
            {
                velocity /= newVelocity;
                newVelocity = 1 * newVelocity * (1 - (newVelocity / MaxSpeed));
                if(newVelocity < 0)
                    _logger.Warn($"newVelocity < 0 consider rising MaxSpeed or decreasing acceleration");
                
                Velocity = Math.Abs(newVelocity) * velocity;
            }

            DeltaPosition = (Velocity * t).Rotate(Rotation);
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
        
        
        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################

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