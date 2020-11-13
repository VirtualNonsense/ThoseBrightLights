using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using NLog.Fluent;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class MainMenu : GameState
    {
        private readonly IScreen _screen;
        private List<Menubutton> _buttons;
        private Logger _logger;

        public MainMenu(IScreen screen)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = screen;
            _buttons = new List<Menubutton>();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _logger.Debug("LoadingContent");
            var font = contentManager.Load<SpriteFont>("Font/Font2");
            var texture = contentManager.Load<Texture2D>("Artwork/Controls/button");
            _buttons.Add(new Menubutton(texture, font)
            {
                Text = "New Game",
                Position = new Vector2(_screen.ScreenWidth/2f, _screen.ScreenHeight/3f-texture.Height),
                PenColour = Color.White
            });
            _buttons.Last().Click += (sender, args) => { _logger.Debug("new Game"); };
            _buttons.Add(new Menubutton(texture, font)
            {
                Text = "Settings",
                Position = new Vector2(_screen.ScreenWidth/2f, _screen.ScreenHeight/3f),
                PenColour = Color.White
            });
            _buttons.Last().Click += (sender, args) => { _logger.Debug("Settings"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.StartSettings);};
            _buttons.Add(new Menubutton(texture, font)
            {
                Text = "Quit",
                Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 3f + texture.Height),
                PenColour = Color.White
            });
            _buttons.Last().Click += (sender, args) => { _logger.Debug("Quit"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.QuitGame); };
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
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
            spriteBatch.Begin();
            foreach (var button in _buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }
}