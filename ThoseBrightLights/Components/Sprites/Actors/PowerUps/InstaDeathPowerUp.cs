using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.PowerUps
{
    // Instant death power up 
    public class InstaDeathPowerUp : PowerUp
    {
        
        public InstaDeathPowerUp(AnimationHandler animationHandler, float health = 0.01f, SoundEffect soundEffect = null) : base (animationHandler,health,soundEffect)
        {
            
        }
    }
}
