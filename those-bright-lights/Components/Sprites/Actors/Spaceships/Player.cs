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
        private readonly Input _input;
        private readonly Logger _logger;
        private KeyboardState _currentKey;
        private KeyboardState _previousKey;
        private int _score;
        private float _omega;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// class for the Player
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="propulsion"></param>
        /// <param name="input"></param>
        /// <param name="acceleration"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="rotationAcceleration"></param>
        /// <param name="maxRotationSpeed"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="impactDamage"></param>
        /// <param name="impactSound"></param>
        public Player(AnimationHandler animationHandler, 
                      AnimationHandler propulsion,
                      Input input=null,
                      float acceleration = .1f,
                      float maxSpeed = 30,
                      float rotationAcceleration = .2f,
                      float maxRotationSpeed = 10,
                      float health = 100,
                      float? maxHealth = null,
                      float impactDamage = 5,
                      SoundEffect impactSound = null) 
            : base(animationHandler, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed, health,
                maxHealth: maxHealth, impactSound: impactSound, impactDamage: impactDamage)
        {
            Score = 0;
            _input = input;
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

        // #############################################################################################################
        // Properties
        // #############################################################################################################
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
        // public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            var t = gameTime.ElapsedGameTime.Milliseconds/10f;

            #region Movement
            var velocity = Velocity;
            if (_currentKey.IsKeyDown(_input.Left))
            {
                _omega -= 0.5f * RotationAcceleration * t * t;
            }
            else if (_currentKey.IsKeyDown(_input.Right))
            {
                _omega += 0.5f * RotationAcceleration * t * t;
            }
            // else
            // {
            //     if (Math.Abs(Rotation) >= MathExtensions.DegToRad(2))
            //     {
            //         var p = 0;
            //         // first quadrant
            //         if (Rotation >= 4.72124f || Rotation <= 1.5708f)
            //             p = Rotation > Math.PI ? 1 : -1;
            //         else
            //             p = Rotation < Math.PI ? 1 : -1;
            //         
            //         omega += p * 0.25f * RotationAcceleration * t * t;
            //     }
            // }

            _omega *= 0.85f * (1 - Math.Abs(_omega / MaxRotationSpeed));
            DeltaRotation = MathExtensions.DegToRad(_omega*t);
            Rotation += DeltaRotation;
            if (_currentKey.IsKeyDown(_input.Down))
            {
                velocity.X -= Acceleration * t * t;
            }
            if (_currentKey.IsKeyDown(_input.Up))
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

            if (_currentKey.IsKeyDown(_input.Shoot))
            {
                ShootCurrentWeapon();
            }
            #endregion
            
            base.Update(gameTime);
        }
        
        
        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################

        private void InvokeOnScoreChanged()
        {
            OnScoreChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case ScoreBonusPowerUp sp:
                    Score += sp.BonusScore;
                    break;
            }
            base.ExecuteInteraction(other);
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.PlayerDiedEventArgs();
        }
    }
}