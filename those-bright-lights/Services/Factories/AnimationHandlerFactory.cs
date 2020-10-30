using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class AnimationHandlerFactory
    {

        public AnimationHandlerFactory()
        {
        }
        
        public AnimationHandler GetAnimationHandler(Animation animation, AnimationSettings settings)
        {
            return new AnimationHandler(animation, settings);
        }
    }
}