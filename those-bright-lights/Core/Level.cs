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
            foreach(var actor1 in _components.OfType<Actor>())
            {
                foreach(var actor2 in _components.OfType<Actor>())
                {
                    //check if one actor has the other actor as parent
                    if (actor1.Parent != null && actor1.Parent == actor2)
                        continue;
                    if (actor2.Parent != null && actor2.Parent == actor1)
                        continue;
                    
                        
                    var collisionPosition = actor1.Intersects(actor2);
                    //collision detected
                    if(!(collisionPosition is null))
                    {
                        //if one actor is of type bullet remove that actor and create explosion
                        #region Actor is Bullet
                        if (actor1 is Bullet b1)
                        {
                            b1.IsRemoveAble = true;
                            explosions.Add(b1.Explosion);
                        }
                        if (actor2 is Bullet b2)
                        {
                            b2.IsRemoveAble = true;
                            explosions.Add(b2.Explosion);
                        }
                        #endregion
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
                OnLevelEvent(e, player.Position);
            };  
            _components.Add(player);

            var enemy = _enemyFactory.GetInstance(contentManager);
            _components.Add(enemy);

            // //TODO: try to load the json map via the contentmanager
            // _components.Add(_mapFactory.LoadMap(contentManager,
            //     JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@".\Content\Level\TestLevel\TestLevel.json"))));
            //
        }

    }
}