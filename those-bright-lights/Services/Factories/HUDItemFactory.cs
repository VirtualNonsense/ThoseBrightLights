using SE_Praktikum.Components.HUD;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Services.Factories
{
    public class HUDItemFactory
    {
        private readonly TileSetFactory tileSetFactory;
        private readonly AnimationHandlerFactory animationHandlerFactory;

        public HUDItemFactory(TileSetFactory tileSetFactory, AnimationHandlerFactory animationHandlerFactory)
        {
            this.tileSetFactory = tileSetFactory;
            this.animationHandlerFactory = animationHandlerFactory;
        }



    }
}
