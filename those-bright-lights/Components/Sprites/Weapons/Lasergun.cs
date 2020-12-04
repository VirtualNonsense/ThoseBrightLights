using System;
using Microsoft.Xna.Framework;
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
        private readonly Func<ExplosionsParticle> _getParticle;
        private Logger _logger;

        public Lasergun(AnimationHandlerFactory animationHandlerFactory, TileSet textureTileSet, Func<ExplosionsParticle> getParticle)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _textureTileSet = textureTileSet;
            _getParticle = getParticle;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override Bullet GetBullet(Vector2 velocitySpaceship, Vector2 positionSpaceship,float rotation, Actor parent)
        {
            Laser l = new Laser(_animationHandlerFactory.GetAnimationHandler(_textureTileSet,
                    new AnimationSettings(1, isPlaying: false)),positionSpaceship,rotation,_getParticle(), parent);
            return l;
        }
    }
}