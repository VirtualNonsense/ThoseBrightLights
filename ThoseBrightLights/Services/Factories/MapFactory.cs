using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using NLog;
using Microsoft.Xna.Framework;
using ThoseBrightLights.Components;
using ThoseBrightLights.Components.Sprites.Actors;
using ThoseBrightLights.Models;
using ThoseBrightLights.Models.Tiled;


namespace ThoseBrightLights.Services.Factories
{
    public class MapFactory
    {
        // Fields 
        private readonly ILogger _logger;
        private readonly TileSetFactory _setFactory;
        private readonly TileFactory tileFactory;
        private readonly ContentManager _contentManager;

        // Constructor
        public MapFactory(TileSetFactory _setFactory, TileFactory tileFactory, ContentManager contentManager)
        {
            _logger = LogManager.GetCurrentClassLogger();

            this._setFactory = _setFactory;
            this.tileFactory = tileFactory;
            _contentManager = contentManager;

        }
        // Method to read json files
        public Map LoadMap(string path)
        {
            var absPath = Path.GetFullPath(path);
            LevelBlueprint blueprint = JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(absPath));

            // Loading all necessary tile sets
            List<TileSet> tileSets = new List<TileSet>();
            foreach (var tileSet in blueprint.tilesets)
            {
                var array = tileSet.Source.Split(".json")[0].Split("/");
                var title = array[array.Count() - 1];

                try
                {
                    var dirPath = Path.GetFullPath("..", absPath);
                    
                    var tileSetPath = Path.Join(dirPath, tileSet.Source);
                    tileSets.Add(_setFactory.GetInstance(tileSetPath, tileSet.FirstGId));
                }
                catch (ContentLoadException e)
                {
                    _logger.Warn($"Texture {title} is missing: {e}");
                    var lastFirstGId = tileSets.Count > 0 ? tileSets.Last().StartEntry + tileSets.Last().Tiles - 1 : 0;
                    tileSets.Add(new TileSet(_contentManager.Load<Texture2D>($"Artwork/missing_texture"), blueprint.TileWidth, blueprint.TileHeight, null, lastFirstGId + 1 ));
                }
                catch (Exception e)
                {
                    _logger.Error($"failed to load map at: {path}. Full log:\n{e}");
                    throw;
                }
            }
            Polygon SpawnPoint = null;
            EventZone WinningZone = null;
            Dictionary<float, QuadTree<Tile>> tiles = new Dictionary<float, QuadTree<Tile>>();
            List<(EnemyType, Vector2)> EnemySpawnpoints = new List<(EnemyType, Vector2)>();
            List<(PowerUpType, Vector2)> PowerUpSpawnpoints = new List<(PowerUpType, Vector2)>();
            var l = 0f;
            var area = new Rectangle(0, 0, blueprint.Width * blueprint.TileWidth, blueprint.Height * blueprint.TileHeight);

            // Iterate over all layers used in map
            foreach(var layer in blueprint.Layers)
            {
                if (layer.Data != null)
                {
                    var c = tileFactory.GenerateTiles(layer.Data,
                                                              l,
                                                              tileSets,
                                                              blueprint.TileWidth,
                                                              blueprint.TileHeight,
                                                              blueprint.Height,
                                                              blueprint.Width,
                                                              layer.Visible ? 1 : 0,
                                                              area);
                    
                    tiles.Add(l, c);
                    l += 3;
                    continue;
                }
                if(layer.objects != null)
                {
                    // Iterate over all objects on each layer
                    foreach(var obj in layer.objects)
                    {
                        // Use those case names in tiled in the type field to let them spawn via a point in object layers
                        switch(obj.type)
                        {
                            case "PlayerSpawnPoint":
                                SpawnPoint = ConvertObjectToPolygon(obj);
                                break;
                            case "WinningZone":
                                if (WinningZone == null)
                                    WinningZone = new EventZone();
                                var zone = ConvertObjectToPolygon(obj);
                                zone.DrawAble = true;
                                zone.Color = Color.LawnGreen;
                                WinningZone.Polygons.Add(zone);
                                break;
                            case "TurretSpawn":
                                EnemySpawnpoints.Add((EnemyType.Turret, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "AlienSpawn":
                                EnemySpawnpoints.Add((EnemyType.Alienship, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "BossSpawn":
                                EnemySpawnpoints.Add((EnemyType.Boss, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "KamikazeSpawn":
                                EnemySpawnpoints.Add((EnemyType.Kamikaze, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "MineSpawn":
                                EnemySpawnpoints.Add((EnemyType.Mines, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "HealthPowerUpSpawn":
                                PowerUpSpawnpoints.Add((PowerUpType.HealthPowerUp, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "FullHealthPowerUpSpawn":
                                PowerUpSpawnpoints.Add((PowerUpType.FullHealthPowerUp, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "AmmoPowerUpSpawn":
                                PowerUpSpawnpoints.Add((PowerUpType.InfAmmoPowerUp, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "InstaDeathPowerUpSpawn":
                                PowerUpSpawnpoints.Add((PowerUpType.InstaDeathPowerUp, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "ScoreBonusPowerUp":
                                PowerUpSpawnpoints.Add((PowerUpType.ScoreBonusPowerUp, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "StarPowerUpSpawn":
                                PowerUpSpawnpoints.Add((PowerUpType.StarPowerUp, ConvertObjectToPolygon(obj).Center));
                                break;
                            case "WeaponPowerUpSpawn":
                                PowerUpSpawnpoints.Add((PowerUpType.WeaponPowerUp, ConvertObjectToPolygon(obj).Center));
                                break;
                            default:
                                _logger.Warn($"{obj.type} not found");
                                break;
                                

                        }
                    }
                }

                
            }
            return new Map(tiles, area, winningZone: WinningZone, EnemySpawnpoints, PowerUpSpawnpoints) { PlayerSpawnPoint = SpawnPoint};
        }

        // Convert objects from tiled into drawable polygons (with hitbox)
        private Polygon ConvertObjectToPolygon(ObjectBluePrint objectt)
        {
            Polygon polygon;
            if (objectt.polygon != null)
            {
                List<Vector2> Vector2List = new List<Vector2>();
                foreach (var p in objectt.polygon)
                {
                    Vector2List.Add(new Vector2(p.x, p.y));
                }
                polygon = new Polygon(new Vector2(objectt.x,objectt.y), Vector2.Zero, 0, Vector2List);
            }
            else
            {
                polygon = new Polygon(
                new Vector2(objectt.x, objectt.y),
                new Vector2(0,0),
                0,
                new List<Vector2>
                {
                            new Vector2(0,0),
                            new Vector2(objectt.width,0),
                            new Vector2(objectt.width,objectt.height),
                            new Vector2(0,objectt.height)
                }
                );
            }
            return polygon;
        }

        
    }
}