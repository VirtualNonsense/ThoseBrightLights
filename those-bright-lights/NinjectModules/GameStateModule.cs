using SE_Praktikum.Core.GameStates;

namespace SE_Praktikum.NinjectModules
{
    public class GameStateModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<Splashscreen>().ToSelf().InSingletonScope();
            Bind<MainMenu>().ToSelf().InSingletonScope();
            Bind<Settings>().ToSelf().InSingletonScope();
            Bind<LevelSelect>().ToSelf().InSingletonScope();
        }
    }
}