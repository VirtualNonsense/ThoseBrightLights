using Microsoft.Xna.Framework;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class AnimationHandlerFactory
    {

        public AnimationHandlerFactory()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="settings"></param>
        /// <param name="position"></param>
        /// <param name="origin">sets the zero position within the frame. will set to frame center when null</param>
        /// <returns></returns>
        public AnimationHandler GetAnimationHandler(TileSet animation,
                                                    AnimationSettings settings,
                                                    Vector2? position = null, 
                                                    Vector2? origin = null)
        {
            origin ??= new Vector2(animation.FrameWidth / 2f, animation.FrameHeight / 2f);
            return new AnimationHandler(animation, settings, position, origin);
        }
    }
}