using Microsoft.Xna.Framework;
using NLog;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites
{
    public class StatusEffectParticle : Particle
    {
        private Logger _logger;
        private CastTimeAbility _destructionTimer;

        /// <summary>
        /// Particle that is meant for status effects invincibility for example 
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="destructionTime"></param>
        public StatusEffectParticle(AnimationHandler animationHandler, int destructionTime) : base(animationHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            // self destruct when time is up
            _destructionTimer = new CastTimeAbility(destructionTime, () =>
            {
                IsRemoveAble = true;
            });
            _destructionTimer.Fire();
        }

        public override void Update(GameTime gameTime)
        {
            //updating timer
            _destructionTimer.Update(gameTime);
            
            // scale decreases when time progresses
            Scale = 1 - _destructionTimer.CoolDownProgress * Scale;
            base.Update(gameTime);
        }

        public override Particle Clone()
        {
            return MemberwiseClone() as Particle;
        }
    }
}