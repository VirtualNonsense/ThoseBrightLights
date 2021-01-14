using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;


namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    public class StarPowerUp : PowerUp
    {
        /// <summary>
        /// In Milliseconds
        /// </summary>
        public readonly float Duration;
        public StarPowerUp(AnimationHandler animationHandler,float duration, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            Duration = duration;
        }
    }
}
