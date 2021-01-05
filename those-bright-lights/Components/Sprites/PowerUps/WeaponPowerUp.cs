using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.Sprites.PowerUps
{
    public class WeaponPowerUp : PowerUp
    {
        public Weapon Weapon;
        public WeaponPowerUp(AnimationHandler animationHandler, Weapon weapon, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            Weapon = weapon;
        }
    }
}
