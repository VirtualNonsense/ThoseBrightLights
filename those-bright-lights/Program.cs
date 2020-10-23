using System;
using Microsoft.Xna.Framework;
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
            var kernel = new KernelConfiguration(new GameModule()).BuildReadonlyKernel();
            
            using var game = kernel.Get<Game>();
            game.Run();
        }
    }
}