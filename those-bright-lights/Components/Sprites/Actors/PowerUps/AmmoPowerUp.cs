using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    public class AmmoPowerUp : PowerUp
    {
        public readonly float Duration;
        public AmmoPowerUp(AnimationHandler animationHandler,int duration, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            Duration = duration;
        }
    }
}
