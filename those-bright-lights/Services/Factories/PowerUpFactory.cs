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
    public enum ImpactSounds
    {
        Health,
        FullHealth,
        InstaDeath,
        Ammo,
        Weapon,
        ScoreBoni,
        Star
    }
    public class PowerUpFactory
    {
        public Logger _logger;
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;
        private SoundHandler<ImpactSounds> _soundHandler;
        

        public PowerUpFactory(AnimationHandlerFactory animationHandlerFactory,WeaponFactory weaponFactory, ParticleFactory particleFactory, TileSetFactory tileSetFactory, ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            this._weaponFactory = weaponFactory;
            _particleFactory = particleFactory;
            _tileSetFactory = tileSetFactory;
            _contentManager = contentManager;
            _soundHandler = new SoundHandler<ImpactSounds>();
            LoadSoundEffects();
        }

        public HealthPowerUp HealthGetInstance(float health, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(8, 200,layer, isLooping:true);
            var hp = new HealthPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                healthbonus: health, soundEffect : _soundHandler.Get(ImpactSounds.Health))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return hp;
        }

        public FullHealthPowerUp FullHealthGetInstance(float health, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(8, 30,isLooping:true);
            var fhp = new FullHealthPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                healthbonus: health, soundEffect: _soundHandler.Get(ImpactSounds.FullHealth))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return fhp;
        }

        public InstaDeathPowerUp DeathGetInstance(Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\skullanimation_19_24_9.json", 0);
            var animationSettings = new AnimationSettings(9, 50, layer, isLooping:true);
            var ikp = new InstaDeathPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), 
                soundEffect: _soundHandler.Get(ImpactSounds.InstaDeath))
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
                _weaponFactory.GetMissileLauncher(null), soundEffect: _soundHandler.Get(ImpactSounds.Weapon))
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
                _weaponFactory.GetEnemyLaserGun(null), soundEffect: _soundHandler.Get(ImpactSounds.Weapon))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return lp;
        }

        public AmmoPowerUp AmmoGetInstance(int ammo, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\infammoanimation.json", 0);
            var animationSettings = new AnimationSettings(20,50,layer,isLooping:true);
            var ra = new AmmoPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), ammo, 
                soundEffect: _soundHandler.Get(ImpactSounds.Ammo))
            {
                Layer = layer, 
                Position = position ?? new Vector2(0, 0)
            };
            return ra;
        }

        public StarPowerUp StarGetInstance(float duration,Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(8, 50, layer, isLooping:true);
            var s = new StarPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),duration,
                soundEffect: _soundHandler.Get(ImpactSounds.Star))
            {

                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return s;
        }

        public ScoreBonusPowerUp ScoreBonusGetInstance(int bonusscore, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(new List<(int, float)> { (2, 1) }, layer, isLooping: true, isPlaying: false) ;
            var sc = new ScoreBonusPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),bonusscore,
                soundEffect: _soundHandler.Get(ImpactSounds.ScoreBoni))
            {
                Layer = layer,
                
                Position = position ?? new Vector2(0, 0)
            };
            return sc;
        }

        public void LoadSoundEffects()
        {
            _soundHandler.Add(ImpactSounds.Health, _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Clip_Empty"));
            _soundHandler.Add(ImpactSounds.FullHealth, _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Secret_Bell"));
            _soundHandler.Add(ImpactSounds.InstaDeath, _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Doh_bassoon"));
            _soundHandler.Add(ImpactSounds.Weapon, _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/West"));
            _soundHandler.Add(ImpactSounds.ScoreBoni, _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Wrong"));
            _soundHandler.Add(ImpactSounds.Ammo, _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/CautionOverheat"));
            _soundHandler.Add(ImpactSounds.Star, _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Jungle"));
        }
    }
}
