using System;
using Microsoft.Xna.Framework;
using Ninject.Modules;
using SE_Praktikum.Core.GameStates;

namespace SE_Praktikum.NinjectModules
{
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Game>().To<SE_Praktikum_Game>().InSingletonScope();
            Bind<IObservable<GameState>>().To<GameStateMachine>();
        }
    }
}