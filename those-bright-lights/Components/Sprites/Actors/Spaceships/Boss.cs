using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Boss : EnemyWithViewbox
    {
        public Boss(AnimationHandler animationHandler, Polygon viewBox, float speed = 3, float health = 200, SoundEffect impactSound = null) : base(animationHandler, viewBox, speed, health, impactSound)
        {
            Shoot = new CooldownAbility(500, _shootTarget);
        }

        public override void Update(GameTime gameTime)
        {
            Shoot.Update(gameTime);
            if (I == InterAction.InView && Target != null)
                Shoot.Fire();
            base.Update(gameTime);
            base.Update(gameTime);
        }
    }
}