using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Mine : Enemy
    {
        public Mine(AnimationHandler animationHandler,
            Particle explosion,
            SoundEffect impactSound,
            float health = 1,
            float? maxHealth = null,
            float impactDamage = 5,
            bool indestructible = false) : base(animationHandler, health: health)
        {
            Explosion = explosion;
        }

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