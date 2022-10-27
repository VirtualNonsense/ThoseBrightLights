using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThoseBrightLights.Components.HUD;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Services.Factories
{
    public class HUDFactory
    {
        // Fields
        private readonly HUDItemFactory hUDItemFactory;
        private readonly IScreen screen;

        // Constructor
        public HUDFactory(HUDItemFactory hUDItemFactory, IScreen screen)
        {
            this.hUDItemFactory = hUDItemFactory;
            this.screen = screen;
        }

        // Builds instance of a HUD
        public HUD GetInstance(Player player)
        {
            var hud = new HUD(player);
            
            var width = screen.Camera.GetPerspectiveScreenWidth(player.Layer);
            var height = screen.Camera.GetPerspectiveScreenHeight(player.Layer);

            var lifebar = hUDItemFactory.GetLifeBar(hud);
            lifebar.Position = new Vector2(-width / 2f, -height / 2f);
            hud.AddHUDItem(lifebar);

            var scorebar = hUDItemFactory.GetScoreBar(hud);
            scorebar.Position = new Vector2(width / 2f, -height / 2f);
            hud.AddHUDItem(scorebar);

            return hud;
        }
    }
}
