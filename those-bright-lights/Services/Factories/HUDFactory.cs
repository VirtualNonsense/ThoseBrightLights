using SE_Praktikum.Components.HUD;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Services.Factories
{
    public class HUDFactory
    {
        private readonly HUDItemFactory hUDItemFactory;

        public HUDFactory(HUDItemFactory hUDItemFactory)
        {
            this.hUDItemFactory = hUDItemFactory;
        }

        public HUD GetInstance(Player player)
        {
            var hud = new HUD(player);

            hud.AddHUDItem(hUDItemFactory.GetLifeBar(hud));

            return hud;
        }
    }
}
