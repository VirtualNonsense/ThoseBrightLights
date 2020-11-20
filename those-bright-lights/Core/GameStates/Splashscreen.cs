using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.SplashScreen;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.ParticleEmitter;
using SE_Praktikum.Services.StateMachines;
using SE_Praktikum.Components;
using SE_Praktikum.Services.Factories;
using Newtonsoft.Json;
using SE_Praktikum.Models.Tiled;
using System.IO;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private IScreen _screen;
        private int _splashscreenTime = 60;
        private readonly ExplosionEmitter _explosionEmitter;
        private Map TestMap;
        public Song _song;
        private Logger _logger;
        private MapFactory MapFactory;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter, MapFactory mapFactory)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = parent;
            _explosionEmitter = explosionEmitter;
            MapFactory = mapFactory;
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            var p = new Animation(contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"), 7);
            _explosionEmitter.Animation = p;
            //_explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);
            var LevelBlueprint = JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@".\Content\Level\TestLevel\TestLevel.json"));
            TestMap = MapFactory.LoadMap(contentManager, LevelBlueprint);

        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
        }

        public override void Update(GameTime gameTime)
        {
            _explosionEmitter.Update(gameTime);
            if(Keyboard.GetState().IsKeyDown(Keys.Space) || _splashscreenTime < gameTime.ElapsedGameTime.Seconds)
            {
                _logger.Debug("exiting splashscreen");
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SkipSplashScreen);
            }
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
                              RasterizerState.CullCounterClockwise, // Render only the texture side that faces the camara to boost performance 
                              _screen.Camera.GetCameraEffect());
            TestMap.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}