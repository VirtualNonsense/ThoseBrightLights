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
            _buttons = new List<Menubutton>();
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
            _logger.Debug("LoadingContent");
            var font = contentManager.Load<SpriteFont>("Font/Font2");
            var texture = contentManager.Load<Texture2D>("Artwork/Controls/button");
            _buttons.Add(new Menubutton(texture, font)
            {
                Text = "Level 1",
                Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 3f - texture.Height),
                PenColour = Color.White
            });
            _buttons.Last().Click += (sender, args) => { _logger.Debug("Level 1 selected"); };
            _buttons.Add(new Menubutton(texture, font)
            {
                Text = "Level 2",
                Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 3f),
                PenColour = Color.White
            });
            _buttons.Last().Click += (sender, args) => { _logger.Debug("Level 2 selected"); };
            _buttons.Add(new Menubutton(texture, font)
            {
                Text = "Level 3",
                Position = new Vector2(_screen.ScreenWidth / 2f, _screen.ScreenHeight / 3f + texture.Height),
                PenColour = Color.White
            });
            _buttons.Last().Click += (sender, args) => { _logger.Debug("Level 3 selected"); };
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
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
    }
}
