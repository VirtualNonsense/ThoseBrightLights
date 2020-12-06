using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
        private readonly ContentManager _contentManager;
        private Song _song;

        public InGame(IScreen parent, Level level, ContentManager contentManager)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = parent;
            _level = level;
            _contentManager = contentManager;
        }

        public override void LoadContent()
        {
            _level.LoadContent(_contentManager);
            _song = _contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;

        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
        }

        public override void Update(GameTime gameTime)
        {
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