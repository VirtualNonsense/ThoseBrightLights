using Microsoft.Xna.Framework;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class AnimationHandlerFactory
    {

        public AnimationHandlerFactory()
        {
        }
        
        public AnimationHandler GetAnimationHandler(TileSet animation,
                                                    AnimationSettings settings,
                                                    Vector2? position = null, 
                                                    Vector2? origin = null)
        {
            return new AnimationHandler(animation, settings, position, origin);
        }
    }
}