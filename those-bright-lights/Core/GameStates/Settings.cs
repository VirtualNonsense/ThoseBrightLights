using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using NLog;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Models;
using SE_Praktikum.Services.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE_Praktikum.Components;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Core.GameStates
{
    public class Settings : GameState
    {
        private Logger _logger;
        private readonly IScreen _screen;
        private readonly ControlElementFactory _factory;
        private ComponentGrid _components;


        public override void LoadContent(ContentManager contentManager)
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
            
            var s = _factory.GetSliderByDimension(contentManager, 20, 10, 30, width, Vector2.Zero, _screen.Camera);
            s.OnValueChanged += (sender, args) => { _logger.Debug($"{s.Value}"); };
            _components.Add(s);
            
            MenuButton b = _factory.GetMenuButtonByDimension(contentManager,
                width,
                height,
                new Vector2(0, 0),
                "Back to main menu",
                _screen.Camera);
            b.Click += (sender, args) => { _logger.Debug("Back to main menu"); _subject.OnNext(GameStateMachine.GameStateMachineTrigger.Back); };
            _components.Add(b);
        }

        public Settings(IScreen screen, ControlElementFactory factory)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = screen;
            _factory = factory;
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
        
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_components == null)
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
            foreach (var button in _components)
            {
                button.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
