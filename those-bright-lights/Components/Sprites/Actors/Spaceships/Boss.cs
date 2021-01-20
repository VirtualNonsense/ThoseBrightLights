using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Boss : EnemyWithViewbox
    {
        public Boss(AnimationHandler animationHandler, 
                    Polygon viewBox,
                    AnimationHandler propulsion,
                    float maxSpeed = 3,
                    float acceleration = 4,
                    float rotationAcceleration = .1f,
                    float maxRotationSpeed = 1000,
                    float health = 200,
                    float? maxHealth = null,
                    float impactDamage = 20,
                    SoundEffect impactSound = null) : base(animationHandler, viewBox, maxSpeed, acceleration,rotationAcceleration, maxRotationSpeed, health, maxHealth, impactDamage, impactSound)
        {
            Shoot = new CooldownAbility(500, _shootTarget);
            RotateWeapon = true;
            Components.Add(
                new Propulsion(
                    propulsion,
                    this,
                    new Vector2(-55,64),
                    0,
                    null,
                    scale: Scale*1.8f));
        }

        public override void Update(GameTime gameTime)
        {
            Shoot.Update(gameTime);
            if (InterAction == InterAction.InView && Target != null)
                Shoot.Fire();
            base.Update(gameTime);
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.BossDiedEventArgs();
        }
    }
}