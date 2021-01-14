using System.Collections.Generic;
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
        private readonly IGameEngine _engine;
        private IScreen _screen;
        private Logger _logger;
        private readonly ContentManager _contentManager;
        private bool _pause;
        private readonly ControlElementFactory _factory;
        private readonly ILevelContainer _levelContainer;
        private readonly ISaveGameHandler saveGameHandler;
        private ComponentGrid _components;
        private KeyboardState _lastKeyboardState;
        private Polygon _origin;

        public InGame(IGameEngine engine, 
                      IScreen screen,
                      ContentManager contentManager,
                      ControlElementFactory factory,
                      ILevelContainer levelContainer,
                      ISaveGameHandler saveGameHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _engine = engine;
            _screen = screen;
            _contentManager = contentManager;
            _pause = false;
            _lastKeyboardState = Keyboard.GetState();
            _factory = factory;
            _levelContainer = levelContainer;
            this.saveGameHandler = saveGameHandler;
            _origin = new Polygon(Vector2.Zero, Vector2.Zero, 0,new List<Vector2>
            {
                new Vector2(0,0),
                new Vector2(10,0),
                new Vector2(0,10),
            }, color: Color.Red);
        }

        public override void LoadContent()
        {
            _pause = false;
            _levelContainer.SelectedLevel?.LoadContent(_contentManager);
            _levelContainer.SelectedLevel.OnLevelComplete +=
                (sender, args) =>
                {
                    if (_levelContainer.SelectedLevel.LevelNumber >= saveGameHandler.SaveGame.clearedStage)
                    {
                        saveGameHandler.SaveGame.clearedStage++;
                        saveGameHandler.Save();
                    }
                    _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SaveAndBackToMenu);
                };
            _levelContainer.SelectedLevel.OnPlayerDead += (sender, args) =>
            {
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);
            };

                // creating pause menu
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
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);
            };
            _components.Add(backButton);
        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
            _levelContainer.SelectedLevel?.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (_lastKeyboardState.IsKeyDown(Keys.Escape) && !state.IsKeyDown(Keys.Escape)) _pause = !_pause;
            _lastKeyboardState = state;
            if(!_pause)
                _levelContainer.SelectedLevel?.Update(gameTime);
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
            _levelContainer.SelectedLevel?.PostUpdate();
        }

        public override void Draw()
        {
            _engine.Render(_origin);
            if(!_pause)
                _levelContainer.SelectedLevel?.Draw();
            else
            {
                _engine.Render(_components);
            }
        }
    }
}