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
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly TileSet _tileSet;
        private readonly TileSet _propulsion;
        private int _clipSize;
        private int _ammo;
        private Logger _logger;

        public MissileLauncher(AnimationHandlerFactory animationHandlerFactory, TileSet tileSet, TileSet propulsion)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _tileSet = tileSet;
            _propulsion = propulsion;
            _clipSize = 50;
            _ammo = _clipSize;
            _logger = LogManager.GetCurrentClassLogger();
        }


        public override Bullet GetBullet(Vector2 velocitySpaceship)
        {
            if (_ammo <= 0)
            {
                _logger.Warn("No Ammo left!");
                return null;
            }   
            _ammo--;
            Missile m = new Missile(_animationHandlerFactory.GetAnimationHandler(_tileSet,
                new AnimationSettings(1, isPlaying: false)),velocitySpaceship,
                _animationHandlerFactory.GetAnimationHandler(_propulsion,new AnimationSettings(6,50, isLooping:true)));
            return m;
        }

    }
}