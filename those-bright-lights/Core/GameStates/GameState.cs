using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SE_Praktikum.Core.GameStates
{
    public abstract class GameState
    {
        public GameStateMachine Machine;

        public abstract void LoadContent(SpriteBatch batch);
        
        /// <summary>
        /// Updates every component within state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="game"></param>
        /// <param name="manager"></param>
        /// <param name="graphicsDevice"></param>
        public abstract void Update(GameTime gameTime, Game game, ContentManager manager, GraphicsDevice graphicsDevice);

        /// <summary>
        /// For cleanup;
        /// </summary>
        /// <param name="game"></param>
        /// <param name="manager"></param>
        /// <param name="graphicsDevice"></param>
        public abstract void PostUpdate(Game game, ContentManager manager, GraphicsDevice graphicsDevice);

        /// <summary>
        /// Draw everything within specific state;
        /// </summary>
        /// <param name="game"></param>
        /// <param name="manager"></param>
        /// <param name="graphicsDevice"></param>
        public abstract void Draw(Game game, ContentManager manager, GraphicsDevice graphicsDevice);
    }
}