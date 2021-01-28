using Ninject.Modules;
using SE_Praktikum.Core;
using SE_Praktikum.Core.GameStates;

namespace SE_Praktikum.NinjectModules
{
    /// <summary>
    /// this module will inject every game state
    /// </summary>
    public class GameStateModule : NinjectModule
    {
        public override void Load()
        {
            // entry state - shows logos and stuff
            Bind<Splashscreen>().ToSelf().InSingletonScope();
            Bind<MainMenu>().ToSelf().InSingletonScope();
            Bind<Settings>().ToSelf().InSingletonScope();
            Bind<LevelSelect, ILevelContainer>().To<LevelSelect>().InSingletonScope();
            Bind<InGame>().ToSelf().InSingletonScope();
            Bind<SaveSelect>().ToSelf().InSingletonScope();
        }
    }
}