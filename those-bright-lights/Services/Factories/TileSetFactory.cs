using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Models.Tiled;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Services.Factories
{
    public class TileSetFactory
    {
        private AnimationHandlerFactory _animationHandlerFactory;
        private ContentManager _contentManager;
        private Logger _logger;


        public TileSetFactory(AnimationHandlerFactory animationHandlerFactory, ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _contentManager = contentManager;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public TileSet GetInstance(string jsonpath, int startindex)
        {
            var TileSet = JsonConvert.DeserializeObject<TileSetBlueprint>(File.ReadAllText(jsonpath));
            var tileSetPath = TileSet.image.Split(".png")[0].Substring(6);
            var Texture = _contentManager.Load<Texture2D>(tileSetPath);
            if(TileSet.tiles == null)
                return new TileSet(Texture, TileSet.tilewidth, TileSet.tileheight, null, startindex);
            var Dictionary = new Dictionary<int, Polygon[]>();
            
            foreach(var tile in TileSet.tiles)
            {
                var count = 0;
                var index = tile.id + startindex;
                foreach(var objectt in tile.objectgroup.objects)
                {
                    Polygon polygon;
                    if (objectt.polygon != null)
                    {
                        List<Vector2> Vector2List = new List<Vector2>();
                        foreach(var p in objectt.polygon)
                        {
                            Vector2List.Add(new Vector2(p.x + objectt.x, p.y + objectt.y));
                        }
                        polygon = new Polygon(Vector2.Zero, Vector2.Zero, 0, Vector2List);
                    }
                    else
                    {
                       polygon = new Polygon(
                       Vector2.Zero,
                       new Vector2(objectt.width / 2, objectt.height / 2),
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
                       
                    if(!(Dictionary.ContainsKey(index)))
                    {
                        Dictionary.Add(index, new Polygon[tile.objectgroup.objects.Length]);

                    }
                    Dictionary[index][count] = polygon;
                    count++;
                    

                }
                
            }
            return new TileSet(Texture, TileSet.tilewidth,TileSet.tileheight,Dictionary,startindex);

        }
    }
}