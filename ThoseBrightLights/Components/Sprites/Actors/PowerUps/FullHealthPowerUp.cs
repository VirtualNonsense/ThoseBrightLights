using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.PowerUps
{
    // Fullhealth powerup 
    public class FullHealthPowerUp : PowerUp
    {
        public readonly float HealthBonus;
        public FullHealthPowerUp(AnimationHandler animationHandler, float healthbonus, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            HealthBonus = healthbonus;
        }
    }
}
