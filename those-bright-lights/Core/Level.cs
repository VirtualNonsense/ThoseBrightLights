using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Extensions;
using Microsoft.Xna.Framework.Media;
using NLog.Targets;
using SE_Praktikum.Services.Abilities;
using SE_Praktikum.Services.ParticleEmitter;

namespace SE_Praktikum.Core
{
    public class Level 
    {
        private Map _map;
        private readonly Song _song;
        private float _collisionLayer;
        private readonly Logger _logger;
        public readonly int LevelNumber;
        private readonly IScreen _screen;
        private readonly string _mapPath;
        private List<IComponent> _components;
        private readonly HUDFactory _hudFactory;
        private readonly MapFactory _mapFactory;
        private readonly IGameEngine _gameEngine;
        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly PowerUpFactory _powerUpFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly ParticleFactory _particleFactory;

        private Song _invincibilitySong;

        public Sprite _winningScreen;
        public Sprite _gameOverScreen;
        public Sprite _levelClearedScreen;

        public Sprite _exitScreen;

        private CastTimeAbility _exitScreenTimer;

        private ParticleEmitter _emitter;
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public Level(string mapPath, 
                     int levelNumber,
                     MapFactory mapFactory,
                     PlayerFactory playerFactory,
                     ParticleFactory particleFactory,
                     EnemyFactory enemyFactory,
                     PowerUpFactory powerUpFactory,
                     IScreen screen,
                     IGameEngine gameEngine,
                     HUDFactory hudFactory,
                     TileSetFactory tileSetFactory,
                     AnimationHandlerFactory animationHandlerFactory,
                     Song song = null
                     )
        {
            _logger = LogManager.GetCurrentClassLogger();
            _mapPath = mapPath;
            LevelNumber = levelNumber;
            _mapFactory = mapFactory;
            _playerFactory = playerFactory;
            _particleFactory = particleFactory;
            _enemyFactory = enemyFactory;
            _powerUpFactory = powerUpFactory;
            _screen = screen;
            _gameEngine = gameEngine;
            _hudFactory = hudFactory;
            _tileSetFactory = tileSetFactory;
            _animationHandlerFactory = animationHandlerFactory;
            _song = song;
            _components = new List<IComponent>();
            _emitter = new StarEmitter(5, 700, _particleFactory);
            _exitScreenTimer = new CastTimeAbility(2500, () => { });
        }

        // #############################################################################################################
        // Events
        // #############################################################################################################
        public event EventHandler OnLevelComplete;
        public event EventHandler OnPlayerDead;
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public bool OnEndScreen => _exitScreen != null;
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################

        public void Draw()
        {
            if (_exitScreen != null)
            {
                _gameEngine.Render(_exitScreen);
                return;
            }
            
            foreach (var actor in _components.OfType<Actor>())
            {
                if(actor.IsCollideAble)
                    _gameEngine.Render(actor.HitBox);
                if(actor is EnemyWithViewbox e)
                    _gameEngine.Render(e.ViewBox);
            }

            var tiles = _map.RetrieveItems(_map.Area);
            var polys = tiles.Where(t => t.HitBox != null).Select(t => t.HitBox);
            foreach (var poly in polys)
            {
                _gameEngine.Render(poly);
            }

            var nextPolys = _components.OfType<Spaceship>()
                .Select(t=> t.Components)
                .SelectMany(t=>
                    t.Where(s=>s.HitBox!=null)
                        .SelectMany(s=>s.HitBox));
            foreach (var poly in nextPolys)
            {
                _gameEngine.Render(poly);
            }
            

            _gameEngine.Render(tiles);
            if(_map.WinningZone != null)
                _gameEngine.Render(_map.WinningZone.Polygons);
            _gameEngine.Render(_components);
            _gameEngine.Render(_emitter);
        }

