using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        private readonly IScreen _screen;
        private readonly ControlElementFactory _factory;
        private ComponentGrid _buttons;
        private Logger _logger;

        public MainMenu(IScreen screen, ControlElementFactory factory)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = screen;
            _factory = factory;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            if (_buttons != null)
            {
                return;
            }

            _logger.Debug("LoadingContent");
            _buttons = new ComponentGrid(new Vector2(0,0), 
                                      _screen.Camera.GetPerspectiveScreenWidth(),
                                      _screen.Camera.GetPerspectiveScreenHeight(),
                                      1);
            var buttons = 3;
            uint width = (uint) (_screen.Camera.GetPerspectiveScreenWidth() / buttons);
            uint height = (uint) (_screen.Camera.GetPerspectiveScreenHeight() / buttons);
            MenuButton b = _factory.GetButton(
                width,
                height,
                new Vector2(0, 0),
                "New Game",
                _screen.Camera);
            b.Click += (sender, args) => { _logger.Debug("Start new game"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.StartGame); };
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
                "Quit",
                _screen.Camera);
            b.Click += (sender, args) => { _logger.Debug("Quit game"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.QuitGame); };
            _buttons.Add(b);
            
            _logger.Error(_buttons.Count);
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_buttons == null)
            {
                return;
            }

            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend,
                SamplerState.PointClamp, // Sharp Pixel rendering
                DepthStencilState.DepthRead,
                RasterizerState.CullCounterClockwise, // Render only the texture side that faces the camara to boost performance 
                _screen.Camera.GetCameraEffect()
                );
            foreach (var button in _buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }
}