using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Alienship : EnemyWithViewbox
    {
        
        public Alienship(AnimationHandler animationHandler,
                         Polygon viewBox,
                         AnimationHandler propulsion,
                         float maxSpeed = 3,
                         float acceleration = 4,
                         float rotationAcceleration = .1f,
                         float maxRotationSpeed = 10,
                         float health = 50,
                         float? maxHealth = null,
                         float impactDamage = 5,
                         SoundEffect impactSound = null) : base(animationHandler, viewBox, maxSpeed, acceleration,rotationAcceleration, maxRotationSpeed, health, maxHealth, impactDamage, impactSound)
        {
            Shoot = new CooldownAbility(1000, _shootTarget);
            RotateAndShoot = true;
            Components.Add(
                new Propulsion(
                    propulsion,
                    this,
                    new Vector2(0,27),
                    0,
                    null));
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