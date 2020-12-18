using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;

namespace SE_Praktikum.Services.Factories
{
    public class TileSetFactory
    {
        private AnimationHandlerFactory _animationHandlerFactory;
        private ContentManager _contentManager;
        private Logger _logger;


        public TileSetFactory(AnimationHandlerFactory animationHandlerFactory, ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _contentManager = contentManager;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public TileSet GetInstance(string jsonpath)
        {
            var TileSet = JsonConvert.DeserializeObject<TileSetBlueprint>(File.ReadAllText(jsonpath));
            var Texture = _contentManager.Load<Texture2D>(TileSet.image);

        }
    }
}