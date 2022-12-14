using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using ThoseBrightLights.Components.Sprites.Actors;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Components
{
    public class Map
    {
        //fields
        private Dictionary<float, QuadTree<Tile>> _tileContainer;

        public Rectangle Area { get; }
        
        public float TopLayer => _tileContainer.Keys.Max();

        public Polygon PlayerSpawnPoint;

        public List<(EnemyType,Vector2)> EnemySpawnpoints;

        public List<(PowerUpType, Vector2)> PowerUpSpawnpoints;

        public EventZone WinningZone { get; set; }

        //Constructor
        public Map (Dictionary<float, QuadTree<Tile>> tiles, Rectangle area, EventZone winningZone, List<(EnemyType, Vector2)> enemySpawnPoints, List<(PowerUpType, Vector2)> powerUpSpawnpoints)
        {
            _tileContainer = tiles;
            Area = area;
            WinningZone = winningZone;
            EnemySpawnpoints = enemySpawnPoints;
            PowerUpSpawnpoints = powerUpSpawnpoints;
            if (winningZone == null) return;
            foreach(var p in winningZone.Polygons)
            {
                p.Layer = TopLayer;
            }
        }
        
        // Get list of tiles
        public List<Tile> RetrieveItems(float layer, Rectangle rect)
        {
            if (_tileContainer.ContainsKey(layer))
            {
                return _tileContainer[layer].Retrieve(rect);
            }

            return new List<Tile>();
        }

        public List<Tile> RetrieveItems(Rectangle rect)
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
            WinningZone?.Update(player);
        }


    }

    // Enumerations for map placing via tiled
    public enum EnemyType
    {
        Turret, 
        Alienship,
        Boss,
        Mines,
        Kamikaze
    }
    public enum PowerUpType
    {
        HealthPowerUp,
        FullHealthPowerUp,
        InfAmmoPowerUp,
        BonusClipPowerUp,
        InstaDeathPowerUp,
        ScoreBonusPowerUp,
        StarPowerUp,
        WeaponPowerUp
    }
    
}
