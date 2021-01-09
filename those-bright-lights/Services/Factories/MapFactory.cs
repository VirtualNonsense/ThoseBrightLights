using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using Microsoft.Xna.Framework;
using SE_Praktikum.Components.Sprites.Actors;


namespace SE_Praktikum.Services.Factories
{
    public class MapFactory
    {
        private readonly ILogger _logger;
        private readonly TileSetFactory _setFactory;
        private readonly TileFactory tileFactory;
        private readonly ContentManager _contentManager;

        public MapFactory(TileSetFactory _setFactory, TileFactory tileFactory, ContentManager contentManager)
        {
            _logger = LogManager.GetCurrentClassLogger();

            this._setFactory = _setFactory;
            this.tileFactory = tileFactory;
            _contentManager = contentManager;

        }

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
                    var tileSetPath = Path.GetFullPath(
                        @"..\/"+tileSet.Source, //first part is necessary to get rid of the file in absPath
                        absPath);
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
                    _logger.Error(e);
                    throw;
                }
            }
            Polygon SpawnPoint = null;
            EventZone WinningZone = null;
            Dictionary<float, QuadTree<Tile>> tiles = new Dictionary<float, QuadTree<Tile>>();
            List<(EnemyType, Vector2)> EnemySpawnpoints = new List<(EnemyType, Vector2)>();
            var l = 0f;
            var area = new Rectangle(0, 0, blueprint.Width * blueprint.TileWidth, blueprint.Height * blueprint.TileHeight);
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
                    foreach(var obj in layer.objects)
                    {

                        switch(obj.type)
                        {
                            case "PlayerSpawnPoint":
                                SpawnPoint = ConvertObjectToPolygon(obj);
                                break;
                            case "WinningZone":
                                if (WinningZone == null)
                                    WinningZone = new EventZone();
                                WinningZone.Polygons.Add(ConvertObjectToPolygon(obj));
                                break;
                            case "TurretSpawn":
                                EnemySpawnpoints.Add((EnemyType.Turret, ConvertObjectToPolygon(obj).Center));
                                break;

                            default:
                                _logger.Warn($"{obj.type} not found");
                                break;
                                

                        }
                    }
                }

                
            }
            return new Map(tiles, area, winningZone: WinningZone, EnemySpawnpoints) { PlayerSpawnPoint = SpawnPoint};
        }
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
                            new Vector2(-objectt.width/2,-objectt.height/2),
                            new Vector2(objectt.width/2,-objectt.height/2),
                            new Vector2(objectt.width/2,objectt.height/2),
                            new Vector2(-objectt.width/2,objectt.height/2)
                }
                );
            }
            return polygon;
        }

        
    }
}