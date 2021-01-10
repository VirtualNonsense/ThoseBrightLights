using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Turret : EnemyWithViewbox
    {
        private Logger _logger;
        public Turret(AnimationHandler animationHandler, Polygon viewbox, float maxSpeed = 3, float acceleration = 0,float health = 50, SoundEffect impactSound = null) : base(animationHandler, viewbox, maxSpeed, acceleration)
        {
            _logger = LogManager.GetCurrentClassLogger();
            RotateAndShoot = true;
        }

        public override void Update(GameTime gameTime)
        {
            Shoot.Update(gameTime);
            if (I == InterAction.InView && Target != null)
                Shoot.Fire();
            base.Update(gameTime);
        }
    }
}