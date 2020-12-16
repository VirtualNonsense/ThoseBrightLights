using System;
using Microsoft.Xna.Framework.Content;
using NLog;
using SE_Praktikum.Models;

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

        public TileSet GetInstance()
        {
            throw new NotImplementedException();
        }
    }
}