﻿using System;
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
using Microsoft.Xna.Framework.Input;

namespace SE_Praktikum.Core
{
    public class Level 
    {
        private readonly string _mapPath;
        public readonly int LevelNumber;
        private readonly MapFactory _mapFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly PowerUpFactory powerUpFactory;
        private readonly IScreen _screen;
        private readonly IGameEngine _gameEngine;
        private readonly HUDFactory hUDFactory;
        private List<IComponent> _components;
        private readonly Logger _logger;
        private float _collisionLayer;
        private Map _map;
        private MouseState _previousMousestate;
        private MouseState _mouseState;
        

        private event EventHandler OnExplosion;
        public event EventHandler OnLevelComplete;

        //Constructor
        public Level(string mapPath, 
                     int levelNumber,
                     MapFactory mapFactory,
                     PlayerFactory playerFactory,
                     ParticleFactory particleFactory,
                     EnemyFactory enemyFactory,
                     PowerUpFactory powerUpFactory,
                     IScreen screen,
                     IGameEngine gameEngine,
                     HUDFactory hUDFactory
                     )
        {
            _mapPath = mapPath;
            this.LevelNumber = levelNumber;
            _mapFactory = mapFactory;
            _playerFactory = playerFactory;
            _particleFactory = particleFactory;
            _enemyFactory = enemyFactory;
            this.powerUpFactory = powerUpFactory;
            _screen = screen;
            _gameEngine = gameEngine;
            this.hUDFactory = hUDFactory;
            _components = new List<IComponent>();
            _logger = LogManager.GetCurrentClassLogger();
        }


        public void Draw()
        {
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
        }

        public void Update(GameTime gameTime)
        {
            //_previousMousestate = _mouseState;
            //_mouseState = Mouse.GetState();
            //if (_previousMousestate.Position != _mouseState.Position)
            //    _logger.Trace($"{_screen.Camera.ProjectScreenPosIntoWorld(_mouseState.Position.ToVector2())}");
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
        }
        
        

        public void PostUpdate()
        {
            CheckForCollisions();
            RemoveDeadActors();
        }

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

        private void CheckForCollisions()
        {

        }


