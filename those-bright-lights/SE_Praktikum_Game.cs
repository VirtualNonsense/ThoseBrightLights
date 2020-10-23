using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Core.GameStates;

namespace SE_Praktikum
{
    public class SE_Praktikum_Game : Game, IObserver<GameState>
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ILogger _logger;
        private readonly IDisposable _statePublisherTicket;
        private GameState _currentState;
        private GameState _nextState;
        

        public SE_Praktikum_Game(IObservable<GameState> statePublisher)
        {
            // init logger
            _logger = LogManager.GetCurrentClassLogger();
            _statePublisherTicket = statePublisher.Subscribe(this);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _logger.Debug("Constructor finished");
        }

        protected override void Initialize()
        {
            _logger.Debug("Start Initialisiation");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _logger.Debug("loading content");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            _logger.Debug("unloading content");
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            if (_currentState != null)
            {
            }
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void OnNext(GameState value)
        {
            _nextState = value;
            _logger.Debug("Preparing new state");
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            _logger.Error($"{error.Message}");
            throw error;
        }
    }
}