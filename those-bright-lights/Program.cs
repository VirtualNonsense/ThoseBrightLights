using System;
using Ninject;
using SE_Praktikum.Core.GameStates;
using SE_Praktikum.NinjectModules;

namespace SE_Praktikum
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var kernel = new KernelConfiguration(
                new GameModule(),
                new FactoryModule(),
                new GameStateModule()
            ).BuildReadonlyKernel();
            using var game = kernel.Get<ThoseBrightLights>();
            // Stupid but the only way i found.....
            game.StatePublisherTicket = kernel.Get<IObservable<GameState>>().Subscribe(game);
            game.Run();
        }
    }
}