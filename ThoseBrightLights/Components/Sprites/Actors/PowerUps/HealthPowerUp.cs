using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.PowerUps
{
    // Health bonus powerup
    public class HealthPowerUp : PowerUp 
    {
        public readonly float HealthBonus;
        public HealthPowerUp(AnimationHandler animationHandler, float healthbonus, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            HealthBonus = healthbonus;
        }
    }
}
