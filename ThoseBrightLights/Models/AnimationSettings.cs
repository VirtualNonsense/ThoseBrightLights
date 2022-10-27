using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ThoseBrightLights.Models
{
    public class AnimationSettings
    {
        public bool IsLooping;
        
        /// <summary>
        /// This List contains the information about which frame (Item1) should be shown for which amount of time (item2)
        /// the list will be "played" from front to back
        /// </summary>
        public List<(int,float)> UpdateList;
        
        /// <summary>
        /// determines the distance from the back layer (at 0)
        /// </summary>
        public float Layer;

        /// <summary>
        /// May be set to flip the graphic either horizontally or vertically
        /// </summary>
        public SpriteEffects SpriteEffects;

        // maybe it's more reasonable to move this into the class wielding the AnimationHandler
        // this way it's a bit hidden
        public float Rotation;
        
        // maybe it's more reasonable to move this into the class wielding the AnimationHandler
        // this way it's a bit hidden
        public float Scale;

        /// <summary>
        /// tinting color
        /// due to rendering issues currently not working
        /// </summary>
        public Color Color;

        /// <summary>
        /// Determines the visibility of the current animation
        /// 0 -> invisible
        /// 1 -> visible
        /// due to rendering issues currently not working 
        /// </summary>
        public float Opacity;

        /// <summary>
        /// set true to play the animation
        /// </summary>
        public bool IsPlaying;

        
        // #############################################################################################################
        // constructor
        // #############################################################################################################
        /// <summary>
        /// Settings for animationhandler 
        /// </summary>
        /// <param name="updateList">fully configured update list</param>
        /// <param name="layer"></param>
        /// <param name="isLooping"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="opacity"></param>
        /// <param name="isPlaying"></param>
        public AnimationSettings(List<(int, float)> updateList = null, float layer = 1, bool isLooping = false,  Color? color = null, 
                                 float rotation = 0f, float scale = 1f, float opacity = 1, bool isPlaying = true)
        {
            Layer = layer;
            IsLooping = isLooping;
            UpdateList = updateList ?? new List<(int,float)> {(0,100)};
            SpriteEffects = SpriteEffects.None;
            Color = color??Color.White;
            Rotation = rotation;
            Scale = scale;
            Opacity = opacity;
            IsPlaying = isPlaying;
        }
        
        /// <summary>
        /// Settings for AnimationHandler
        /// (Will generate update list for you, but keep in mind that it will start at frame 0!)
        /// </summary>
        /// <param name="frames">amount of frames</param>
        /// <param name="duration">duration PER FRAME</param>
        /// <param name="layer"></param>
        /// <param name="isLooping"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="opacity"></param>
        /// <param name="isPlaying"></param>
        /// <param name="spriteEffects"></param>
        public AnimationSettings(int frames = 1, float duration = 100f, float layer = 1, bool isLooping = false,  Color? color = null, 
                                 float rotation = 0f, float scale = 1f, float opacity = 1, bool isPlaying = true, SpriteEffects? spriteEffects = null)
        {
            Layer = layer;
            IsLooping = isLooping;
            UpdateList = new List<(int, float)>();
            for (var i = 0; i < frames; i++)
            {
                UpdateList.Add((i,duration));
            }     
            SpriteEffects = spriteEffects??SpriteEffects.None;
            Color = color??Color.White;
            Rotation = rotation;
            Scale = scale;
            Opacity = opacity;
            IsPlaying = isPlaying;
        }
    }
}