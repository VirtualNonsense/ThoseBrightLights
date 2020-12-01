using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Core;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class ControlElementFactory
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private TileSet _buttonsAndSwitches;
        private const int _tileWidth = 32;
        private const int _tileHeight = 32;
        private Logger _logger;

        public ControlElementFactory(AnimationHandlerFactory animationHandlerFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _logger = LogManager.GetCurrentClassLogger();
        }

        private void loadAssetsIfNecessary(ContentManager contentManager)
        {
            _buttonsAndSwitches ??= new TileSet(contentManager.Load<Texture2D>("Artwork/Tilemaps/ButtonsAndSwitches"), _tileWidth,
                _tileHeight);
        }

        public MenuButton GetInstanceByDimension(ContentManager contentManager, uint width, uint height, Vector2 position,
            string text = "", bool useCenterAsOrigin = true, Camera camera = null)
        {
            loadAssetsIfNecessary(contentManager);
            uint tilesX = (uint) (width / _buttonsAndSwitches.TileDimX);
            uint tilesY = (uint) (height / _buttonsAndSwitches.TileDimY);
            return GetInstance(contentManager,tilesX, tilesY, position, text, useCenterAsOrigin, camera);
        }
        public MenuButton GetInstance(ContentManager contentManager, uint tilesX, uint tilesY, Vector2 position, string text = "", bool useCenterAsOrigin = true, Camera camera = null)
        {
            List<AnimationHandler> handlers = new List<AnimationHandler>();
            loadAssetsIfNecessary(contentManager);

            AnimationSettings animationSettings;
            for (var y = 0; y < tilesY; y++)
            {
                for (var x = 0; x < tilesX; x++)
                {
                    // Top left
                    if (x == 0 && y == 0)
                    {
                        // only one line
                        if (y == tilesY - 1)
                        {
                            _logger.Trace($"column {x} top left, only one line");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (0, 1f), // normal
                                (3, 1f), // clicked
                            });
                        }
                        else
                        {
                            _logger.Trace($"pos {x} {y} top left");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (8, 1f),
                                (11, 1f)
                            });
                        }
                    }
                    // Top right
                    else if (x == tilesX - 1 && y == 0)
                    {
                        // only one line
                        if (y == tilesY - 1)
                        {
                            _logger.Trace($"column {x} top right, only one line");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (2, 1f),
                                (5, 1f),
                            });
                        }
                        else
                        {
                            _logger.Trace($"pos {x} {y} top right");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (10, 1f),
                                (13, 1f)
                            });
                        }
                    }
                    // Bottom left
                    else if (x == 0 && y == tilesY - 1)
                    {
                        _logger.Trace($"pos {x} {y} bottom left");
                        animationSettings = new AnimationSettings(new List<(int, float)>
                        {
                            (24, 1f),
                            (19, 1f)
                        });
                    }
                    // Bottom right
                    else if (x == tilesX - 1 && y == tilesY - 1)
                    {
                        _logger.Trace($"pos {x} {y} bottom right");
                        animationSettings = new AnimationSettings(new List<(int, float)>
                        {
                            (26, 1f),
                            (21, 1f)
                        });
                    }
                    // Top
                    else if(y == 0)
                    {
                        // only one line
                        if (y == tilesY - 1)
                        {
                            _logger.Trace($"column {x} , only one line");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (1, 1f),
                                (4, 1f),
                            });
                        }
                        else
                        {
                            _logger.Trace($"pos {x} {y} top");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (9, 1f),
                                (12, 1f)
                            });
                        }
                    }
                    // Bottom
                    else if(y == tilesY - 1)
                    {
                        _logger.Trace($"pos {x} {y} bottom");
                        animationSettings = new AnimationSettings(new List<(int, float)>
                        {
                            (25, 1f),
                            (20, 1f)
                        });
                    }
                    // Left
                    else if (x == 0)
                    {
                        _logger.Trace($"pos {x} {y} left");
                        animationSettings = new AnimationSettings(new List<(int, float)>
                        {
                            (16, 1f),
                        });
                        
                    }
                    // Right
                    else if (x == tilesX - 1)
                    {
                        _logger.Trace($"pos {x} {y} right");
                        animationSettings = new AnimationSettings(new List<(int, float)>
                        {
                            (18, 1f),
                        });
                    }
                    // Middle tile
                    else
                    {
                        _logger.Trace($"pos {x} {y}");
                        animationSettings = new AnimationSettings(new List<(int, float)>
                        {
                            (17, 1f)
                        });
                    }
                    
                    animationSettings.IsPlaying = false;
                    // should be < 0 to avoid problem with text rendering
                    animationSettings.Layer = -.10f;
                    var handler = _animationHandlerFactory.GetAnimationHandler(
                        _buttonsAndSwitches, 
                        animationSettings,
                        new Vector2(x * _tileWidth, y * _tileWidth),
                        Vector2.Zero
                    );
                    handlers.Add(handler);
                }
            }
            return new MenuButton(handlers, contentManager.Load<SpriteFont>("Font/Font2"), text: text, position: position, useCenterAsOrigin: useCenterAsOrigin, camera: camera);;
        }
        
    }
}