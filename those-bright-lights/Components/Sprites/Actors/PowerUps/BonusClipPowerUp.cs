using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    public class BonusClipPowerUp : PowerUp
    {
        private int _bonusClips;
        public BonusClipPowerUp(AnimationHandler animationHandler, int bonusClips ,float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            _bonusClips = bonusClips;
        }
    }
}