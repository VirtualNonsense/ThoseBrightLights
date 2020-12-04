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
        private readonly ExplosionEmitter _explosionEmitter;
        private Map TestMap;
        private Logger _logger;
        private MapFactory MapFactory;
        private readonly ContentManager _contentManager;

        public InGame(IScreen parent, ExplosionEmitter explosionEmitter, MapFactory mapFactory, ContentManager contentManager)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = parent;
            _explosionEmitter = explosionEmitter;
            MapFactory = mapFactory;
            _contentManager = contentManager;
        }

        public override void LoadContent()
        {
            var p = new TileSet(_contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"), 45, 45, 0);
            _explosionEmitter.TileSet = p;
            //_explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);
            var LevelBlueprint =
                JsonConvert.DeserializeObject<LevelBlueprint>(
                    File.ReadAllText(@".\Content\Level\TestLevel\TestLevel.json"));
            TestMap = MapFactory.LoadMap(LevelBlueprint);

        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
        }

        public override void Update(GameTime gameTime)
        {
            _explosionEmitter.Update(gameTime);

            _screen.Camera.Update(gameTime);
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
            TestMap.Draw(gameTime, spriteBatch);
            _explosionEmitter.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}