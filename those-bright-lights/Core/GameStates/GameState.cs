using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SE_Praktikum.Core.GameStates
{
    public abstract class GameState
    {
        public event EventHandler OnStateComplete;

        protected GameState()
        {
        }

        public abstract void LoadContent(ContentManager contentManager);

        public abstract void UnloadContent();
        
        /// <summary>
        /// Updates every component within state
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// For cleanup;
        /// </summary>
        public abstract void PostUpdate(GameTime gameTime);

        /// <summary>
        /// Draw everything within specific state;
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        protected virtual void OnStateCompleted(EventArgs e)
        {
            EventHandler handler = OnStateComplete;
            handler?.Invoke(this, e);
        }
    }
}