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
        public List<Polygon> Polygons;
        public List<Player> InZoneList;

        

        public event EventHandler<ZoneEventArgs> OnZoneEntered;
        public event EventHandler<ZoneEventArgs> OnZoneLeft;
        private void InvokeOnZoneEntered(Player player)
        {
            OnZoneEntered?.Invoke(this, new ZoneEventArgs(player));
        }


        private void InvokeOnZoneLeft(Player player)
        {
            OnZoneLeft?.Invoke(this, new ZoneEventArgs(player));
        }
        public EventZone()
        {
            Polygons = new List<Polygon>();
            InZoneList = new List<Player>();
        }

        public void Update(Player player)
        {
            var i = Intersects(player);
            var o = InZoneList.Contains(player);
            if (!o && i)
            { 
                InZoneList.Add(player);
                InvokeOnZoneEntered(player);
            }
            if(!i && o)
            {
                InZoneList.Remove(player);
                InvokeOnZoneLeft(player);
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

    public class ZoneEventArgs : EventArgs
    {
        public Player Player;

        public ZoneEventArgs(Player player)
        {
            Player = player;
        }
    }
}
