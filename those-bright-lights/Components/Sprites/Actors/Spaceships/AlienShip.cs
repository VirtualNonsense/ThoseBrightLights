using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class AlienShip : EnemyWithViewbox
    {
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Enemy with the ship
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
        public AlienShip(AnimationHandler animationHandler,
                         Polygon viewBox,
                         AnimationHandler propulsion,
                         float maxSpeed = 3,
                         float acceleration = 4,
                         float rotationAcceleration = .1f,
                         float maxRotationSpeed = 1000,
                         float health = 50,
                         float? maxHealth = null,
                         float impactDamage = 5,
                         SoundEffect impactSound = null) 
            : base(animationHandler, viewBox, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed, health,
                maxHealth, impactDamage, impactSound)
        {
            Shoot = new CooldownAbility(1000, _shootTarget);
            RotateWeapon = false;
            Components.Add(
                new Propulsion(
                    propulsion,
                    this,
                    new Vector2(0,27),
                    0,
                    null));
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
    }
}