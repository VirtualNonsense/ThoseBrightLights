using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Ninject.Modules;
using ThoseBrightLights.Services.ParticleEmitter;
using ThoseBrightLights.Core;
using ThoseBrightLights.Core.GameStates;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;
using ThoseBrightLights.Services.StateMachines;

namespace ThoseBrightLights.NinjectModules
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