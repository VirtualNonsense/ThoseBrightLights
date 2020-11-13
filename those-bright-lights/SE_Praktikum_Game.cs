using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Core.GameStates;
using SE_Praktikum.Models;

namespace SE_Praktikum
{
    public class SE_Praktikum_Game : Game, IScreen, IObserver<GameState>
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ILogger _logger;
        private GameState _currentState;
        private GameState _nextState;

        public SE_Praktikum_Game()
        {
            // init logger
            _logger = LogManager.GetCurrentClassLogger();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ScreenHeight = 720;
            ScreenWidth = 1280;
            
            _logger.Debug("Constructor finished");
        }

        public IDisposable StatePublisherTicket { get; set; }

        protected override void Initialize()
        {
            _logger.Debug("Start Initialisiation");
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();
            base.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _logger.Debug("loading content");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState?.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            _logger.Debug("unloading content");
            _currentState?.UnloadContent();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _logger.Debug("Performing Reload");
                _currentState?.UnloadContent();
                _currentState = _nextState;
                _currentState.LoadContent(Content);
                _nextState = null;
                
            }
            //_logger.Debug("Update!");
            _currentState?.Update(gameTime);
            _currentState?.PostUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        public void OnNext(GameState value)
        {
            _nextState = value;
            _logger.Debug("Preparing new state");
        }

        public void OnCompleted()
        {
            _logger.Debug("OnCompleted(): shutting down");
            Exit();
        }

        public void OnError(Exception error)
        {
            _logger.Error($"{error.Message}");
            throw error;
        }

        public int ScreenHeight { get; }
        public int ScreenWidth { get; }
    }
}