using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;
using NLog;
using SE_Praktikum.Components.Sprites.PowerUps;

namespace SE_Praktikum.Services.Factories
{
    public class PowerUpFactory
    {
        public Logger _logger;
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;

        public PowerUpFactory(AnimationHandlerFactory animationHandlerFactory, ParticleFactory particleFactory, TileSetFactory tileSetFactory,ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _particleFactory = particleFactory;
            _tileSetFactory = tileSetFactory;
            _contentManager = contentManager;
        }

        public HealthPowerUp HealthGetInstance(float health)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(new List<(int,float)> {(0,1)}, isPlaying: false);
            var hp = new HealthPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), healthbonus:health);
            return hp;
        }

        public InstaDeathPowerUp DeathGetInstance()
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(1, isPlaying: false);
            var ikp = new InstaDeathPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings));
            return ikp;
        }

        public LaserAmmoPowerUp LAmmoGetInstance()
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(1, isPlaying: false);
            var la = new LaserAmmoPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings));
            return la;
        }

        public RocketAmmoPowerUp RAmmoGetInstance()
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(1, isPlaying: false);
            var ra = new RocketAmmoPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings));
            return ra;
        }

        //public ShotgunAmmoPowerUp SAmmoGetInstance()
        //{

        //}

        //public RocketPowerUp RocketGetInstance()
        //{

        //}

        //public ShotgunPowerUp ShotgunGetInstance()
        //{

        //}

        //public ScoreBonusPowerUp ScoreGetInstance()
        //{

        //}

        

        

        
    }
}
