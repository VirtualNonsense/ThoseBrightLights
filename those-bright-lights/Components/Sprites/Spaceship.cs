using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Spaceship : Actor
    {
        protected float Speed;
        private readonly Logger _logger;
        protected int Health;
        protected Vector2 Direction;
        protected KeyboardState CurrentKey;
        protected KeyboardState PreviousKey;
        
        public Spaceship(AnimationHandler animationHandler, int health, float speed = 1f) : base(animationHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Health = health;
            Speed = speed;
        }

        public override void BaseCollide(Actor actor)
        {
            _logger.Info(Health);
        }


    }
}