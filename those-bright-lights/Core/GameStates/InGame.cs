using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Services.ParticleEmitter;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class InGame : GameState
    {
        private IScreen _screen;
        private Logger _logger;
        private readonly Level _level;
        private readonly ContentManager _contentManager;
        private Song _song;
        private bool _pause;
        private readonly ControlElementFactory _factory;
        private ComponentGrid _components;
        private KeyboardState _lastKeyboardState;

        public InGame(IScreen screen, Level level, ContentManager contentManager, ControlElementFactory factory)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = screen;
            _level = level;
            _contentManager = contentManager;
            _pause = false;
            _lastKeyboardState = Keyboard.GetState();
            _factory = factory;
        }

        public override void LoadContent()
        {
            _pause = false;
            _level.LoadContent(_contentManager);
            _song = _contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;

            _components = new ComponentGrid(new Vector2(0,0), 
                _screen.Camera.GetPerspectiveScreenWidth(),
                _screen.Camera.GetPerspectiveScreenHeight(),
                1);
            var buttons = 2;
            uint width = (uint) (_screen.Camera.GetPerspectiveScreenWidth() / buttons);
            uint height = (uint) (_screen.Camera.GetPerspectiveScreenHeight() / buttons);

            MenuButton backButton = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "Back to main menu",
                _screen.Camera);
            backButton.Click += (sender, args) =>
            {
                _logger.Debug("Back to main menu");
                _screen.Camera.StopFollowing();
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SaveAndBackToMenu);
            };
            _components.Add(backButton);
        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
        }

        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (_lastKeyboardState.IsKeyDown(Keys.Escape) && !state.IsKeyDown(Keys.Escape)) _pause = !_pause;
            _lastKeyboardState = state;
            if(!_pause)
                _level.Update(gameTime);
            else
            {
                _components.Position = new Vector2( _screen.Camera.Position.X, _screen.Camera.Position.Y);
                foreach (var component in _components)
                {
                    component.Update(gameTime);
                }
            }
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
            
            if(!_pause)
                _level.Draw(gameTime, spriteBatch);
            else
            {
                foreach (var component in _components)
                {
                    component.Draw(gameTime, spriteBatch);
                }
            }
            spriteBatch.End();
        }
    }
}