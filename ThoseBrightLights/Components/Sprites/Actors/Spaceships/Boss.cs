using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;
using ThoseBrightLights.Services.Abilities;

namespace ThoseBrightLights.Components.Sprites.Actors.Spaceships
{
    public class Boss : EnemyWithViewbox
    {
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Boss
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="viewBox"></param>
        /// <param name="propulsion"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="acceleration"></param>
        /// <param name="rotationAcceleration"></param>
        /// <param name="maxRotationSpeed"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="impactDamage"></param>
        /// <param name="impactSound"></param>
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
                    SoundEffect impactSound = null) 
            : base(animationHandler, viewBox, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed, health,
                maxHealth, impactDamage, impactSound)
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

        // #############################################################################################################
        // public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            Shoot.Update(gameTime);
            if (InterAction == InterAction.InView && Target != null)
                Shoot.Fire();
            base.Update(gameTime);
        }

        // #############################################################################################################
        // protected/private Methods
        // #############################################################################################################
        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.BossDiedEventArgs();
        }
    }
}