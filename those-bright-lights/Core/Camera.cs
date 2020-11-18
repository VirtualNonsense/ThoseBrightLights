using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;

namespace SE_Praktikum.Core
{
    public class Camera
    {
        private readonly CameraControls _controls;

        private Vector3 _position;
        private readonly BasicEffect _spriteEffect;
        private Logger _logger;
        private int _rotation;

        public Vector3 Position
        {
            get => _position;
        }
        public float FieldOfView { get; set; }
        public float AspectRatio { get; }
        public float ZNearPlane { get; }
        public float ZFarPlane { get; }
        
        
        public float CameraSpeed { get; set; }
        
        public float CameraZoomSpeed { get; set; }

        private Vector3 _offSetVector = Vector3.Up;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="fieldOfView">field of view in deg</param>
        /// <param name="aspectRatio"></param>
        /// <param name="spriteEffect"></param>
        /// <param name="cameraSpeed"></param>
        /// <param name="cameraZoomSpeed"></param>
        /// <param name="zNearPlane"></param>
        /// <param name="zFarPlane"></param>
        /// <param name="controls"></param>
        public Camera(Vector3 position,
                      float fieldOfView, 
                      float aspectRatio, 
                      BasicEffect spriteEffect, 
                      float? cameraSpeed = null, 
                      float cameraZoomSpeed = 5f, 
                      float zNearPlane = .1f, 
                      float zFarPlane = float.MaxValue, 
                      CameraControls controls = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _position = position;
            _spriteEffect = spriteEffect;
            FieldOfView = fieldOfView;
            AspectRatio = aspectRatio;
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
                                                       Keys.O, 
                                                       Keys.L );
        }

        private Matrix View()
        {
            var p = new Plane(new Vector3(0, 1, 0), 1);
            return Matrix.CreateLookAt(_position, _position + Vector3.Forward, Vector3.Up)
                   * Matrix.CreateReflection(p)* Matrix.CreateRotationZ(_rotation * (float)Math.PI/180);
        }

        private Matrix GetProjection()
        {
            return Matrix.CreatePerspectiveFieldOfView((float) (FieldOfView * Math.PI/180f), AspectRatio, ZNearPlane, ZFarPlane);
        }

        public void Update(GameTime gameTime)
        {
            if (_controls == null || Keyboard.GetState().GetPressedKeyCount() == 0) return;
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(_controls.Up))
                _position.Y += CameraSpeed * time + _position.Z/2 * time;

            if (Keyboard.GetState().IsKeyDown(_controls.Down))
                _position.Y -= CameraSpeed * time + _position.Z/2 * time;

            if (Keyboard.GetState().IsKeyDown(_controls.Right))
                _position.X += CameraSpeed * time + _position.Z/2 * time;
            
            if (Keyboard.GetState().IsKeyDown(_controls.Left))
                _position.X -= CameraSpeed * time + _position.Z/2 * time;

            if (Keyboard.GetState().IsKeyDown(_controls.ZoomIn))
            {
                _position.Z += CameraZoomSpeed * time;
                if (_position.Z > ZFarPlane)
                    _position.Z = ZFarPlane - float.Epsilon;
            }
                
            if (Keyboard.GetState().IsKeyDown(_controls.ZoomOut))
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
                
            _logger.Debug(_position);
        }

        public class CameraControls : Input
        {
            public Keys ZoomIn { get; set; }
        
            public Keys ZoomOut { get; set; }

            public CameraControls(Keys up, Keys down, Keys left, Keys right, Keys turnRight, Keys turnLeft, Keys zoomIn, Keys zoomOut) : base(up, down, left, right, turnLeft, turnRight)
            {
                ZoomIn = zoomIn;
                ZoomOut = zoomOut;
            }
        }

        public BasicEffect GetCameraEffect()
        {
            _spriteEffect.View = View();
            _spriteEffect.Projection = GetProjection();
            return _spriteEffect;
        }
        
    }
}