        public void Update(GameTime gameTime)
        {
            int index=0;
            while (index < _components.Count)
            {
                _components[index].Update(gameTime);
                // append only so it should be fine
                index++;
            }

            var actor = _components.OfType<Actor>().ToList();
            for (int i = 0; i < actor.Count; i++)
            {
                var mapObjects = _map.RetrieveItems(actor[i].Layer, actor[i].HitBox.GetBoundingRectangle());

                for (int j = 0; j < mapObjects.Count; j++)
                {
                    actor[i].InterAct(mapObjects[j]);
                }
                
                for (int j = i+1; j < actor.Count; j++)
                {
                    actor[i].InterAct(actor[j]); 
                }
                switch(actor[i])
                {
                    case Player p:
                        _map.ZoneUpdate(p);
                        break;
                }
            }
            _screen.Camera.Update(gameTime);
            _emitter.Update(gameTime);
            _exitScreenTimer.Update(gameTime);
        }
        public void PostUpdate()
        {
            RemoveDeadActors();
        }
        public void LoadContent(ContentManager contentManager)
        {
            // play song if available
            if (_song != null)
            {
                MediaPlayer.Play(_song);
            }
            else
                MediaPlayer.Stop();
            
            // loading map
            _map = _mapFactory.LoadMap(_mapPath);
            if(_map.WinningZone != null)
            {
                _map.WinningZone.OnZoneEntered +=
                    (sender, args) => _logger.Debug($"Player{args.Player} entered WinningZone");
                _map.WinningZone.OnZoneEntered +=
                    (sender, args) => ProcessLevelEvent(new LevelEventArgs.WinningZoneReachedEventArgs());
            }
            _collisionLayer = _map.TopLayer;
            
            // loading player
            var player = _playerFactory.GetInstance();
            player.Position = _map.PlayerSpawnPoint?.Center ?? new Vector2(0, 0);
            player.Layer = _collisionLayer;
            player.OnInvincibilityChanged += (sender, args) => ProcessLevelEvent(args);
            player.OnDeath += (sender, args) => ProcessLevelEvent(args);
            player.OnShoot += (sender, args) =>
            {
                if (!(args is LevelEventArgs e)) return;
                ProcessLevelEvent(e);
            };
            player.OnExplosion += (sender, args) =>
            {
                if (!(args is LevelEventArgs e)) return;
                ProcessLevelEvent(e);
            };
            var hud = _hudFactory.GetInstance(player);
            _components.Add(hud);
            _screen.Camera.Follow(player);
            _screen.Camera.Position += new Vector3(0, 0, player.Layer);
            _components.Add(player);
            SpawnPowerUps(player.Layer);
            SpawnEnemies(player.Layer);
            
            // loading remaining songs and effects
            _invincibilitySong = contentManager.Load<Song>("Audio/Sound_effects/PowerUps/Invincible");
            
            // loading end screens
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\gameOver_48_27.json", 0);
            var settings = new AnimationSettings(1, 10);
            _gameOverScreen =
                new Sprite(
                    _animationHandlerFactory.GetAnimationHandler(tileSet, new List<AnimationSettings> {settings}));
            tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\levelComplete_64_36.json", 0);
            _levelClearedScreen =
                new Sprite(
                    _animationHandlerFactory.GetAnimationHandler(tileSet, new List<AnimationSettings> {settings}));
            tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\youWin_48_27.json", 0);
            _winningScreen =
                new Sprite(
                    _animationHandlerFactory.GetAnimationHandler(tileSet, new List<AnimationSettings> {settings}));
            _exitScreen = null;

        }

        public void Unload()
        {
            _components = null;
        }
        

        // #############################################################################################################
        // private methods
        // #############################################################################################################
        private void RemoveDeadActors()
        {
            for (int i = 0; i < _components.Count;)
            {
                var c = _components[i];
                if (!c.IsRemoveAble)
                {
                    i++;
                    continue;
                }
                _components.RemoveAt(i);
            }
        }


