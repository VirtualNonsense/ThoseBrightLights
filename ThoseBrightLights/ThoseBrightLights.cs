using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
using ThoseBrightLights.Components;
using ThoseBrightLights.Core;
using ThoseBrightLights.Core.GameStates;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;

namespace ThoseBrightLights
{
    public class ThoseBrightLights : Game, IGameEngine, IScreen, IObserver<GameState>, ISaveGameHandler
    {
        private GameState _nextState;
        private GameState _currentState;
        private SpriteBatch _spriteBatch;
        private readonly ILogger _logger;
        private readonly SaveHandler _saveHandler;
        private readonly GraphicsDeviceManager _graphics;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// GameClass
        /// </summary>
        /// <param name="saveHandler"></param>
        public ThoseBrightLights(SaveHandler saveHandler)
        {
            // init logger
            _logger = LogManager.GetCurrentClassLogger();
            
            // init graphic device
            _graphics = new GraphicsDeviceManager(this);
            
            // setting target dir for content
            Content.RootDirectory = "Content";
            
            // window dimensions
            ScreenHeight = 720;
            ScreenWidth = 1280;
            
            // setting class that will handle file interaction
            _saveHandler = saveHandler;
            _logger.Debug("Constructor finished");
        }
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        
        public int ScreenHeight { get; }
        public int ScreenWidth { get; }
        public Camera Camera { get; private set; }
        public SaveGame SaveGame { get; set; }
        public SaveSlot SaveSlot { get; set; }
        
        /// <summary>
        /// Dispose to cancel subscription 
        /// </summary>
        public IDisposable StatePublisherTicket { get; set; }
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public void OnNext(GameState value)
        {
            // setting up the next state that will be loaded in the update method
            _nextState = value;
            _logger.Debug("Preparing new state");
        }

        /// <summary>
        /// will be called when state machine reaches the exit point of its routine
        /// </summary>
        public void OnCompleted()
        {
            _logger.Debug("OnCompleted(): shutting down");
            // executing exit routine
            Exit();
        }

        public void OnError(Exception error)
        {
            _logger.Error($"{error.Message}");
            throw error;
        }


        public void Render(IComponent component)
        {
            // setting up spritebatch with necessary settings
            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
                null,
                SamplerState.PointClamp, // Sharp Pixel rendering
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise, // Render only the texture side that faces the camara to boost performance 
                Camera.GetCameraEffect());
            
            // drawing component
            component.Draw(_spriteBatch);
            
            // ending drawing process
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
                // drawing triangles 
                effect.GraphicsDevice.DrawUserIndexedPrimitives(
                    type,
                    polygon.DrawAbleVertices, 
                    0,
                    polygon.DrawAbleVertices.Length,
                    polygon.VertexDrawingOrder, 
                    0,
                    polygon.TriangleCount);
                
                // drawing triangles with reversed vertex order in case it was flipped
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

        public void Save()
        {
            if (SaveGame == null)
            {
                _logger.Warn("Tried saving without savefile!");
                return;
            }
            AdaptsSettings();
            _saveHandler.Save(SaveGame, SaveSlot);
        }

        public void Load()
        {
            if (!SaveExists(SaveSlot))
            {
                _logger.Warn("Tried loading with no existent savefile!");
                return;
            }
            SaveGame = _saveHandler.Load(SaveSlot);
            AdaptsSettings();
        }

        public bool SaveExists(SaveSlot saveSlot)
        {
            return _saveHandler.SaveExists(saveSlot);
        }

        public void CreateSave()
        {
            SaveGame = new SaveGame();
            SaveGame.clearedStage = 0;
            SaveGame.damage = 0;
            SaveGame.playerPosition = 0;
            SaveGame.enemyPosition = 0;
            SaveGame.weapon = 0;
            SaveGame.score = 0;
            SaveGame.musicVolume = 0.5f;
            SaveGame.sessions = 0;
            AdaptsSettings();
        }
        
        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################
        protected override void Initialize()
        {
            _logger.Debug("Start Initialisiation");
            
            //adapting graphic settings
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();
            
            // enabling mouse
            IsMouseVisible = true;
            
            // initializing camera class
            Camera = new Camera(new Vector3(0,0,150),
                120, 
                _graphics.GraphicsDevice.Viewport, 
                new BasicEffect(_graphics.GraphicsDevice) {TextureEnabled = true});
            
            // call to base class which will finalize everything and prepare the next step
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _logger.Debug("loading content");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
                _nextState.LoadContent();
                _currentState = _nextState;
                _nextState = null;
                
            }
            _currentState?.Update(gameTime);
            _currentState?.PostUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // setting background color
            GraphicsDevice.Clear(Color.Black);
            
            _currentState.Draw();

            base.Draw(gameTime);
        }

        private void AdaptsSettings()
        {
            MediaPlayer.Volume = SaveGame.musicVolume;
            // TODO: If other Settings are saveable continue to add them HERE!
        }
    }
}