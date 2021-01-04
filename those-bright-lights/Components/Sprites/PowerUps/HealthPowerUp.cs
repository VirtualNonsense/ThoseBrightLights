using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.Sprites
{
    public class HealthPowerUp : PowerUp 
    {
        public readonly float HealthBonus;
        public HealthPowerUp(AnimationHandler animationHandler, float healthbonus, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            HealthBonus = healthbonus;
        }
    }
}
