using ThoseBrightLights.Services;
using ThoseBrightLights.Services.Factories;

namespace ThoseBrightLights.NinjectModules
{
    /// <summary>
    /// This module contains every factory
    /// </summary>
    public class FactoryModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            // binding factories in singleton scope
            // this way there will be only one instance
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
            Bind<PowerUpFactory>().ToSelf().InSingletonScope();
            Bind<BulletFactory>().ToSelf().InSingletonScope();
            Bind<HUDFactory>().ToSelf().InSingletonScope();
            Bind<HUDItemFactory>().ToSelf().InSingletonScope();
            Bind<LevelFactory>().ToSelf().InSingletonScope();
        }
    }
}