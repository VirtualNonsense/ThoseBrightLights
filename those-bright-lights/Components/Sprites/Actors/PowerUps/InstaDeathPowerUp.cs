using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    public class InstaDeathPowerUp : PowerUp
    {
        
        public InstaDeathPowerUp(AnimationHandler animationHandler, float health = 0.01f, SoundEffect soundEffect = null) : base (animationHandler,health,soundEffect)
        {
            
        }
    }
}
