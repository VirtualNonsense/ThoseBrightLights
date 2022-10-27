using System;
using System.Collections.Generic;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;

namespace ThoseBrightLights.Models
{
    /// <summary>
    /// Used to create an event zone
    /// </summary>
    public class EventZone
    {
        //fields
        public List<Polygon> Polygons;
        public List<Player> InZoneList;

        
        
        public event EventHandler<ZoneEventArgs> OnZoneEntered;
        public event EventHandler<ZoneEventArgs> OnZoneLeft;

        // Invoke when player enters zone
        private void InvokeOnZoneEntered(Player player)
        {
            OnZoneEntered?.Invoke(this, new ZoneEventArgs(player));
        }

        // Invoke when player leaves the zone
        private void InvokeOnZoneLeft(Player player)
        {
            OnZoneLeft?.Invoke(this, new ZoneEventArgs(player));
        }

        //Constructor
        public EventZone()
        {
            Polygons = new List<Polygon>();
            InZoneList = new List<Player>();
        }

        // Always update player actions 
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

        // Player hitbox overlaps with zone hitbox
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
