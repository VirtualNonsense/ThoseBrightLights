using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using ThoseBrightLights.Components.Sprites.Actors.Weapons;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.PowerUps
{
    // Weapon power up
    public class WeaponPowerUp : PowerUp
    {
        public readonly List <Weapon> WeaponList;
        public WeaponPowerUp(AnimationHandler animationHandler, List <Weapon> weaponList, float health = 0.01f, SoundEffect soundEffect = null) : base(animationHandler, health, soundEffect)
        {
            WeaponList = weaponList;
        }
    }
}
