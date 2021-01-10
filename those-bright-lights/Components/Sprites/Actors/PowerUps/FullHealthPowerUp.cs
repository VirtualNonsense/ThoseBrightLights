using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    public class FullHealthPowerUp : PowerUp
    {
        public readonly float HealthBonus;
        public FullHealthPowerUp(AnimationHandler animationHandler, float healthbonus, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            HealthBonus = healthbonus;
        }
    }
}
