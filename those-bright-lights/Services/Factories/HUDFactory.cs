using Microsoft.Xna.Framework;
using SE_Praktikum.Components.HUD;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Services.Factories
{
    public class HUDFactory
    {
        private readonly HUDItemFactory hUDItemFactory;
        private readonly IScreen screen;

        public HUDFactory(HUDItemFactory hUDItemFactory, IScreen screen)
        {
            this.hUDItemFactory = hUDItemFactory;
            this.screen = screen;
        }

        public HUD GetInstance(Player player)
        {
            var hud = new HUD(player);
            var lifebar = hUDItemFactory.GetLifeBar(hud);
            var width = screen.Camera.GetPerspectiveScreenWidth(player.Layer);
            var height = screen.Camera.GetPerspectiveScreenHeight(player.Layer);
           

            lifebar.Position = new Vector2(-width / 2f, -height / 2f);

            hud.AddHUDItem(lifebar);

            return hud;
        }
    }
}
