using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Services.Factories
{
    public class AnimationHandlerFactory
    {

        public AnimationHandlerFactory()
        {
        }
        /// <summary>
        /// Returns an instance of the animationshandler
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="settings"></param>
        /// <param name="position"></param>
        /// <param name="origin">sets the zero position within the frame. will set to frame center when null</param>
        /// <returns></returns>
        public AnimationHandler GetAnimationHandler(TileSet animation,
                                                    List<AnimationSettings> settings,
                                                    Vector2? position = null, 
                                                    Vector2? origin = null)
        {
            origin ??= new Vector2(animation.TileDimX / 2f, animation.TileDimY / 2f);
            return new AnimationHandler(animation, settings, position, origin);
        }
    }
}