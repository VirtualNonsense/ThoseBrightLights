﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Turret : EnemyWithViewbox
    {
        private Logger _logger;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Class for the turret enemy
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="viewbox"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="acceleration"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="impactSound"></param>
        public Turret(AnimationHandler animationHandler,
                      Polygon viewbox,
                      float maxSpeed = 3,
                      float acceleration = 0,
                      float health = 50,
                      float maxHealth = 50,
                      SoundEffect impactSound = null) 
            : base(animationHandler, viewbox, maxSpeed, acceleration, health: health, maxHealth: maxHealth)
        {
            _impactSound = impactSound;
            _logger = LogManager.GetCurrentClassLogger();
            RotateWeapon = true;
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