using Microsoft.Xna.Framework;
using NLog;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using System.IO;
using System.Linq;
using SE_Praktikum.Components;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Services.StateMachines;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace SE_Praktikum.Core.GameStates
{
    public class LevelSelect : GameState, ILevelContainer
    {
        private Logger _logger;
        private ComponentGrid _buttons;
        private readonly IScreen _screen;
        private readonly IGameEngine _engine;
        private Dictionary<int,Song> _songSelection;
        private readonly LevelFactory _levelFactory;
        private readonly ContentManager contentManager;
        private readonly ControlElementFactory _factory;
        private readonly ISaveGameHandler _saveGameHandler;
        private const string _levelPath = @".\Content\MetaData\Level\";

        public LevelSelect(IGameEngine engine, IScreen screen, ControlElementFactory factory, ISaveGameHandler saveGameHandler, LevelFactory levelFactory, ContentManager contentManager)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _engine = engine;
            _screen = screen;
            _factory = factory;
            _saveGameHandler = saveGameHandler;
            _levelFactory = levelFactory;
            this.contentManager = contentManager;
        }

        public override void Draw()
        {
            _engine.Render(_buttons);
        }

        public override void LoadContent()
        {
            _logger.Debug("Level Selection: LoadingContent");
            
            // exit if buttons are already loaded
            if (_buttons != null) return;

            // load song for different levels
            _songSelection = new Dictionary<int, Song>();
            _songSelection.Add(0, contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3"));
            _songSelection.Add(1, contentManager.Load<Song>("Audio/Music/Song2_remaster2_mp3"));
            _songSelection.Add(2, contentManager.Load<Song>("Audio/Music/Song4_remaster_mp3"));

            // set camera position
            _screen.Camera.Position = new Vector3(0, 0,150);

            // initializing button management component
            _buttons = new ComponentGrid(new Vector2(0,0), 
                _screen.Camera.GetPerspectiveScreenWidth(),
                _screen.Camera.GetPerspectiveScreenHeight(),
                1);
            
            // load all level in directory
            var level = Directory.GetFiles(_levelPath, "*.json");
            
            // amount of level back button
            var buttons = level.Length + 1;
            
            // rough button dimensions
            uint buttonWidth = (uint) (_screen.Camera.GetPerspectiveScreenWidth()/3);
            uint buttonHeight = (uint) (_screen.Camera.GetPerspectiveScreenHeight() / buttons);
            
            int levelCounter = 0;
            foreach (var path in level)
            {
                // extracting levelname
                var levelName = path.Split(".json")[0].Split("\\").Last();
                
                // creating button
                var button = _factory.GetButton(
                    buttonWidth,
                    buttonHeight,
                    new Vector2(0, 0),
                    levelName,
                    _screen.Camera);

                // checking save if level is accessible
                button.Enabled = _saveGameHandler.SaveGame.clearedStage >= levelCounter;

                var levelNumber = levelCounter;
                button.Click += (sender, args) => 
                {
                    _logger.Debug($"starting {levelName}");
                    // loading level
                    SelectedLevel = _levelFactory.GetInstance(path, levelNumber, _songSelection.ContainsKey(levelNumber)? _songSelection[levelNumber] : null);
                    // progressing to gamestate
                    _subject.OnNext(GameStateMachine.GameStateMachineTrigger.StartGame);
                };
                // add button to grid
                _buttons.Add(button);
                levelCounter++;
            }
            // creating back button
            MenuButton b = _factory.GetButton(
                buttonWidth,
                buttonHeight,
                new Vector2(0, 0),
                "back to mainmenu",
                _screen.Camera);
            
            b.Click += (sender, args) => 
            {
                _logger.Debug("back to mainmenu");
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

        public Level SelectedLevel { get; private set; }
    }
}
