using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.PowerUps
{
    // Score bonus powerup
    public class ScoreBonusPowerUp : PowerUp
    {
        public readonly int BonusScore;
        public ScoreBonusPowerUp(AnimationHandler animationHandler, int bonusScore = 50, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            BonusScore = bonusScore;
        }
    }
}
