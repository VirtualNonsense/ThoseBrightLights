using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class InGame : GameState
    {
        private bool _pause;
        private Logger _logger;
        private IScreen _screen;
        private Polygon _origin;
        private ComponentGrid _components;
        private readonly IGameEngine _engine;
        private KeyboardState _lastKeyboardState;
        private readonly ContentManager _contentManager;
        private readonly ControlElementFactory _factory;
        private readonly ILevelContainer _levelContainer;
        private readonly ISaveGameHandler saveGameHandler;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
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

        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public override void LoadContent()
        {
            _pause = false;
            _levelContainer.SelectedLevel.LoadContent(_contentManager);
            _levelContainer.SelectedLevel.OnLevelComplete += SaveAndQuit;
            _levelContainer.SelectedLevel.OnPlayerDead += PlayerDied;

            // creating pause menu
            _components = new ComponentGrid(new Vector2(0,0), 
                _screen.Camera.GetPerspectiveScreenWidth(),
                _screen.Camera.GetPerspectiveScreenHeight(),
                1);
            var buttons = 2;
            uint width = (uint) (_screen.Camera.GetPerspectiveScreenWidth() / buttons);
            uint height = (uint) (_screen.Camera.GetPerspectiveScreenHeight() / buttons);

            MenuButton continueButton = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "Continue",
                _screen.Camera);
            continueButton.Click += (sender, args) =>
            {
                _logger.Debug("Coninueing the game");
                _pause = false;
            };
            
            _components.Add(continueButton);
            MenuButton backButton = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "Back to main menu",
                _screen.Camera);
            backButton.Click += (sender, args) =>
            {
                _logger.Debug("Back to main menu");
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);
            };
            _components.Add(backButton);
        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
            _levelContainer.SelectedLevel.OnLevelComplete -=  SaveAndQuit;
            _levelContainer.SelectedLevel.OnPlayerDead -= PlayerDied;

            _levelContainer.SelectedLevel?.Unload();
            _screen.Camera.StopFollowing();
        }

        public override void Update(GameTime gameTime)
        {
            // check for keyboard input and set pause flag accordingly
            var state = Keyboard.GetState();
            if (_lastKeyboardState.IsKeyDown(Keys.Escape) && !state.IsKeyDown(Keys.Escape)) _pause = !_pause;
            _lastKeyboardState = state;
            
            // updates game when unpaused
            if(!_pause)
                _levelContainer.SelectedLevel?.Update(gameTime);
            
            // updates menu when unpaused
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
            // rendering origin marker (useful or debugging) 
            _engine.Render(_origin);
            
            // rendering level when unpaused
            if(!_pause)
                _levelContainer.SelectedLevel?.Draw();
            
            // rendering menu when paused
            else
            {
                _engine.Render(_components);
            }
        }
        
        // #############################################################################################################
        // private methods
        // #############################################################################################################
        private void SaveAndQuit(object sender, EventArgs args)
        {
            // checking if level was cleared bevor
            if (_levelContainer.SelectedLevel.LevelNumber >= saveGameHandler.SaveGame.clearedStage)
            {
                // marking level as cleared and saving progress
                saveGameHandler.SaveGame.clearedStage++;
                saveGameHandler.Save();
            }
            _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);
        }

        private void PlayerDied(object sender, EventArgs args)
        {
            _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);
            
        }
    }
}