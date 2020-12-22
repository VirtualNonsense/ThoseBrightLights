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
            Bind<PlayerFactory>().ToSelf().InSingletonScope();
            Bind<InputFactory>().ToSelf().InSingletonScope();
            Bind<WeaponFactory>().ToSelf().InSingletonScope();
            Bind<EnemyFactory>().ToSelf().InSingletonScope();
            Bind<ControlElementFactory>().ToSelf().InSingletonScope();
            Bind<TileSetFactory>().ToSelf().InSingletonScope();
        }
    }
}