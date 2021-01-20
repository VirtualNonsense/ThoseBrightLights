using System;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components.Controls;
using SE_Praktikum.Core;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class ControlElementFactory
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly ContentManager _contentManager;
        private TileSet _buttonsAndSwitches;
        private SpriteFont _font;
        private const int _tileWidth = 32;
        private const int _tileHeight = 32;
        private Logger _logger;

        
        // #############################################################################################################
        // constructor
        // #############################################################################################################
        public ControlElementFactory(AnimationHandlerFactory animationHandlerFactory, ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _contentManager = contentManager;
            _logger = LogManager.GetCurrentClassLogger();
        }
        // #############################################################################################################
        // public methods
        // #############################################################################################################


        /// <summary>
        /// constructs a textbox with tilesX times tilesY tiles
        /// </summary>
        /// <param name="tilesX"></param>
        /// <param name="tilesY"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TextBox GetTextBoxByTiles(uint tilesX, uint tilesY, Vector2 position, Color color, string text = "",  Camera camera = null)
        {
            List<AnimationHandler> handlers = new List<AnimationHandler>();
            loadAssetsIfNecessary();

            for (var y = 0; y < tilesY; y++)
            {
                for (var x = 0; x < tilesX; x++)
                {
                    AnimationSettings animationSettings;
                    switch (TableHelper.GetTablePos(x, y, tilesX-1, tilesY-1))
                    {
                        case TableHelper.TablePosition.TopLeft:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (40, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.TopRight:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (42, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.Top:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (41, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.Right:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (50, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.BottomRight:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (58, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.Bottom:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (57, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.BottomLeft:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (56, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.Left:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (48, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.Middle:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (49, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.SingleColumnTop:
                            throw new NotImplementedException();
                        case TableHelper.TablePosition.SingleColumnMiddle:
                            throw new NotImplementedException();
                        case TableHelper.TablePosition.SingleColumnBottom:
                            throw new NotImplementedException();
                        case TableHelper.TablePosition.SingleRowLeft:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (32, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.SingleRowMiddle:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (33, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.SingleRowRight:
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (34, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.SingleTile:
                            throw new NotImplementedException();
                        default:
                            throw new NotImplementedException();
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

            return new TextBox(handlers, _font, position, color, camera, text);
        }
        
        /// <summary>
        /// constructs a button with approximately the given dimension  
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public MenuButton GetButton(uint width, uint height, Vector2 position,
            string text = "", Camera camera = null)
        {
            loadAssetsIfNecessary();
            uint tilesX = (uint) (width / _buttonsAndSwitches.TileDimX);
            uint tilesY = (uint) (height / _buttonsAndSwitches.TileDimY);
            return GetButtonByTiles(tilesX, tilesY, position, text, camera);
        }
        /// <summary>
        /// constructs a button with tilesX times tilesY tiles
        /// </summary>
        /// <param name="tilesX"></param>
        /// <param name="tilesY"></param>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MenuButton GetButtonByTiles(uint tilesX, uint tilesY, Vector2 position, string text = "",  Camera camera = null)
        {
            List<AnimationHandler> handlers = new List<AnimationHandler>();
            loadAssetsIfNecessary();

            for (var y = 0; y < tilesY; y++)
            {
                for (var x = 0; x < tilesX; x++)
                {
                    AnimationSettings animationSettings;
                    
                    switch (TableHelper.GetTablePos(x, y, tilesX - 1, tilesY - 1))
                    {
                        case TableHelper.TablePosition.TopLeft:
                            //_logger.Trace($"pos {x} {y} top left");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (8, 1f),
                                (11, 1f)
                            });
                            break;
                        case TableHelper.TablePosition.TopRight:
                            //_logger.Trace($"pos {x} {y} top right");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (10, 1f),
                                (13, 1f)
                            });
                            break;
                        case TableHelper.TablePosition.Top:
                            //_logger.Trace($"pos {x} {y} top");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (9, 1f),
                                (12, 1f)
                            });
                            break;
                        case TableHelper.TablePosition.Right:
                            //_logger.Trace($"pos {x} {y} right");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (18, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.BottomRight:
                            //_logger.Trace($"pos {x} {y} bottom right");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (26, 1f),
                                (21, 1f)
                            });
                            break;
                        case TableHelper.TablePosition.Bottom:
                            //_logger.Trace($"pos {x} {y} bottom");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (25, 1f),
                                (20, 1f)
                            });
                            break;
                        case TableHelper.TablePosition.BottomLeft:
                            //_logger.Trace($"pos {x} {y} bottom left");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (24, 1f),
                                (19, 1f)
                            });
                            break;
                        case TableHelper.TablePosition.Left:
                            //_logger.Trace($"pos {x} {y} left");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (16, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.Middle:
                            //_logger.Trace($"pos {x} {y}");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (17, 1f)
                            });
                            break;
                        case TableHelper.TablePosition.SingleColumnTop:
                            //_logger.Trace($"row {y} top left, only one column");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (14, 1f), // normal
                                (15, 1f), // clicked
                            });
                            break;
                        case TableHelper.TablePosition.SingleColumnMiddle:
                            //_logger.Trace($"row {y} left, only one column");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (22, 1f), // normal
                            });
                            break;
                        case TableHelper.TablePosition.SingleColumnBottom:
                            //_logger.Trace($"row {y} bottom left, only one column");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (30, 1f), // normal
                                (23, 1f), // clicked
                            });
                            break;
                        case TableHelper.TablePosition.SingleRowLeft:
                            //_logger.Trace($"column {x} top left, only one line");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (0, 1f), // normal
                                (3, 1f), // clicked
                            });
                            break;
                        case TableHelper.TablePosition.SingleRowMiddle:
                            //_logger.Trace($"column {x} , only one line");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (1, 1f),
                                (4, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.SingleRowRight:
                            //_logger.Trace($"column {x} top right, only one line");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (2, 1f),
                                (5, 1f),
                            });
                            break;
                        case TableHelper.TablePosition.SingleTile:
                            //_logger.Trace($"Single tile");
                            animationSettings = new AnimationSettings(new List<(int, float)>
                            {
                                (6, 1f), // normal
                                (7, 1f), // clicked
                            });
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
            var soundEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Button/Button_dry");
            return new MenuButton(handlers, _font, soundEffect, text: text, position: position, camera: camera, textOffSetWhenPressed: 3);;
        }
        /// <summary>
        /// Creates slider with approximately the given dimensions 
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="camera"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public Slider GetSlider(
            float initialValue,
            float min,
            float max,
            Vector2 position,
            uint width,
            Camera camera = null, float layer = 0)
        {
            
            loadAssetsIfNecessary();
            uint tilesX = (uint) (width / _buttonsAndSwitches.TileDimX);
            return GetSliderByTiles(initialValue, min, max, position, tilesX, camera);
        }
        /// <summary>
        /// creates a slider with the given tilewidth
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="position"></param>
        /// <param name="tilesX"></param>
        /// <param name="camera"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public Slider GetSliderByTiles(
            float initialValue,
            float min,
            float max,
            Vector2 position,
            uint tilesX,
            Camera camera = null, 
            float layer = 0)
        {
            List<AnimationHandler> handlers = new List<AnimationHandler>();
            loadAssetsIfNecessary();

            for (var x = 0; x < tilesX; x++)
            {
                AnimationSettings animationSettings;
                if (x == 0)
                {
                    animationSettings = new AnimationSettings(new List<(int, float)>
                    {
                        (27,1f)
                    });
                }
                else if (x == tilesX - 1)
                {
                    animationSettings = new AnimationSettings(new List<(int, float)>
                    {
                        (29,1f)
                    });
                }
                else
                {
                    animationSettings = new AnimationSettings(new List<(int, float)>
                    {
                        (28,1f)
                    });
                }

                animationSettings.IsPlaying = false;
                animationSettings.Layer = layer;
                var handler = _animationHandlerFactory.GetAnimationHandler(
                    _buttonsAndSwitches, 
                    animationSettings,
                    new Vector2(x * _tileWidth, 0),
                    Vector2.Zero
                );
                handlers.Add(handler);
            }

            var sliderSettings = new AnimationSettings(new List<(int, float)>
            {
                (31, 1f), // when still
                (39, 1f) // when dragged
            }) 
            {
                IsPlaying = false,
                Layer =  layer + float.Epsilon // forcing render order
            };
            var _sliderBladeHandler = new List<AnimationHandler>
            {
                _animationHandlerFactory.GetAnimationHandler(
                    _buttonsAndSwitches, 
                    sliderSettings,
                    Vector2.Zero,
                    Vector2.Zero
                )
            };
            var soundEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Button/Button_slider_block_single");
            return new Slider(initialValue, min, max,  position, new Slider.SliderHandle(_sliderBladeHandler, camera), handlers, soundEffect, camera);
        }
        
        // #############################################################################################################
        // private methods
        // #############################################################################################################
        private void loadAssetsIfNecessary()
        {
            _buttonsAndSwitches ??= new TileSet(_contentManager.Load<Texture2D>("Artwork/Tilemaps/ButtonsAndSwitches"), _tileWidth,
                _tileHeight, null);
            _font ??= _contentManager.Load<SpriteFont>("Font/Font2");
        }
    }
}