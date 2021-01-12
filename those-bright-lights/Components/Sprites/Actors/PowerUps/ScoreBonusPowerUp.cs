using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
{
    public class ScoreBonusPowerUp : PowerUp
    {
        public readonly int BonusScore;
        public ScoreBonusPowerUp(AnimationHandler animationHandler, int bonusScore = 50, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            BonusScore = bonusScore;
        }
    }
}
