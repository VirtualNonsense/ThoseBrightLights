using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using NLog;
using SE_Praktikum.Components.Sprites.Actors.PowerUps;

namespace SE_Praktikum.Services.Factories
{
    public class PowerUpFactory
    {
        public Logger _logger;
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;
        

        public PowerUpFactory(AnimationHandlerFactory animationHandlerFactory,WeaponFactory weaponFactory, ParticleFactory particleFactory, TileSetFactory tileSetFactory,ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            this._weaponFactory = weaponFactory;
            _particleFactory = particleFactory;
            _tileSetFactory = tileSetFactory;
            _contentManager = contentManager;
        }

        public HealthPowerUp HealthGetInstance(float health, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(frames:8, 10,layer);
            var hp = new HealthPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                healthbonus: health)
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return hp;
        }

        public FullHealthPowerUp FullHealthGetInstance(float health, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(8, 10, layer);
            var fhp = new FullHealthPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                healthbonus: health)
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return fhp;
        }

        public InstaDeathPowerUp DeathGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\skullanimation_19_24_9.json", 0);
            var animationSettings = new AnimationSettings(9, 10, layer);
            var ikp = new InstaDeathPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return ikp;
        }

        public WeaponPowerUp RocketGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missile.json", 0);
            var animationSettings = new AnimationSettings(1);
            var rp = new WeaponPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                _weaponFactory.GetMissileLauncher(null))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return rp;
        }

        public WeaponPowerUp LaserGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\laser.json", 0);
            var animationSettings = new AnimationSettings(1);
            var lp = new WeaponPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                _weaponFactory.GetEnemyLaserGun(null))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return lp;
        }

        public AmmoPowerUp LaserAmmoGetInstance(int ammo, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\infammoanimation.json", 0);
            var animationSettings = new AnimationSettings(20,10,layer);
            var la = new AmmoPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), ammo)
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return la;
        }

        public AmmoPowerUp RocketAmmoGetInstance(int ammo, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\infammoanimation.json", 0);
            var animationSettings = new AnimationSettings(20,10,layer);
            var ra = new AmmoPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), ammo)
            {
                Layer = layer, 
                Position = position ?? new Vector2(0, 0)
            };
            return ra;
        }

        public StarPowerUp StarGetInstance(float health, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(8,10,layer);
            var s = new StarPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), healthinf:health)
            {
                
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return s;
        }

        public ScoreBonusPowerUp ScoreBonusGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(1,10,layer);
            var sc = new ScoreBonusPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings))
            {
                Layer = layer,
                
                Position = position ?? new Vector2(0, 0)
            };
            return sc;
        }
    }
}
