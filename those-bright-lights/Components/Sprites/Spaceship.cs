using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Spaceship : Actor
    {
        protected float _speed;
        private Logger _logger;
        protected int _health;
        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;
        
        public Spaceship(AnimationHandler animationHandler, int health, float speed = 1f) : base(animationHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _health = health;
            _speed = speed;
        }

        public override void BaseCollide(Actor actor)
        {
            _logger.Info(_health);
        }

        public void Shoot()
        {
            throw new NotImplementedException();
        }

    }
}