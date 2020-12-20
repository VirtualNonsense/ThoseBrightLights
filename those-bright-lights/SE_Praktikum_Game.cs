using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Core;
using SE_Praktikum.Core.GameStates;
using SE_Praktikum.Models;

namespace SE_Praktikum
{
    public class SE_Praktikum_Game : Game, IGameEngine, IScreen, IObserver<GameState>
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
            MediaPlayer.Volume = .3f;
            
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();
            IsMouseVisible = true;
            Camera = new Camera(new Vector3(0,0,150),
                120, 
                _graphics.GraphicsDevice.Viewport, 
                new BasicEffect(_graphics.GraphicsDevice) {TextureEnabled = true});
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _logger.Debug("loading content");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState?.LoadContent();
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
                _currentState.LoadContent();
                _nextState = null;
                
            }
            //_logger.Debug("Update!");
            _currentState?.Update(gameTime);
            _currentState?.PostUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _currentState.Draw();

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
        public Camera Camera { get; private set; }

        public void Render(IComponent component)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
                null,
                SamplerState.PointClamp, // Sharp Pixel rendering
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise, // Render only the texture side that faces the camara to boost performance 
                Camera.GetCameraEffect());
            component.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public void Render(IEnumerable<IComponent> components)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
                null,
                SamplerState.PointClamp, // Sharp Pixel rendering
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise, // Render only the texture side that faces the camara to boost performance 
                Camera.GetCameraEffect());
            foreach (var component in components)
            {
                component.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }

        public void Render(IEnumerable<Polygon> polygons)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            foreach (var polygon in polygons)
            {
                Render(polygon);
            }
        }

        public void Render(Polygon polygon)
        {
            
            if(!polygon.DrawAble) return;
            var type = PrimitiveType.TriangleList;
            var effect = Camera.GetCameraEffectForPrimitives();
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.GraphicsDevice.DrawUserIndexedPrimitives(
                    type,
                    polygon.DrawAbleVertices, 
                    0,
                    polygon.DrawAbleVertices.Length,
                    polygon.VertexDrawingOrder, 
                    0,
                    polygon.TriangleCount);
                effect.GraphicsDevice.DrawUserIndexedPrimitives(
                    type,
                    polygon.DrawAbleVertices, 
                    0,
                    polygon.DrawAbleVertices.Length,
                    polygon.VertexDrawingOrder.Reverse().ToArray(), 
                    0,
                    polygon.TriangleCount);
            }
        }
    }
}