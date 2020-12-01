using NLog;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class MissileLauncher : Weapon
    {
        private readonly AnimationHandler _animationHandler;
        private int _clipSize;
        private int _ammo;
        private Logger _logger;

        public MissileLauncher(AnimationHandler animationHandler)
        {
            _animationHandler = animationHandler;
            _clipSize = 5;
            _ammo = _clipSize;
            _logger = LogManager.GetCurrentClassLogger();
        }


        public override Bullet GetBullet()
        {
            if (_clipSize <= 0)
            {
                _logger.Warn("No Ammo left!");
                _clipSize--;
                return null;
            }   
            return new Missile(_animationHandler);
        }

    }
}