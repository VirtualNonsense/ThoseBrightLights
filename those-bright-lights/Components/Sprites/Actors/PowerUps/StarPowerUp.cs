using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;


namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    public class StarPowerUp : PowerUp
    {
        
        public StarPowerUp(AnimationHandler animationHandler, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            
        }
    }
}
