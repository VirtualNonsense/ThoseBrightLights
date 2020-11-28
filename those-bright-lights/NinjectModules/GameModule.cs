using System;
using Microsoft.Xna.Framework.Content;
using Ninject;
using Ninject.Modules;
using SE_Praktikum.Core;
using SE_Praktikum.Core.GameStates;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.ParticleEmitter;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.NinjectModules
{
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<SE_Praktikum_Game>().ToSelf().InSingletonScope();
            Bind<IScreen>().ToMethod(c => c.Kernel.Get<SE_Praktikum_Game>());
            Bind<ContentManager>().ToMethod(c => c.Kernel.Get<SE_Praktikum_Game>().Content);
            Bind<IObservable<GameState>>().To<GameStateMachine>();
            Bind<ExplosionEmitter>().ToSelf();
            Bind<Level>().ToSelf();

        }
    }
}