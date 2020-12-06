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
        private readonly IScreen _screen;
        private readonly List<IComponent> _components;
        private readonly Logger _logger;
        private Map _map;
        

        private event EventHandler OnExplosion;

        //Constructor
        public Level(MapFactory mapFactory, PlayerFactory playerFactory, ParticleFactory particleFactory, EnemyFactory enemyFactory, IScreen screen)
        {
            
            _mapFactory = mapFactory;
            _playerFactory = playerFactory;
            _particleFactory = particleFactory;
            _enemyFactory = enemyFactory;
            _screen = screen;
            _components = new List<IComponent>();
            _logger = LogManager.GetCurrentClassLogger();
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(var i in _components)
            {
                i.Draw(gameTime, spriteBatch);
            }
            _map.Draw(gameTime, spriteBatch);
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
            _screen.Camera.Update(gameTime);
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
            _map = _mapFactory.LoadMap(JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@".\Content\Level\AlphaLevel\AlphaMap.json")));

            //TODO: Set player level to _map.TopLayer
            
            var player = _playerFactory.GetInstance(contentManager);
            player.X = 160;
            player.Y = 4128;
            player.Layer = _map.TopLayer;
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
            _screen.Camera.Position += new Vector3(0, 0, _map.TopLayer);
            _components.Add(player);

            var enemy = _enemyFactory.GetInstance(contentManager);
            enemy.Layer = player.Layer;
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