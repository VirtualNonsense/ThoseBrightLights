using Microsoft.Xna.Framework;
using NLog;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using System.IO;
using SE_Praktikum.Components;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class LevelSelect : GameState
    {
        private readonly IGameEngine _engine;
        private readonly IScreen _screen;
        private readonly ControlElementFactory _factory;
        private readonly ISaveGameHandler _saveGameHandler;
        private ComponentGrid _buttons;
        private Logger _logger;
        private const string _levelPath = @".\Content\MetaData\Level\";

        public LevelSelect(IGameEngine engine, IScreen screen, ControlElementFactory factory, ISaveGameHandler saveGameHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _engine = engine;
            _screen = screen;
            _factory = factory;
            _saveGameHandler = saveGameHandler;
        }

        public override void Draw()
        {
            _engine.Render(_buttons);
        }

        public override void LoadContent()
        {
            if (_buttons != null) return;
            
            _logger.Debug("LoadingContent");
            _buttons = new ComponentGrid(new Vector2(0,0), 
                _screen.Camera.GetPerspectiveScreenWidth(),
                _screen.Camera.GetPerspectiveScreenHeight(),
                1);
            var level = Directory.GetFiles(_levelPath, "json$");
            _logger.Debug(level);
            var buttons = 3;
            uint width = (uint) (_screen.Camera.GetPerspectiveScreenWidth() / buttons);
            uint height = (uint) (_screen.Camera.GetPerspectiveScreenHeight() / buttons);
            MenuButton b = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "back to mainmenu",
                _screen.Camera);

            b.Click += (sender, args) => 
            {
                _logger.Debug("Levelselection 0");
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);
            };
            _buttons.Add(b);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void UnloadContent()
        {
            _buttons = null;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(gameTime);
            }
        }
    }
}
