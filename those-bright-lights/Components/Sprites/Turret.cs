using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Turret : Enemy
    {
        private Logger _logger;
        public Turret(AnimationHandler animationHandler, float speed = 3, float health = 50, SoundEffect impactSound = null) : base(animationHandler, speed, health, impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            RotateAndShoot = false;
            RotationSpeed = 5000;
        }

        public override void Update(GameTime gameTime)
        {
            Shoot.Update(gameTime);
            if (I == InterAction.InView && Target != null)
            {
                FinalRotation = MathExtensions.RotationToPlayer(new Vector2(Target.X - Position.X, Target.Y - Position.Y), FlippedHorizontal);
                FinalRotation = MathExtensions.Modulo2PiPositive(FinalRotation);
                // _logger.Info(Rotation);
                Shoot.Fire();
            }
            base.Update(gameTime);
        }
    }
}