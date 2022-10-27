using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using ThoseBrightLights.Components.Sprites.Actors.PowerUps;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Services.Factories
{
    // Enumeration for impact sounds
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
    /// <summary>
    /// Create this field to use powerUps
    /// </summary>
    public class PowerUpFactory
    {
        //fields
        public Logger _logger;
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;
        private SoundHandler<ImpactSounds> _soundHandler;
        
        // Constructor
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
        // Create powerup objects
        public HealthPowerUp HealthGetInstance(float health, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\health.json", 0);
            var animationSettings = new AnimationSettings(8, 200,layer, isLooping:true);
            var hp = new HealthPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})),
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
            var fhp = new FullHealthPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})),
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
            var ikp = new InstaDeathPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})), 
                soundEffect: _soundHandler.Get(ImpactSounds.InstaDeath))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return ikp;
        }
        


        // Random weapon creation for weapon power up spawnpoints
        public WeaponPowerUp GetRandomInstance(Vector2? position = null,float layer = 0)
        {
            var t = _weaponFactory.GetRandomWeapon(null);
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\allweapons_32_32.json",0);
            var animationSettings = t[0].NameTag switch
            {
                "Minigun" => new AnimationSettings(new List<(int, float)> {(2, 100)}),
                "Shotgun" => new AnimationSettings(new List<(int, float)> {(1, 100)}),
                "Missile Launcher" => new AnimationSettings(new List<(int, float)> {(4, 100)}),
                "Laser gun" => new AnimationSettings(new List<(int, float)> {(0, 100)}),
                _ => throw new NotImplementedException()
            };
            animationSettings.IsPlaying = false;
            animationSettings.Scale = 2;
            var rw = new WeaponPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})),
                    t, soundEffect: _soundHandler.Get(ImpactSounds.Weapon))
            {
                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return rw;
        }

        public InfAmmoPowerUp InfAmmoGetInstance(int ammo, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\infammoanimation.json", 0);
            var animationSettings = new AnimationSettings(20,50,layer,isLooping:true);
            var ra = new InfAmmoPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})), ammo, 
                soundEffect: _soundHandler.Get(ImpactSounds.Ammo))
            {
                Layer = layer, 
                Position = position ?? new Vector2(0, 0)
            };
            return ra;
        }

        public BonusClipPowerUp BonusClipGetInstance(int clips, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\ammoplus_21_30_16.json", 0);
            var animationSettings = new AnimationSettings(16,50,layer,isLooping:true);
            var pa = new BonusClipPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})), clips, 
                soundEffect: _soundHandler.Get(ImpactSounds.Ammo))
            {
                Layer = layer, 
                Position = position ?? new Vector2(0, 0)
            };
            return pa;
        }

        public StarPowerUp StarGetInstance(float duration,Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\star_21_21_10.json", 0);
            var animationSettings = new AnimationSettings(8, 50, layer, isLooping:true);
            var s = new StarPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})), duration,
                soundEffect: _soundHandler.Get(ImpactSounds.Star))
            {

                Layer = layer,
                Position = position ?? new Vector2(0, 0)
            };
            return s;
        }

        public ScoreBonusPowerUp ScoreBonusGetInstance(int bonusscore, Vector2? position = null, float layer = 0)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\moneymoneymoney_20_20_9.json", 0);
            var animationSettings = new AnimationSettings(9, 50, layer, isLooping: true, isPlaying: true) ;
            var sc = new ScoreBonusPowerUp(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})), bonusscore,
                soundEffect: _soundHandler.Get(ImpactSounds.ScoreBoni))
            {
                Layer = layer,
                
                Position = position ?? new Vector2(0, 0)
            };
            return sc;
        }
        // Load sounds for each powerup
        public void LoadSoundEffects()
        {
            _soundHandler.Add(ImpactSounds.Health,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Clip_Empty"));
            _soundHandler.Add(ImpactSounds.FullHealth,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Secret_Bell"));
            _soundHandler.Add(ImpactSounds.InstaDeath,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Doh_bassoon"));
            _soundHandler.Add(ImpactSounds.Weapon,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/West"));
            _soundHandler.Add(ImpactSounds.ScoreBoni,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Wrong"));
            _soundHandler.Add(ImpactSounds.Ammo,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/CautionOverheat"));
            _soundHandler.Add(ImpactSounds.Star,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/PowerUps/Jungle"));
        }
    }
}
