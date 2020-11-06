using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;

namespace SE_Praktikum.Services.Factories
{
    public class MapFactory
    {
        private readonly ILogger _logger;
        private Regex _regex;

        public MapFactory()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _regex = new Regex(@"\w+_\d+_\d.?");
        }

        public Map LoadMap(ContentManager contentManager, LevelBlueprint blueprint)
        {
            // Loading all necessary tile sets
            List<TileSet> tileSets = new List<TileSet>();
            foreach (var tileSet in blueprint.TileSetsBlueprints)
            {
                if (!_regex.IsMatch(tileSet.Source))
                {
                    _logger.Warn($"{tileSet.Source} is not available");
                    var lastFirstGId = tileSets.Count > 0 ? tileSets.Last().StartEntry + tileSets.Last().Tiles - 1 : 0;
                    var texture = contentManager.Load<Texture2D>($"Artwork/missing_texture");
                    tileSets.Add(new TileSet(texture, texture.Height, texture.Width, lastFirstGId + 1 ));
                    continue;
                }
                var title = tileSet.Source.Split(".")[0];
                var columns = Int32.Parse(title.Split("_")[1]);
                var rows = Int32.Parse(title.Split("_")[2]);
                try
                {
                    tileSets.Add(new TileSet(contentManager.Load<Texture2D>($"Artwork/Tilemaps/{title}"), columns, rows, tileSet.FirstGId));
                }
                catch (FileNotFoundException e)
                {
                    _logger.Warn($"Texture {title} is missing: ", e);
                    var lastFirstGId = tileSets.Count > 0 ? tileSets.Last().StartEntry + tileSets.Last().Tiles - 1 : 0;
                    tileSets.Add(new TileSet(contentManager.Load<Texture2D>($"Artwork/missing_texture"), columns, rows, lastFirstGId + 1 ));
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    throw;
                }
            }
            return new Map(blueprint, new TileMap(tileSets));
        }
    }
}