using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SE_Praktikum.Extensions;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;

namespace SE_Praktikum.Models
{
    public class EventZone
    {
        public Polygon[] Polygons { get; }
        public List<Player> InZoneList;

        

        public event EventHandler OnZoneEntered;
        public event EventHandler OnZoneLeft;
        private void InvokeOnZoneEntered()
        {
            OnZoneEntered?.Invoke(this, EventArgs.Empty);
        }

        private void InvokeOnZoneLeft()
        {
            OnZoneLeft?.Invoke(this, EventArgs.Empty);
        }
        public EventZone(Polygon[] polygons)
        {
            Polygons = polygons;
            InZoneList = new List<Player>();
        }

        public void Update(Player player)
        {
            var i = Intersects(player);
            var o = InZoneList.Contains(player);
            if (!o && i)
            { 
                InZoneList.Add(player);
                InvokeOnZoneEntered();
            }
            if(!i && o)
            {
                InZoneList.Remove(player);
                InvokeOnZoneLeft();
            }
        }

        private bool Intersects(Player player)
        {
            foreach(var p in Polygons)
            {
                foreach (var p2 in player.HitBox)
                {
                    if (p.Overlap(p2))
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }
}
