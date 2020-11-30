using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private Logger _logger;
        private IScreen _screen;
        private float _elapsedTime = 0;
        private int _splashscreenTime = 60;

        public Splashscreen(IScreen parent)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = parent;
        }
        
        public override void LoadContent(ContentManager contentManager)
        {

        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds/1000f;
            _logger.Trace(_elapsedTime);
            if(Keyboard.GetState().IsKeyDown(Keys.Escape) || _splashscreenTime < _elapsedTime)
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
            spriteBatch.End();
        }
    }
}