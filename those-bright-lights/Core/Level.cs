﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Core
{
    public class Level : IComponent
    {
        private readonly MapFactory _mapFactory;
        private readonly PlayerFactory _playerFactory;
        private List<IComponent> _components;
        private Logger _logger;

        //Constructor
        public Level(MapFactory mapFactory, PlayerFactory playerFactory)
        {
            _mapFactory = mapFactory;
            _playerFactory = playerFactory;
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
            foreach(var actor1 in _components.OfType<Actor>())
            {
                foreach(var actor2 in _components.OfType<Actor>())
                {
                    var collisionPosition = actor1.Intersects(actor2);
                    if(!(collisionPosition is null))
                    {
                        _logger.Info("Collision");
                    }
                }
            }
        }

        
        public void OnLevelEvent(LevelEvent levelEvent)
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
            _components.Add(player);
            
            //TODO: try to load the json map via the contentmanager
            _components.Add(_mapFactory.LoadMap(contentManager,
                JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@".\Content\Level\TestLevel\TestLevel.json"))));
            
        }
    }
}