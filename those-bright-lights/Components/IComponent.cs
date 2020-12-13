using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SE_Praktikum.Components
{
    /// <summary>
    /// Derive from this class if you 
    /// </summary>
    public interface IComponent
    {
        Vector2 Position { get; set; }
        
        void Draw(SpriteBatch spriteBatch);

        void Update(GameTime gameTime);

        bool IsRemoveAble { get; set; }
    }
}