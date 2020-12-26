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
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using Microsoft.Xna.Framework;

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
            Dictionary<float, QuadTree<Tile>> tiles = new Dictionary<float, QuadTree<Tile>>();
            var l = 0f;
            var area = new Rectangle(0, 0, blueprint.Width * blueprint.TileWidth, blueprint.Height * blueprint.TileHeight);
            foreach(var layer in blueprint.Layers)
            {
                var c = tileFactory.GenerateTiles(layer.Data, 
                                                          l, 
                                                          tileSets,
                                                          blueprint.TileWidth,
                                                          blueprint.TileHeight,
                                                          blueprint.Height,
                                                          blueprint.Width,
                                                          layer.Visible? 1 : 0,
                                                          area);
                tiles.Add(l,c);
                l+=3;
            }
            return new Map(tiles, area);
        }
    }
}