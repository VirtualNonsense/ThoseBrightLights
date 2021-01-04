using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.Sprites.PowerUps
{
    public class ScoreBonusPowerUp : PowerUp
    {
        public readonly int BonusScore;
        public ScoreBonusPowerUp(AnimationHandler animationHandler, int bonusScore, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            BonusScore = bonusScore;
        }
    }
}