        private void ProcessLevelEvent(LevelEventArgs levelEventArgs)
        {
            string info;
            switch (levelEventArgs)
            {
                case LevelEventArgs.WinningZoneReachedEventArgs _:
                    _exitScreen = _levelClearedScreen; 
                    _screen.Camera.StopFollowing();
                    _screen.Camera.Position = new Vector3(0, 0, 20);
                    _screen.Camera.Follow(_exitScreen);
                    _exitScreenTimer.Ability = InvokeOnLevelComplete;
                    _exitScreenTimer.Fire();
                    return;
                
                //if player or enemy shoots the ShootBullet event triggers
                case LevelEventArgs.ShotBulletEventArgs t:
                    _components.Add(t.Bullet);
                    t.Bullet.OnExplosion += (sender, args) =>
                    {
                        if (!(args is LevelEventArgs e)) return;
                        ProcessLevelEvent(e);
                    };
                    return;
                case LevelEventArgs.ExplosionEventArgs s:
                    if (s.Particle is null) return;
                    _components.Add(s.Particle);
                    return;
                
                case LevelEventArgs.InvincibilityChangedEventArgs i:
                    if (i.Target.Indestructible)
                    {
                        MediaPlayer.Play(_invincibilitySong);
                        if(_emitter.TargetZones.Contains(i.Target))
                            return;
                        _emitter.TargetZones.Add(i.Target);
                        return;
                    }
                    if(_song != null)
                        MediaPlayer.Play(_song);
                    else
                        MediaPlayer.Stop();
                    if(!_emitter.TargetZones.Contains(i.Target))
                        return;
                    _emitter.TargetZones.Remove(i.Target);
                    break;
                case LevelEventArgs.BossDiedEventArgs s:
                    _exitScreen = _winningScreen;
                    _screen.Camera.StopFollowing();
                    _screen.Camera.Position = new Vector3(0, 0, 20);
                    _screen.Camera.Follow(_exitScreen);
                    if (s.Aggressor is Player p1)
                    {
                        p1.Score += (int)s.Victim.MaxHealth;
                    }
                    _exitScreenTimer.Ability = InvokeOnLevelComplete;
                    _exitScreenTimer.Fire();
                    break;
                
                case LevelEventArgs.EnemyDiedEventArgs enemyDiedEventArgs:
                    if (enemyDiedEventArgs.Aggressor is Player p2)
                    {
                        p2.Score += (int) enemyDiedEventArgs.Victim.MaxHealth;
                    }
                    break;
                case LevelEventArgs.PlayerDiedEventArgs _:
                    _exitScreen = _gameOverScreen;
                    _screen.Camera.StopFollowing();
                    _screen.Camera.Position = new Vector3(0, 0, 20);
                    _screen.Camera.Follow(_exitScreen);
                    _exitScreenTimer.Ability = InvokeOnPlayerDead;
                    _exitScreenTimer.Fire();
                    break;
            }

            if (levelEventArgs is LevelEventArgs.ActorDiedEventArgs asdf)
            {
                info = asdf.Tool == asdf.Aggressor ? $"{asdf.Aggressor}" : $"{asdf.Aggressor} with {asdf.Tool}";
                _logger.Info($"{asdf.Victim} was killed by {info}");
            }
        }

        private void InvokeOnLevelComplete()
        {
            _logger.Debug("LEVEL COMPLETE!");
            OnLevelComplete?.Invoke(this, EventArgs.Empty);
        }

