using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using NLog;
using NLog.Fluent;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class MainMenu : GameState
    {
        private readonly IGameEngine _engine;
        private readonly IScreen _screen;
        private readonly ControlElementFactory _factory;
        private ComponentGrid _buttons;
        private Logger _logger;
        private Song _song;
        private ContentManager _contentManager;
        private ISaveGameHandler _saveGameHandler;

        public MainMenu(IGameEngine engine, IScreen screen, ControlElementFactory factory, ContentManager contentManager, ISaveGameHandler saveGameHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _engine = engine;
            _screen = screen;
            _factory = factory;
            _contentManager = contentManager;
            _saveGameHandler = saveGameHandler;
        }

        public override void LoadContent()
        {
            if (_buttons != null)
            {
                return;
            }
            _screen.Camera.Position = new Vector3(0, 0,150);
            if (_song == null || MediaPlayer.Queue.ActiveSong != _song)
            {
                _song = _contentManager.Load<Song>("Audio/Music/Death_mp3");
                MediaPlayer.Play(_song);
                MediaPlayer.IsRepeating = true;
            }
            
            _logger.Debug("LoadingContent");
            _buttons = new ComponentGrid(new Vector2(0,0), 
                                      _screen.Camera.GetPerspectiveScreenWidth(),
                                      _screen.Camera.GetPerspectiveScreenHeight(),
                                      1);
            var buttons = 4;
            uint width = (uint) (_screen.Camera.GetPerspectiveScreenWidth() / 3);
            uint height = (uint) (_screen.Camera.GetPerspectiveScreenHeight() / buttons);
            MenuButton b = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                _saveGameHandler.SaveGame.sessions == 0 ? "New Game" : "Continue",
                _screen.Camera);

            b.Click += (sender, args) => 
            {
                _logger.Debug("Start new game");
                _saveGameHandler.SaveGame.sessions++;
                _saveGameHandler.Save();
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.StartLevelSelect); 
            };

            _buttons.Add(b);
            b = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "Settings",
                _screen.Camera);
            b.Click += (sender, args) => { _logger.Debug("Go to settings"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.StartSettings); };
            _buttons.Add(b);
            b = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "Back to saveselection",
                _screen.Camera);
            b.Click += (sender, args) => { _logger.Debug("Back to save selection"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back); };
            _buttons.Add(b);

            b = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "Quit",
                _screen.Camera);
            b.Click += (sender, args) => { _logger.Debug("Quit game"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.QuitGame); };
            _buttons.Add(b);

        }

        public override void UnloadContent()
        {
            _buttons = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (_buttons == null)
            {
                return;
            }
            _screen.Camera.Update(gameTime);
            foreach (var button in _buttons)
            {
                button.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            if (_buttons == null)
            {
                return;
            }
            _engine.Render(_buttons);
        }
    }
}