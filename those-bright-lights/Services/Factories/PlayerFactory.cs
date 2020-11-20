using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class PlayerFactory
    {
        private Logger _logger;
        private AnimationHandlerFactory _animationHandlerFactory;
        private TileFactory _tileFactory;
        private int _health = 100;
        private float _speed = 1;
        

        public PlayerFactory(AnimationHandlerFactory animationHandlerFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
        }

        public Player GetInstance(ContentManager contentManager, Input input = null)
        {
            Input i = input ?? new Input(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Q, Keys.E);
            Player p = new Player(
                _animationHandlerFactory.GetAnimationHandler(
                    new TileSet(contentManager.Load<Texture2D>("Artwork/actors/spaceship")),
                    new AnimationSettings(1)),
                i, _health, _speed);
            return p;
        }
    }
}