        private void OnLevelEvent(LevelEvent levelEvent)
        {
            switch (levelEvent)
            {
                //if player or enemy shoots the ShootBullet event triggers
                case LevelEvent.ShootBullet t:
                    _components.Add(t.Bullet);
                    t.Bullet.OnExplosion += (sender, args) =>
                    {
                        if (!(args is LevelEvent e)) return;
                        OnLevelEvent(e);
                    };
                    return;
                case LevelEvent.Explosion s:
                    if (s.Particle is null) return;
                    _components.Add(s.Particle);
                    _logger.Info("Added Particle");
                    return;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            _map = _mapFactory.LoadMap(_mapPath);
            if(_map.WinningZone != null)
            {
                _map.WinningZone.OnZoneEntered +=
                    (sender, args) => _logger.Debug($"Player{args.Player} entered WinningZone");
                _map.WinningZone.OnZoneEntered +=
                    (sender, args) => InvokeOnLevelComplete();
            }
            _collisionLayer = _map.TopLayer;
            
            var player = _playerFactory.GetInstance(contentManager);
            player.Position = _map.PlayerSpawnPoint?.Center ?? new Vector2(0, 0);
            player.Layer = _collisionLayer;
            player.OnShoot += (sender, args) =>
            {
                if (!(args is LevelEvent e)) return;
                OnLevelEvent(e);
            };
            player.OnExplosion += (sender, args) =>
            {
                if (!(args is LevelEvent e)) return;
                OnLevelEvent(e);
            };
            var hud = hUDFactory.GetInstance(player);
            _components.Add(hud);
            _screen.Camera.Follow(player);
            _screen.Camera.Position += new Vector3(0, 0, player.Layer);
            _components.Add(player);
            foreach(var p in _map.PowerUpSpawnpoints)
            {
                switch(p.Item1)
                {
                    case PowerUpType.HealthPowerUp:
                        var healthpowerup = powerUpFactory.HealthGetInstance(10, new Vector2(0,0));
                        healthpowerup.Layer = player.Layer;
                        healthpowerup.Position = p.Item2;
                        healthpowerup.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(healthpowerup);
                        break;

                    case PowerUpType.InstaDeathPowerUp:
                        var instadeathpowerup = powerUpFactory.DeathGetInstance(new Vector2(0,0));
                        instadeathpowerup.Layer = player.Layer;
                        instadeathpowerup.Position = p.Item2;
                        instadeathpowerup.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(instadeathpowerup);
                        break;

                    case PowerUpType.FullHealthPowerUp:
                        var fullhealthpowerup = powerUpFactory.FullHealthGetInstance(100, new Vector2(0,0));
                        fullhealthpowerup.Layer = player.Layer;
                        fullhealthpowerup.Position = p.Item2;
                        fullhealthpowerup.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(fullhealthpowerup);
                        break;

                    case PowerUpType.ScoreBonusPowerUp:
                        var scorebonuspowerup = powerUpFactory.ScoreBonusGetInstance(50, new Vector2(0,0));
                        scorebonuspowerup.Layer = player.Layer;
                        scorebonuspowerup.Position = p.Item2;
                        scorebonuspowerup.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(scorebonuspowerup);
                        break;

                    case PowerUpType.AmmoPowerUp:
                        var ammopowerup = powerUpFactory.AmmoGetInstance(200, new Vector2(0,0));
                        ammopowerup.Layer = player.Layer;
                        ammopowerup.Position = p.Item2;
                        ammopowerup.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(ammopowerup);
                        break;

                    case PowerUpType.StarPowerUp:
                        var starpowerup = powerUpFactory.StarGetInstance(200,new Vector2(0, 0));
                        starpowerup.Layer = player.Layer;
                        starpowerup.Position = p.Item2;
                        starpowerup.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(starpowerup);
                        break;

                    case PowerUpType.WeaponPowerUp:
                        var weaponpowerup = powerUpFactory.RocketGetInstance(new Vector2(0, 0));
                        weaponpowerup.Layer = player.Layer;
                        weaponpowerup.Position = p.Item2;
                        weaponpowerup.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(weaponpowerup);
                        break;



                }
            }
            foreach(var e in _map.EnemySpawnpoints)
            {
                switch(e.Item1)
                {
                    case EnemyType.Turret:
                        var turret = _enemyFactory.GetTurret(contentManager);
                        turret.Layer = player.Layer;
                        turret.Position = e.Item2;
                        turret.Rotation = (float)Math.PI;
                        turret.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                         
                        turret.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(turret);
                        break;
                    case EnemyType.Alienship:
                        var alienship = _enemyFactory.GetAlienship(contentManager); 
                         alienship.Layer = player.Layer;
                         alienship.Position = e.Item2;
                         alienship.Rotation = (float)Math.PI;
                         alienship.OnShoot += (sender, args) =>
                         {
                             if (!(args is LevelEvent e)) return;
                             OnLevelEvent(e);
                         };
                        
                         alienship.OnExplosion += (sender, args) =>
                         {
                             if (!(args is LevelEvent e)) return;
                             OnLevelEvent(e);
                         };
                         _components.Add(alienship);
                        break;
                    case EnemyType.Boss:
                        var boss = _enemyFactory.GetBoss(contentManager);
                        boss.Layer = player.Layer;
                        boss.Position = e.Item2;
                        boss.Rotation = (float)Math.PI;
                        boss.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };

                        boss.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(boss);
                        break;
                    case EnemyType.Mines:
                        var mines = _enemyFactory.GetMines(contentManager);
                        mines.Layer = player.Layer;
                        mines.Position = e.Item2;
                        mines.Rotation = (float)Math.PI;
                        mines.OnShoot += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };

                        mines.OnExplosion += (sender, args) =>
                        {
                            if (!(args is LevelEvent e)) return;
                            OnLevelEvent(e);
                        };
                        _components.Add(mines);
                        break;
                        
                }
            }
        }

        public void Unload()
        {
            _components = null;
        }

        public void InvokeOnLevelComplete()
        {
            OnLevelComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}