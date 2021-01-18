using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services;
using SE_Praktikum.Components;
using NLog;
using NLog.LayoutRenderers;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Models;

namespace SE_Praktikum.Components.Sprites.Actors
{
    public class Propulsion : SpaceshipAddOn
    {
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