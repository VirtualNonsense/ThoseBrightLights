using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    // Infinite ammo powerup
    public class InfAmmoPowerUp : PowerUp
    {
        public readonly float Duration;
        public InfAmmoPowerUp(AnimationHandler animationHandler,int duration, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            Duration = duration;
        }
    }
}
