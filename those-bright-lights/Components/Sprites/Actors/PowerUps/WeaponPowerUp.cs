using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Actors.Weapons;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
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
