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
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Core
{
    public class Level 
    {
        private readonly MapFactory _mapFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly List<IComponent> _components;
        private readonly Logger _logger;

        private event EventHandler OnExplosion;

        //Constructor
        public Level(MapFactory mapFactory, PlayerFactory playerFactory, ParticleFactory particleFactory, EnemyFactory enemyFactory)
        {
            _mapFactory = mapFactory;
            _playerFactory = playerFactory;
            _particleFactory = particleFactory;
            _enemyFactory = enemyFactory;
            _components = new List<IComponent>();
            _logger = LogManager.GetCurrentClassLogger();
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(var i in _components)
            {
                i.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            int index=0;
            while (index < _components.Count)
            {
                _components[index].Update(gameTime);
                index++;
            }

            CheckForCollisions();
            
        }

        private void CheckForCollisions()
        {
            
            var explosions = new List<IComponent>();
            var actorList = _components.OfType<Actor>().ToList();
            for (var i = 0; i < actorList.Count()-1; i++)
            {
                var actor1 = actorList[i];
                for (var j = i + 1; j < actorList.Count(); j++)
                {
                    var actor2 = actorList[j];
                    //check if one actor has the other actor as parent
                    if (actor1.Parent != null && actor1.Parent == actor2)
                        continue;
                    if (actor2.Parent != null && actor2.Parent == actor1)
                        continue;
                    
                        
                    var collisionPosition = actor1.Intersects(actor2);
                    //collision detected
                    if(!(collisionPosition is null))
                    {
                        actor1.TakeDamage(actor2);
                        actor2.TakeDamage(actor1);
                    }
                }
            }
            _components.AddRange(explosions);

            var index = 0;
            while (index < _components.Count)
            {
                if (_components[index].IsRemoveAble == true)
                {
                    _components.RemoveAt(index);
                    continue;
                }

                index++;
            }
        }


        public void OnLevelEvent(LevelEvent levelEvent, Vector2 playerPosition)
        {
            var t = (LevelEvent.ShootBullet)levelEvent;
            //if player or enemy shoots the ShootBullet event triggers
            if (t!=null)                                    
            {
                _components.Add(t.Bullet);
                _logger.Info("Shot bullet!");
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            var player = _playerFactory.GetInstance(contentManager);
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
            _components.Add(player);

            var enemy = _enemyFactory.GetInstance(contentManager);
            _components.Add(enemy);
            enemy.OnExplosion += (sender, args) =>
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