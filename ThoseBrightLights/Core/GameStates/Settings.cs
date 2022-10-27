using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using NLog;
using ThoseBrightLights.Components;
using ThoseBrightLights.Components.Controls;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services.Factories;
using ThoseBrightLights.Services.StateMachines;

namespace ThoseBrightLights.Core.GameStates
{
    public class Settings : GameState
    {
        private Logger _logger;
        private readonly IScreen _screen;
        private ComponentGrid _components;
        private readonly IGameEngine _engine;
        private readonly ControlElementFactory _factory;
        private readonly ISaveGameHandler _saveGameHandler;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public Settings(IGameEngine engine, IScreen screen, ControlElementFactory factory, ISaveGameHandler saveGameHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _engine = engine;
            _screen = screen;
            _factory = factory;
            _saveGameHandler = saveGameHandler;
        }

        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public override void LoadContent()
        {
            if (_components != null)
            {
                return;
            }

            _logger.Debug("LoadingContent");
            _components = new ComponentGrid(new Vector2(0,0), 
                _screen.Camera.GetPerspectiveScreenWidth(),
                _screen.Camera.GetPerspectiveScreenHeight(),
                2);
            var buttons = 3;
            uint width = (uint) (_screen.Camera.GetPerspectiveScreenWidth() / buttons);
            uint height = (uint) (_screen.Camera.GetPerspectiveScreenHeight() / buttons);

            var musicVolumeLabel = _factory.GetTextBoxByTiles(6, 1, Vector2.Zero, Color.Black, "Music Volume", _screen.Camera);
            
            var musicVolumeSlider = _factory.GetSlider(_saveGameHandler.SaveGame.musicVolume, 0 , 1, Vector2.Zero, width, _screen.Camera);
            musicVolumeSlider.OnValueChanged += (sender, args) => {
                _saveGameHandler.SaveGame.musicVolume = musicVolumeSlider.Value;
                MediaPlayer.Volume = musicVolumeSlider.Value;
            };

            MenuButton backButton = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "Back to main menu",
                _screen.Camera);
            backButton.Click += (sender, args) => 
            {
                _logger.Debug("Back to main menu");
                _saveGameHandler.Save();
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);
            };

            _components.Add(musicVolumeLabel);
            _components.Add(backButton);
            _components.Add(musicVolumeSlider);
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void UnloadContent()
        {
            _components = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (_components == null)
            {
                return;
            }

            foreach (var button in _components)
            {
                button.Update(gameTime);
            }
        }
   
        public override void Draw()
        {
            if (_components == null)
            {
                return;
            }
            _engine.Render(_components);
        }
    }
}
