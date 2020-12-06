using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class LevelSelect : GameState
    {
        private readonly IScreen _screen;
        private List<Menubutton> _buttons;
        private Logger _logger;

        public LevelSelect(IScreen screen)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = screen;
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

        public override void LoadContent(ContentManager contentManager)
        {
            if (_buttons != null) return;
            // {
            //     Text = "Level 1",
            //     Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 4f - texture.Height),
            //     TextColor = Color.White
            // });
            // _buttons.Last().Click += (sender, args) => { _logger.Debug("Level 1 selected"); };
            // _buttons.Add(new Menubutton(texture, font)
            // {
            //     Text = "Level 2",
            //     Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 4f),
            //     TextColor = Color.White
            // });
            // _buttons.Last().Click += (sender, args) => { _logger.Debug("Level 2 selected"); };
            // _buttons.Add(new Menubutton(texture, font)
            // {
            //     Text = "Level 3",
            //     Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 4f + texture.Height),
            //     TextColor = Color.White
            // });
            // _buttons.Last().Click += (sender, args) => { _logger.Debug("Level 3 selected"); };
            // _buttons.Add(new Menubutton(texture, font)
            // {
            //     Text = "Back",
            //     Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 4f + 2*texture.Height),
            //     TextColor = Color.White
            // });
            // _buttons.Last().Click += (sender, args) => { _logger.Debug("back"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back);};
            _buttons = new List<Menubutton>();
            _logger.Debug("LoadingContent");
            var font = contentManager.Load<SpriteFont>("Font/Font2");
            var texture = contentManager.Load<Texture2D>("Artwork/Controls/button");
            // _buttons.Add(new Menubutton(texture, font)
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
