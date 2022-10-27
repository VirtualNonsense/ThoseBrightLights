using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using NLog;
using ThoseBrightLights.Components;
using ThoseBrightLights.Extensions;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Core
{
    public class Camera
    {
        private readonly CameraControls _controls;

        private Vector3 _position;
        private readonly Viewport _viewport;
        private readonly BasicEffect _spriteEffect;
        private Logger _logger;
        private IComponent _target;
        private int _rotation;
        private Vector3 _offSetVector = Vector3.Up;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="fieldOfView">field of view in deg</param>
        /// <param name="viewport"></param>
        /// <param name="spriteEffect"></param>
        /// <param name="cameraSpeed"></param>
        /// <param name="cameraZoomSpeed"></param>
        /// <param name="zNearPlane"></param>
        /// <param name="zFarPlane"></param>
        /// <param name="controls"></param>
        public Camera(Vector3 position,
                      float fieldOfView, 
                      Viewport viewport,
                      BasicEffect spriteEffect, 
                      float? cameraSpeed = 1000, 
                      float cameraZoomSpeed = 5000f, 
                      float zNearPlane = .1f, 
                      float zFarPlane = float.MaxValue, 
                      CameraControls controls = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _position = position;
            _viewport = viewport;
            _spriteEffect = spriteEffect;
            FieldOfView = fieldOfView;
            CameraSpeed = cameraSpeed ?? fieldOfView;
            CameraZoomSpeed = cameraZoomSpeed;
            ZNearPlane = zNearPlane;
            ZFarPlane = zFarPlane;
            _controls = controls ?? new CameraControls(Keys.U,
                                                       Keys.J, 
                                                       Keys.H, 
                                                       Keys.K, 
                                                       Keys.I, 
                                                       Keys.Y, 
                                                       Keys.L, 
                                                       Keys.O );
        }
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                if (_target != null)
                    _logger.Warn($"camera won't be moved because it's still following {_target}");
            }
        }
        /// <summary>
        /// determines the area covered by the camera
        /// Set value in degrees
        /// </summary>
        public float FieldOfView { get; set; }
        
        /// <summary>
        /// determines one end of the viewing distance
        /// </summary>
        public float ZNearPlane { get; }
        
        /// <summary>
        /// determines one end of the viewing distance
        /// </summary>
        public float ZFarPlane { get; }
        
        /// <summary>
        /// free cam movement speed
        /// </summary>
        public float CameraSpeed { get; set; }
        
        /// <summary>
        /// free cam movement speed
        /// </summary>
        public float CameraZoomSpeed { get; set; }
        
        /// <summary>
        /// overwrite following mode and move the camera freely 
        /// </summary>
        public bool FreeCam { get; set; }

        // #############################################################################################################
        // public methods
        // #############################################################################################################
        /// <summary>
        /// Follow a given object e.g. the player
        /// </summary>
        /// <param name="target"></param>
        public void Follow(IComponent target)
        {
            _target = target;
        }
        
        /// <summary>
        /// Disable following mode
        /// </summary>
        public void StopFollowing()
        {
            _target = null;
        }

        public void Update(GameTime gameTime)
        {
            
            if(FreeCam)
            {
                if (_controls == null || Keyboard.GetState().GetPressedKeyCount() == 0) return;
                var time = (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (Keyboard.GetState().IsKeyDown(_controls.Down))
                    _position.Y += CameraSpeed * time + _position.Z / 2 * time;

                if (Keyboard.GetState().IsKeyDown(_controls.Up))
                    _position.Y -= CameraSpeed * time + _position.Z / 2 * time;

                if (Keyboard.GetState().IsKeyDown(_controls.Right))
                    _position.X += CameraSpeed * time + _position.Z / 2 * time;

                if (Keyboard.GetState().IsKeyDown(_controls.Left))
                    _position.X -= CameraSpeed * time + _position.Z / 2 * time;

                if (Keyboard.GetState().IsKeyDown(_controls.ZoomOut))
                {
                    _position.Z += CameraZoomSpeed * time;
                    if (_position.Z > ZFarPlane)
                        _position.Z = ZFarPlane - float.Epsilon;
                }

                if (Keyboard.GetState().IsKeyDown(_controls.ZoomIn))
                {
                    _position.Z -= CameraZoomSpeed * time;
                    if (_position.Z < ZNearPlane)
                        _position.Z = 0 + float.Epsilon;
                }

                if (Keyboard.GetState().IsKeyDown(_controls.TurnRight))
                {
                    _rotation += 1;
                    if (_rotation > 359)
                        _rotation = 0;
                }

                if (Keyboard.GetState().IsKeyDown(_controls.TurnLeft))
                {
                    _rotation -= 1;
                    if (_rotation < 0)
                        _rotation = 359;
                }
                return;
            }

            if (_target == null)
                return;
                    
            _position = new Vector3(_target.Position, _position.Z);
        }
        
        /// <summary>
        /// Returns the effect for drawing a spriteeffect
        /// </summary>
        /// <returns></returns>
        public BasicEffect GetCameraEffect()
        {
            _spriteEffect.VertexColorEnabled = false;
            _spriteEffect.TextureEnabled = true;
            _spriteEffect.View = View();
            _spriteEffect.Projection = GetProjection();
            return _spriteEffect;
        }
        
        /// <summary>
        /// returns a basiceffect configured for primitive rendering e.g. triangles
        /// </summary>
        /// <returns></returns>
        public BasicEffect GetCameraEffectForPrimitives()
        {
            var t = GetCameraEffect();
            t.TextureEnabled = false;
            t.VertexColorEnabled = true;
            return t;
        }
        
        /// <summary>
        /// calculates the perspective position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 ProjectScreenPosIntoWorld(Vector2 position)
        {
            var angle = MathExtensions.DegToRad(FieldOfView/2);
            var max = (float)Math.Tan(angle) * _position.Z;
            var x = MathExtensions.Remap(position.X, 0, _viewport.Width, - max * _viewport.AspectRatio, max * _viewport.AspectRatio);
            var y = MathExtensions.Remap(position.Y, 0, _viewport.Height, - max, max);
            return new Vector2(x + _position.X - .5f, y + _position.Y - 2.5f); // adding camera position and tooling numbers
        }
        
        /// <summary>
        /// Calculates the screen width on a specific layer
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public uint GetPerspectiveScreenWidth(float height = 0)
        {
            height = _position.Z - height;
            if (height <= 0)
                throw new ArgumentException($"height should be smaller than Position.Z ({_position.Z})");
            var angle = MathExtensions.DegToRad(FieldOfView / 2);
            var max = (float) Math.Tan(angle) * _position.Z;
            return (uint) (2 * max * _viewport.AspectRatio);
        }
        
        /// <summary>
        /// calculates the screen height on a given height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public uint GetPerspectiveScreenHeight(float height = 0)
        {
            // sanity checking height (math.abs would work but if that case is true something went wrong i guess)
            height = _position.Z - height;
            if (height <= 0)
                throw new ArgumentException($"height should be smaller than Position.Z ({_position.Z})");
            // get angle
            var angle = MathExtensions.DegToRad(FieldOfView / 2);
            // calc one half of the height
            var max = (float) Math.Tan(angle) * _position.Z;
            // doubling and returning 
            return (uint) (2 * max);
        }
        
        // #############################################################################################################
        // private methods
        // #############################################################################################################
        /// <summary>
        /// get's the matrix that sets the camera perspective
        /// </summary>
        /// <returns></returns>
        private Matrix View()
        {
            // this caused so much pain....
            // for some reason the perspective the Y access in monogame is flipped
            // this without the reflection on the p layer this method fixes this "problem" but unfortunately this is not
            // how the rendering works. therefore everything is upside down. including the graphics
            var p = new Plane(new Vector3(0, 1, 0), 1);
            return Matrix.CreateLookAt(_position, _position + Vector3.Forward, Vector3.Up) // set position
                   * Matrix.CreateReflection(p) // reflect on p -> set y access right
                   * Matrix.CreateRotationZ(_rotation * (float)Math.PI/180); // turn view (usually not used)
        }

        private Matrix GetProjection()
        {
            // creates perspective rendering where stuff that's on a higher layer appears more closer
            return Matrix.CreatePerspectiveFieldOfView((float) (FieldOfView * Math.PI/180f), _viewport.AspectRatio, ZNearPlane, ZFarPlane);
        }
        
        // #############################################################################################################
        // Subclasses
        // #############################################################################################################
        
        /// <summary>
        /// holds the camera inputs
        /// </summary>
        public class CameraControls : Input
        {
            public Keys ZoomIn { get; set; }
        
            public Keys ZoomOut { get; set; }

            public CameraControls(Keys up, Keys down, Keys left, Keys right, Keys turnRight, Keys turnLeft, Keys zoomIn, Keys zoomOut) : base(up, down, left, right, turnLeft, turnRight, Keys.Add)
            {
                ZoomIn = zoomIn;
                ZoomOut = zoomOut;
            }
        }
    }
}