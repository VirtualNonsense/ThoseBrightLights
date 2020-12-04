using System;
using Microsoft.Xna.Framework;
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
        private readonly Func<Particle> _getParticle;
        private readonly int _clipSize = 50;
        private int _ammo;
        private Logger _logger;

        public MissileLauncher(AnimationHandlerFactory animationHandlerFactory,TileSet textureTileSet, TileSet propulsion, Func <Particle> getParticle)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _textureTileSet = textureTileSet;
            _propulsion = propulsion;
            _getParticle = getParticle;
            _ammo = _clipSize;
            _logger = LogManager.GetCurrentClassLogger();
        }


        public override Bullet GetBullet(Vector2 velocitySpaceship, Vector2 positionSpaceship)
        {
            if (_ammo <= 0)
            {
                _logger.Warn("No Ammo left!");
                return null;
            }   
            _ammo--;
            Missile m = new Missile(_animationHandlerFactory.GetAnimationHandler(_textureTileSet,
                new AnimationSettings(1, isPlaying: false)),velocitySpaceship, positionSpaceship,
                _animationHandlerFactory.GetAnimationHandler(_propulsion,new AnimationSettings(6,50, isLooping:true)),_getParticle());
            return m;
        }

    }
}