using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SE_Praktikum.Components
{
    /// <summary>
    /// This is the Interface for everything that is drawable/updatable
    /// use this to implement anything that needs to be drawn / updated to simplify this process
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Position of the objects origin in world space
        /// </summary>
        Vector2 Position { get; set; }
        
        /// <summary>
        /// Draw the object
        /// </summary>
        /// <param name="spriteBatch">the "canvas the object will be drawn on"</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Updates the object
        /// e.g should the object be moved? etc.
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Is true when the item needs to be removed
        /// </summary>
        bool IsRemoveAble { get; set; }
    }
}