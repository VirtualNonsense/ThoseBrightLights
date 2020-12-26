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
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Extensions;

namespace SE_Praktikum.Core
{
    public class Level 
    {
        private readonly MapFactory _mapFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly IScreen _screen;
        private readonly IGameEngine _gameEngine;
        private readonly List<IComponent> _components;
        private readonly Logger _logger;
        private float _collisionLayer;
        private Map _map;
        

        private event EventHandler OnExplosion;

        //Constructor
        public Level(MapFactory mapFactory, PlayerFactory playerFactory, ParticleFactory particleFactory, EnemyFactory enemyFactory, IScreen screen, IGameEngine gameEngine)
        {
            
            _mapFactory = mapFactory;
            _playerFactory = playerFactory;
            _particleFactory = particleFactory;
            _enemyFactory = enemyFactory;
            _screen = screen;
            _gameEngine = gameEngine;
            _components = new List<IComponent>();
            _logger = LogManager.GetCurrentClassLogger();
        }


        public void Draw()
        {
            foreach (var actor in _components.OfType<Actor>())
            {
                if(actor.IsCollideAble)
                    _gameEngine.Render(actor.HitBox);
                if(actor is Enemy e)
                    _gameEngine.Render(e.ViewBox);
            }
            _gameEngine.Render(_map.GetCollidable(_map.Area));
            _gameEngine.Render(_components);
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
                var mapObjects = _map.GetCollidable(actor[i].Layer, actor[i].HitBox.GetBoundingRectangle());

                for (int j = 0; j < mapObjects.Count; j++)
                {
                    actor[i].InterAct(mapObjects[j]);
                }
                
                for (int j = i+1; j < actor.Count; j++)
                {
                    actor[i].InterAct(actor[j]); 
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
                    _logger.Info("Shot bullet!");
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
            
            //TODO: try to load the json map via the contentmanager
            // var map = _mapFactory.LoadMap(JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@".\Content\MetaData\Level\AlphaMap.json")));
            _map = _mapFactory.LoadMap(@".\Content\MetaData\Level\TestLevel.json");
            _collisionLayer = _map.TopLayer;
            //TODO: Set player level to _map.TopLayer
            
            var player = _playerFactory.GetInstance(contentManager);
            player.X = 0;
            player.Y = 0;
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
            _screen.Camera.Follow(player);
            _screen.Camera.Position += new Vector3(0, 0, player.Layer);
            _components.Add(player);

            var turret = _enemyFactory.GetTurret(contentManager);
            turret.Layer = player.Layer;
            turret.X = 200;
            turret.Y = 0;
            turret.Rotation = (float) Math.PI;
            turret.OnShoot += (sender, args) =>
            {
                if (!(args is LevelEvent e)) return;
                OnLevelEvent(e);
            };
            _components.Add(turret);
            
            _components.AddRange(map);
            
            turret.OnExplosion += (sender, args) =>
            _components.Add(turret);
                
            turret.OnExplosion += (sender, args) =>
            {
                if (!(args is LevelEvent e)) return;
                OnLevelEvent(e);
            };

            // //TODO: try to load the json map via the contentmanager
            // _components.Add(_mapFactory.LoadMap(contentManager,
            //     JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@".\Content\Level\TestLevel\TestLevel.json"))));
            //
        }

    }
}