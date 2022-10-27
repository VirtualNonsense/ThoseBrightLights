using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThoseBrightLights.Extensions;
using ThoseBrightLights.Components;
using NLog;
using NLog.LayoutRenderers;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors
{
    public class Propulsion : SpaceshipAddOn
    {
        /// <summary>
        /// class for propulsion, helps with implementing
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="parent"></param>
        /// <param name="relativePosition"></param>
        /// <param name="relativeRotation"></param>
        /// <param name="impactSound"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="scale"></param>
        public Propulsion(
            AnimationHandler animationHandler,
            Actor parent,
            Vector2 relativePosition,
            float relativeRotation,
            SoundEffect impactSound,
            float health = 100,
            float maxHealth = 100,
            float? scale = null)
            : base(animationHandler,
                parent,
                relativePosition,
                relativeRotation,
                impactSound,
                health,
                maxHealth)
        {
            Scale = scale ?? 1.2f;
        }

        protected override void ExecuteInteraction(Actor other)
        {
            throw new System.NotImplementedException();
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            throw new NotImplementedException();
        }
    }
}