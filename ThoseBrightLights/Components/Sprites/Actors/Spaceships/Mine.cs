using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.Spaceships
{
    public class Mine : Enemy
    {
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Mine that explodes when something touches it
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="explosion"></param>
        /// <param name="impactSound"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="impactDamage"></param>
        /// <param name="indestructible"></param>
        public Mine(AnimationHandler animationHandler,
            Particle explosion,
            SoundEffect impactSound,
            float health = 1,
            float? maxHealth = null,
            float impactDamage = 10000,
            bool indestructible = false) : base(animationHandler, health: health)
        {
            Explosion = explosion;
            Damage = impactDamage;
        }

        // #############################################################################################################
        // protected/private Methods
        // #############################################################################################################
        protected override void InvokeDeath()
        {
            base.InvokeDeath();
            base.InvokeExplosion();
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.EnemyDiedEventArgs();
        }
    }
}