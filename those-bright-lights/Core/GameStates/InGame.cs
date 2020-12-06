using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Services.ParticleEmitter;

namespace SE_Praktikum.Core.GameStates
{
    public class InGame : GameState
    {
        private IScreen _screen;
        private Logger _logger;
        private readonly Level _level;

        public InGame(IScreen parent, MapFactory mapFactory, Level level)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = parent;
            _level = level;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _level.LoadContent(contentManager);

        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
        }

        public override void Update(GameTime gameTime)
        {

            _screen.Camera.Update(gameTime);
            _level.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                null,
                SamplerState.PointClamp, // Sharp Pixel rendering
                null,
                RasterizerState
                    .CullCounterClockwise, // Render only the texture side that faces the camara to boost performance 
                _screen.Camera.GetCameraEffect());
            _level.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}