using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;

namespace SE_Praktikum.Services.Factories
{
    public class MapFactory
    {
        private readonly ILogger _logger;
        private readonly TileFactory tileFactory;
        private Regex _regex;

        public MapFactory(TileFactory tileFactory)
        {
            _logger = LogManager.GetCurrentClassLogger();
            
            this.tileFactory = tileFactory;
        }

        public Map LoadMap(ContentManager contentManager, LevelBlueprint blueprint)
        {
            // Loading all necessary tile sets
            List<TileSet> tileSets = new List<TileSet>();
            foreach (var tileSet in blueprint.tilesets)
            {
                var title = tileSet.Source.Split(".")[0];
                try
                {
                    tileSets.Add(new TileSet(contentManager.Load<Texture2D>($"Artwork/Tilemaps/{title}"), blueprint.Width, blueprint.Height, tileSet.FirstGId));
                }
                catch (ContentLoadException e)
                {
                    _logger.Warn($"Texture {title} is missing: ", e);
                    var lastFirstGId = tileSets.Count > 0 ? tileSets.Last().StartEntry + tileSets.Last().Tiles - 1 : 0;
                    tileSets.Add(new TileSet(contentManager.Load<Texture2D>($"Artwork/missing_texture"), blueprint.Width, blueprint.Height, lastFirstGId + 1 ));
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    throw;
                }
            }
            List<Tile> tiles = new List<Tile>();
            var t = 0f;
            foreach(var layer in blueprint.Layers)
            {
                var c = tileFactory.GenerateTiles(layer.Data, t, tileSets, blueprint.TileWidth, blueprint.TileHeight, blueprint.Height, blueprint.Width);
                tiles.AddRange(c);

            }
            return new Map(tiles);
        }
    }
}