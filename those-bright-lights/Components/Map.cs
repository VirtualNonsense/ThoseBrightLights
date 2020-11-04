using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Components.Sprites;
using System.Data;

namespace SE_Praktikum.Components
{
    class Map: IComponent
    {

        LevelBlueprint _blueprint;
        List<TileMap> _tilemaps;

        public Map (LevelBlueprint blueprint, List<TileMap> tilemaps)
        {
            this._tilemaps = tilemaps;
            this._blueprint = blueprint;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        //private static List<Tile> GenerateTiles(TileMap tilemap, LevelBlueprint blueprint)
        //{
        //    List<Tile> tiles = new List<Tile>();
        //    foreach(var layer in blueprint.layers)
        //    {
        //        var c = 0;
        //        foreach(var index in layer.data)
        //        {
        //            if (index == 0)
        //                continue;
        //            var x = c / layer.width;
        //            var y = c % layer.height; 

        //            blueprint.tiles.Add(new Tile());
        //            c++;
        //        }
        //    }
        //}

        private static (int, TileMap) GetTileMap(int index,List <TileMap>maps)
        {
            var offset = 0;
            foreach(var map in maps)
            {
                if(index-offset>map.Tiles)
                {
                    return (index - offset, map);
                }
                offset += map.Tiles;
            }
            return (-1, null);
        }

    }
}
