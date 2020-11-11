using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SE_Praktikum.Models
{
    public class AnimationSettings
    {
        public bool IsLooping;
        
        /// <summary>
        /// Determines how long it takes to update the frame. [ms] 
        /// </summary>
        public float UpdateInterval;

        public float Layer;

        public SpriteEffects SpriteEffects;

        public float Rotation;

        public float Scale;

        public Color Color;

        public float Opacity;

        public bool IsPlaying;

        
        public AnimationSettings(float layer = 1, bool isLooping = false, float updateInterval = 1f, Color? color = null, 
                                 float rotation = 0f, float scale = 1f, float opacity = 1, bool isPlaying = true)
        {
            Layer = layer;
            IsLooping = isLooping;
            UpdateInterval = updateInterval;
            SpriteEffects = SpriteEffects.None;
            Color = color??Color.White;
            Rotation = rotation;
            Scale = scale;
            Opacity = opacity;
            IsPlaying = isPlaying;
        }
    }
}