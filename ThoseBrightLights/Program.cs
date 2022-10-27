using System;
using Ninject;
using ThoseBrightLights.Core.GameStates;
using ThoseBrightLights.NinjectModules;

namespace ThoseBrightLights
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var kernel = new StandardKernel(
                new GameModule(),
                new FactoryModule(),
                new GameStateModule()
            );
            // // get injection kernel and register modules
            // var kernel = new KernelConfiguration(
            //     new GameModule(),
            //     new FactoryModule(),
            //     new GameStateModule()
            // ).BuildReadonlyKernel();

            // request game instance
            using var game = kernel.Get<ThoseBrightLights>();

            // Inject state machine over property to avoid ring injection
            // (unfortunate solution but there is no other way :/)
            game.StatePublisherTicket = kernel.Get<IObservable<GameState>>().Subscribe(game);

            // start the game
            game.Run();
        }
    }
}