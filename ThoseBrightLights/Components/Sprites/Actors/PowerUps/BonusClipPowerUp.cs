using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.PowerUps
{
    public class BonusClipPowerUp : PowerUp
    {
        private int _bonusClips;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Powerup for extra ammo
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="bonusClips"></param>
        /// <param name="health"></param>
        /// <param name="soundEffect"></param>
        public BonusClipPowerUp(AnimationHandler animationHandler,
            int bonusClips,
            float health = 0.01f,
            SoundEffect soundEffect = null) : base(animationHandler,
            health,
            soundEffect)
        {
            _bonusClips = bonusClips;
        }
    }
}