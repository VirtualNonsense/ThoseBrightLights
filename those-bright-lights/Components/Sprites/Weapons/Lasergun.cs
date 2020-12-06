using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Lasergun : Weapon
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly TileSet _textureTileSet;
        private readonly ParticleFactory _particleFactory;
        private Logger _logger;

        public Lasergun(AnimationHandlerFactory animationHandlerFactory, TileSet textureTileSet, ParticleFactory particleFactory, SoundEffect shoot) : base(shoot)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _textureTileSet = textureTileSet;
            _particleFactory = particleFactory;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override Bullet GetBullet(Vector2 velocitySpaceship, Vector2 positionSpaceship,float rotation, Actor parent)
        {
            var particle = _particleFactory.BuildLaserExplosionParticle();
            particle.Layer = parent.Layer;
            var l = new Laser(_animationHandlerFactory.GetAnimationHandler(_textureTileSet,
                new AnimationSettings(1, isPlaying: false)), positionSpaceship, rotation, particle, parent)
            {
                Layer = parent.Layer
            };
            _shoot.Play();
            return l;
        }
    }
}