        private void SpawnPowerUps(float layer)
        {
            foreach(var p in _map.PowerUpSpawnpoints)
            {
                switch(p.Item1)
                {
                    case PowerUpType.HealthPowerUp:
                        var healthPowerUp = _powerUpFactory.HealthGetInstance(10, new Vector2(0,0));
                        healthPowerUp.Layer = layer;
                        healthPowerUp.Position = p.Item2;
                        healthPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        healthPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(healthPowerUp);
                        break;

                    case PowerUpType.InstaDeathPowerUp:
                        var instaDeathPowerUp = _powerUpFactory.DeathGetInstance(new Vector2(0,0));
                        instaDeathPowerUp.Layer = layer;
                        instaDeathPowerUp.Position = p.Item2;
                        instaDeathPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        instaDeathPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(instaDeathPowerUp);
                        break;

                    case PowerUpType.FullHealthPowerUp:
                        var fullHealthPowerUp = _powerUpFactory.FullHealthGetInstance(100, new Vector2(0,0));
                        fullHealthPowerUp.Layer = layer;
                        fullHealthPowerUp.Position = p.Item2;
                        fullHealthPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        fullHealthPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(fullHealthPowerUp);
                        break;

                    case PowerUpType.ScoreBonusPowerUp:
                        var scoreBonusPowerUp = _powerUpFactory.ScoreBonusGetInstance(50, new Vector2(0,0));
                        scoreBonusPowerUp.Layer = layer;
                        scoreBonusPowerUp.Position = p.Item2;
                        scoreBonusPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        scoreBonusPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(scoreBonusPowerUp);
                        break;

                    case PowerUpType.InfAmmoPowerUp:
                        var infAmmoPowerUp = _powerUpFactory.InfAmmoGetInstance(200, new Vector2(0,0));
                        infAmmoPowerUp.Layer = layer;
                        infAmmoPowerUp.Position = p.Item2;
                        infAmmoPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        infAmmoPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(infAmmoPowerUp);
                        break;
                    
                    case PowerUpType.BonusClipPowerUp:
                        var bonusClipPowerUp = _powerUpFactory.BonusClipGetInstance(2, new Vector2(0, 0));
                        bonusClipPowerUp.Layer = layer;
                        bonusClipPowerUp.Position = p.Item2;
                        bonusClipPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        bonusClipPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(bonusClipPowerUp);
                        break;

                    case PowerUpType.StarPowerUp:
                        var starPowerUp = _powerUpFactory.StarGetInstance(30000,new Vector2(0, 0));
                        starPowerUp.Layer = layer;
                        starPowerUp.Position = p.Item2;
                        starPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        starPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(starPowerUp);
                        break;

                    case PowerUpType.WeaponPowerUp:
                        var weaponPowerUp = _powerUpFactory.GetRandomInstance(new Vector2(0, 0));
                        weaponPowerUp.Layer = layer;
                        weaponPowerUp.Position = p.Item2;
                        weaponPowerUp.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        weaponPowerUp.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(weaponPowerUp);
                        break;
                }
            }
        }

        private void SpawnEnemies(float layer)
        {
            foreach(var enemySpawnPoint in _map.EnemySpawnpoints)
            {
                switch(enemySpawnPoint.Item1)
                {
                    case EnemyType.Turret:
                        var turret = _enemyFactory.GetTurret();
                        turret.Layer = layer;
                        turret.Position = enemySpawnPoint.Item2;
                        turret.Rotation = (float)Math.PI;
                        turret.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        turret.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                         
                        turret.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(turret);
                        break;
                    case EnemyType.Alienship:
                        var alienship = _enemyFactory.GetAlienShip(); 
                        alienship.Layer = layer;
                        alienship.Position = enemySpawnPoint.Item2;
                        alienship.Rotation = (float)Math.PI;
                        alienship.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        alienship.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };

                        alienship.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        alienship.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        _components.Add(alienship);
                        break;
                    case EnemyType.Boss:
                        var boss = _enemyFactory.GetBoss();
                        boss.Layer = layer;
                        boss.Position = enemySpawnPoint.Item2;
                        boss.Rotation = (float)Math.PI;
                        boss.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        boss.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        boss.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(boss);
                        break;
                    case EnemyType.Mines:
                        var mines = _enemyFactory.GetMines();
                        mines.Layer = layer;
                        mines.Position = enemySpawnPoint.Item2;
                        mines.Rotation = (float)Math.PI;
                        mines.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        mines.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };

                        mines.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(mines);
                        break;

                    case EnemyType.Kamikaze:
                        var kamikaze = _enemyFactory.GetKamikaze();
                        kamikaze.Layer = layer;
                        kamikaze.Position = enemySpawnPoint.Item2;
                        kamikaze.Rotation = (float)Math.PI;
                        kamikaze.OnDeath += (sender, args) => ProcessLevelEvent(args);
                        kamikaze.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };

                        kamikaze.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEventArgs e)) return;
                            ProcessLevelEvent(e);
                        };
                        _components.Add(kamikaze);
                        break;
                }
            }
        }

        protected virtual void InvokeOnPlayerDead()
        {
            OnPlayerDead?.Invoke(this, EventArgs.Empty);
        }
    }
}