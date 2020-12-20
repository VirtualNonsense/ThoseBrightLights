using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class MissileLauncher : Weapon
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly TileSet _textureTileSet;
        private readonly TileSet _propulsion;
        private readonly ParticleFactory _particleFactory;
        private const int _clipSize = 50;
        private int _ammo;
        private readonly Logger _logger;
        private SoundEffect _midAirSound;
        private SoundEffect _impactSound;

        public MissileLauncher(AnimationHandlerFactory animationHandlerFactory,TileSet textureTileSet, TileSet propulsion, ParticleFactory particleFactory, SoundEffect shoot, SoundEffect midAirSound, SoundEffect impactSound) : base(shoot)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _textureTileSet = textureTileSet;
            _propulsion = propulsion;
            _particleFactory = particleFactory;
            _ammo = _clipSize;
            _logger = LogManager.GetCurrentClassLogger();
            _midAirSound = midAirSound;
            _impactSound = impactSound;
        }


        public override Bullet GetBullet(Vector2 velocitySpaceship, Vector2 positionSpaceship,float rotation, Actor parent)
        {
            if (!TryShoot()) return null;
            if (_ammo <= 0)
            {
                _logger.Warn("No Ammo left!");
                return null;
            }
            _ammo--;
            var particle = _particleFactory.BuildExplosionParticle();
            var m = new Missile(
                _animationHandlerFactory.GetAnimationHandler(
                    _textureTileSet,
                    new AnimationSettings(1, isPlaying: false, layer: parent.Layer)
                ),
                velocitySpaceship,
                positionSpaceship,
                rotation,
                _animationHandlerFactory.GetAnimationHandler(
                    _propulsion,
                    new AnimationSettings(6, 50, isLooping: true, layer: parent.Layer)
                ),
                particle,
                parent,
                _midAirSound,
                _impactSound
            );
            _shoot?.Play();
            return m;
        }

    }
}