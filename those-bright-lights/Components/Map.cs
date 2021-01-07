using SE_Praktikum.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Components.Sprites;
using System.Data;
using System.Linq;
using SE_Praktikum.Models.Tiled;

namespace SE_Praktikum.Components
{
    public class Map
    {
        private Dictionary<float, QuadTree<Tile>> _tileContainer;

        public Rectangle Area { get; }
        
        public float TopLayer => _tileContainer.Keys.Max();

        public Polygon PlayerSpawnPoint;

        public EventZone WinningZone { get; set; }

        public Map (Dictionary<float, QuadTree<Tile>> tiles, Rectangle area, EventZone winningZone)
        {
            _tileContainer = tiles;
            Area = area;
            WinningZone = winningZone;
        }
      
        public List<Tile> GetCollidable(float layer, Rectangle rect)
        {
            if (_tileContainer.ContainsKey(layer))
            {
                return _tileContainer[layer].Retrieve(rect);
            }

            return new List<Tile>();
        }

        public List<Tile> GetCollidable(Rectangle rect)
        {
            var list = new List<Tile>();

            foreach (var item in _tileContainer)
            {
                list.AddRange(item.Value.Retrieve(rect));
            }

            return list;
        }

        public void ZoneUpdate(Player player)
        {
            WinningZone.Update(player);
        }


    }
    //enum ObjectLayerType
    //{
    //    PlayerSpawn,
    //    EnemySpawn,
    //    VictoryZone,

    //}
    
}
