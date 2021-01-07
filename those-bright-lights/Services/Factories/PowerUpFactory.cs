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
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(new List<(int,float)> {(0,1)}, isPlaying: false);
            var hp = new HealthPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), healthbonus:health);
            hp.Layer = layer;
            hp.Position = position ?? new Vector2(0, 0);
            return hp;
        }

        public InstaDeathPowerUp DeathGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(1, isPlaying: false);
            var ikp = new InstaDeathPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings));
            ikp.Layer = layer;
            ikp.Position = position ?? new Vector2(0, 0);
            return ikp;
        }

        public WeaponPowerUp RocketGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(new List<(int, float)> { (11,1) }, isPlaying: false);
            var rp = new WeaponPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),_weaponFactory.GetMissileLauncher(_contentManager));
            rp.Layer = layer;
            rp.Position = position ?? new Vector2(0, 0);
            return rp;
        }

        public WeaponPowerUp LaserGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(new List<(int, float)> { (9, 1) }, isPlaying: false);
            var lp = new WeaponPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), _weaponFactory.EnemyGetLasergun(_contentManager));
            lp.Layer = layer;
            lp.Position = position ?? new Vector2(0, 0);
            return lp;
        }

        public AmmoPowerUp LaserAmmoGetInstance(int ammo, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(new List<(int, float)> { (22, 1) }, isPlaying: false);
            var la = new AmmoPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),ammo);
            la.Layer = layer;
            la.Position = position ?? new Vector2(0, 0);
            return la;
        }

        public AmmoPowerUp RocketAmmoGetInstance(int ammo, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(new List<(int, float)> { (22, 1) }, isPlaying: false);
            var ra = new AmmoPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),ammo);
            ra.Layer = layer;
            ra.Position = position ?? new Vector2(0, 0);
            return ra;
        }










        //public ScoreBonusPowerUp ScoreGetInstance()
        //{

        //}






    }
}
