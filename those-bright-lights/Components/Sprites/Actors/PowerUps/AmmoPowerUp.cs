using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.PowerUps
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
