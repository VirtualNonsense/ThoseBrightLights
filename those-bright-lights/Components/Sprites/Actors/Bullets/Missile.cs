using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Bullets
{
    public class Missile : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private readonly AnimationHandler _propulsionAnimationHandler;
        private readonly Vector2 _offSet;
        private float _elapsedTime = 0;
        
        // #################################################################################################################
        // Constructor
        // #################################################################################################################
        /// <summary>
        /// Missile shot from Missile Launcher
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="spaceShipVelocity"></param>
        /// <param name="spaceShipPosition"></param>
        /// <param name="rotation"></param>
        /// <param name="propulsion"></param>
        /// <param name="explosion"></param>
        /// <param name="parent"></param>
        /// <param name="midAirSound"></param>
        /// <param name="impactSound"></param>
        /// <param name="damage"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="indestructible"></param>
        public Missile(AnimationHandler animationHandler, 
            Vector2 spaceShipVelocity,
            Vector2 spaceShipPosition,
            float rotation,
            AnimationHandler propulsion,
            Particle explosion,
            Actor parent,
            SoundEffect midAirSound,
            SoundEffect impactSound,
            float damage = 20,
            float health = 1,
            float? maxHealth = null,
            bool indestructible = false) 
            : base(animationHandler, parent, explosion, midAirSound, impactSound, damage, health: health, maxHealth: maxHealth, indestructible: indestructible)
        {
            Rotation = rotation;
            _spaceShipVelocity = spaceShipVelocity;
            var positionOffset = new Vector2(0,10);
            Position = spaceShipPosition + positionOffset;
            _propulsionAnimationHandler = propulsion;
            _propulsionAnimationHandler.Origin = new Vector2(_propulsionAnimationHandler.FrameWidth + animationHandler.FrameWidth/2,animationHandler.FrameHeight/2);
            _offSet = new Vector2(-animationHandler.FrameWidth/2-_propulsionAnimationHandler.FrameWidth/2,0);
            Acceleration = 5;
            MaxTime = 5;
        }
        
        // #################################################################################################################
        // Properties
        // #################################################################################################################
        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                if (_propulsionAnimationHandler == null) return;
                _propulsionAnimationHandler.Position = base.Position;
            }    
        }
        
        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                if (_propulsionAnimationHandler == null) return;
                _propulsionAnimationHandler.Rotation = base.Rotation;
            }    
        }

        public override float Layer
        {
            get => base.Layer;
            set
            {
                base.Layer = value;
                if (_propulsionAnimationHandler == null) return;
                _propulsionAnimationHandler.Layer = base.Layer;
            }
        }
        

        // #################################################################################################################
        // public Methods
        // #################################################################################################################
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            TimeSinceUsedMidAir += gameTime.ElapsedGameTime.Milliseconds;
            Position = Movement(_spaceShipVelocity,_elapsedTime);
            _propulsionAnimationHandler.Update(gameTime);
            if (MidAirSoundCooldown < TimeSinceUsedMidAir)
            {
                TimeSinceUsedMidAir = 0;
                MidAirSound?.Play();
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _propulsionAnimationHandler.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}