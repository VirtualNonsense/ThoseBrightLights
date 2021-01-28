using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    /// <summary>
    /// this module injects necessary but misc stuff
    /// </summary>
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            // bind game instance to each layer of access 
            Bind<ThoseBrightLights, IScreen, IGameEngine, ISaveGameHandler>().To<ThoseBrightLights>().InSingletonScope();
            
            // MonoGameClass  necessary for loading stuff
            Bind<ContentManager>().ToMethod(c => c.Kernel.Get<ThoseBrightLights>().Content);
            
            // handles every state transition on a macro level
            Bind<IObservable<GameState>>().To<GameStateMachine>();
            
            // injects class that's responsible for save file handling
            Bind<SaveHandler>().ToSelf().InSingletonScope();

        }
    }
}