using Microsoft.Xna.Framework;
using NLog;
using NVorbis.Ogg;
using SE_Praktikum.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Extensions;

namespace SE_Praktikum.Services.Factories
{
    /// <summary>
    /// created this field because I need to build tiles from tiled
    /// </summary>
    public class TileFactory
    {
        // Fields
        private ILogger _logger;
        private readonly AnimationHandlerFactory _factory;

        // Constructor
        public TileFactory(AnimationHandlerFactory factory)
        {
            _factory = factory;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // Build a tile from desired tilesets
        public Tile GenerateTile(uint index, Vector2 position, float layer, List<TileSet> tilesets, float opacity, int width, int height)
        {
            var t = DecodeIndex(index);
            index = t.Item2;
            foreach(var tileset in tilesets)
            {
                if (index > tileset.StartEntry + tileset.Tiles-1)
                    continue;
                var indestructable = true;
                var tileInfo = tileset.GetInfo((int)index);
                if (tileInfo != null)
                    indestructable = !tileInfo.Destructable;
                var settings = new AnimationSettings(new List<(int, float)> {((int) index, 1f)}, isPlaying: false,
                    opacity: opacity, layer: layer);
                var handler = _factory.GetAnimationHandler(tileset, new List<AnimationSettings>(new[] {settings}),
                    origin: Vector2.Zero);
                handler.Position = position;
                return new Tile(handler, t.Item1){Indestructible = indestructable, MaxHealth = 20, Health = 20};
                
                

            }

            _logger.Warn("Tile not found!");
            throw new ArgumentOutOfRangeException();
        }
        // Better performance for tile generation with quadtree
        public QuadTree<Tile> GenerateTiles(List<uint> indices,
            float layer,
            List<TileSet> tilelist,
            int tilewidth,
            int tileheight,
            int rows,
            int columns,
            float layerOpacity,
            Rectangle area)
        {

            if (indices.Count > rows * columns)
            {
                _logger.Error("Indices out of range");
                throw new IndexOutOfRangeException();
            }
            QuadTree<Tile> list = new QuadTree<Tile>(area);
            int row = 0;
            int column = 0;

            foreach(var index in indices)
            {
                if (index != 0)
                {
                    var p = new Vector2(column * tilewidth, row * tileheight);
                    var tile = GenerateTile(index, p, layer, tilelist, layerOpacity, tileheight, tilewidth);
                    tile.OnDeath += (sender, args) =>
                    {
                        list.Remove((Tile)sender);
                    };
                    var hitbox = tile.HitBox == null ? tile.Rectangle : tile.HitBox.GetBoundingRectangle();
                    list.Insert(hitbox,tile);
                }
                column++;
                if(column >= columns)
                {
                    if(column > columns)
                        _logger.Warn("column > columns");
                    column = 0;
                    row++;
                }
                if(row > rows)
                    _logger.Warn("row > rows");

                
            }
            return list;
        }

        private void Tile_OnDeath(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        // Get the orientaion from tile indices (last numbers of 32 bit reprentation)
        private (TileModifier, uint) DecodeIndex(uint codedIndex)
        {
            var mbits = (codedIndex >> 28);
            uint index = mbits << 28 ^ codedIndex;
            if (mbits == 0)
                return (TileModifier.None, index);
            mbits = ReveresUint(mbits<<28);
            try
            {
                TileModifier mod = (TileModifier)(mbits);
                return (mod, index);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        private uint ReveresUint(uint input)
        {
            var t = new BitArray(BitConverter.GetBytes(input));
            uint output = 0;
            for (int i = 0; i < t.Count; i++)
            {
                if (t[t.Count - 1 - i])
                    output += (uint)Math.Pow(2, i);
            }
            return output;

        }

    }
}
