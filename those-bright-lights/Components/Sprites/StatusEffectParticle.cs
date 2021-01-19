using Microsoft.Xna.Framework;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites
{
    public class StatusEffectParticle : Particle
    {
        private Logger _logger;
        private CastTimeAbility _destructionTimer;
        public StatusEffectParticle(AnimationHandler animationHandler, IScreen Parent, int destructionTime) : base(animationHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _destructionTimer = new CastTimeAbility(destructionTime, () =>
            {
                IsRemoveAble = true;
            });
            _destructionTimer.Fire();
        }

        public override void Update(GameTime gameTime)
        {
            _destructionTimer.Update(gameTime);
            Scale = 1 - _destructionTimer.CoolDownProgress * Scale;
            base.Update(gameTime);
        }

        public override Particle Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}