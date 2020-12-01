using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.NinjectModules
{
    public class FactoryModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<ParticleFactory>().ToSelf().InSingletonScope();
            Bind<AnimationHandlerFactory>().ToSelf().InSingletonScope();
            Bind<MapFactory>().ToSelf().InSingletonScope();
            Bind<TileFactory>().ToSelf().InSingletonScope();
            Bind<ControlElementFactory>().ToSelf().InSingletonScope();
        }
    }
}