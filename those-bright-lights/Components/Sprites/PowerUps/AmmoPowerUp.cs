using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.Sprites.PowerUps
{
    public class AmmoPowerUp : PowerUp
    {
        public readonly int AmmoBonus;
        public AmmoPowerUp(AnimationHandler animationHandler,int ammoBonus, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            AmmoBonus = ammoBonus;
        }
    }
}
