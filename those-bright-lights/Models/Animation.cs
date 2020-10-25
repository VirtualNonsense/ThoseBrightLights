using Microsoft.Xna.Framework.Graphics;

namespace SE_Praktikum.Models
{
    public class Animation
    {
        public int FrameCount { get; private set; }

        public int FrameHeight => Texture.Height;

        public int FrameWidth => Texture.Width / FrameCount;
        public Texture2D Texture { get; private set; }
        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;

            FrameCount = frameCount;

        }

    }
}