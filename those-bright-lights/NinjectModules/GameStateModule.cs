using SE_Praktikum.Core.GameStates;

namespace SE_Praktikum.NinjectModules
{
    public class GameStateModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<Splashscreen>().ToSelf().InSingletonScope();
            Bind<Testscreen>().ToSelf().InSingletonScope();
        }
    }
}