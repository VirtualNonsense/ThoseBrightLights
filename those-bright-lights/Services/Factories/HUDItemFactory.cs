using SE_Praktikum.Components.HUD;
using SE_Praktikum.Models;
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

        public HUDItem GetLifeBar(HUD hUD)
        {
            var tileSet = tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\heart_11_17.json",0);
            var animationSettingsLeftHeart = new AnimationSettings(new List<(int, float)> { 
                (0,0),
                (2,0)
            }, isPlaying : false);

            var animationSettingsRightHeart = new AnimationSettings(new List<(int, float)> {
                (1,0),
                (3,0)
            }, isPlaying: false);
            return new LifeBar(hUD,animationHandlerFactory,tileSet,animationSettingsLeftHeart,animationSettingsRightHeart);
        }
    }
}
