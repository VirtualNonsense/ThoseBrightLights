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

        public Vector3 Position
        {
            get => _position;
        }
        public float CameraViewWidth { get; set; }
        public float AspectRatio { get; }
        public float ZNearPlane { get; }
        public float ZFarPlane { get; }
        
        
        public float CameraSpeed { get; set; }
        
        public float CameraZoomSpeed { get; set; }

        public Camera(Vector3 position, float cameraViewWidth, float aspectRatio, BasicEffect spriteEffect, float? cameraSpeed = null, float cameraZoomSpeed = 10, float zNearPlane = 0f, float zFarPlane = -1f, CameraControls controls = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _position = position;
            _spriteEffect = spriteEffect;
            CameraViewWidth = cameraViewWidth;
            AspectRatio = aspectRatio;
            CameraSpeed = cameraSpeed ?? cameraViewWidth;
            CameraZoomSpeed = cameraZoomSpeed;
            ZNearPlane = zNearPlane;
            ZFarPlane = zFarPlane;
            _controls = controls ?? new CameraControls(Keys.Up,
                                                       Keys.Down, 
                                                       Keys.Left, 
                                                       Keys.Right, 
                                                       Keys.OemOpenBrackets, 
                                                       Keys.OemCloseBrackets, 
                                                       Keys.O, 
                                                       Keys.L );
        }

        private Matrix View()
        {
            return Matrix.CreateLookAt(Position, Position + Vector3.Forward, Vector3.Up);
        }

        private Matrix GetProjection()
        {
            return Matrix.CreateOrthographic(CameraViewWidth, CameraViewWidth/AspectRatio, ZNearPlane, ZFarPlane);
        }

        public void Update(GameTime gameTime)
        {
            if (_controls == null || Keyboard.GetState().GetPressedKeyCount() == 0) return;
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(_controls.Up))
                _position.Y += CameraSpeed * time;

            if (Keyboard.GetState().IsKeyDown(_controls.Down))
                _position.Y -= CameraSpeed * time;

            if (Keyboard.GetState().IsKeyDown(_controls.Right))
                _position.X += CameraSpeed * time;
            
            if (Keyboard.GetState().IsKeyDown(_controls.Left))
                _position.X -= CameraSpeed * time;

            if (Keyboard.GetState().IsKeyDown(_controls.ZoomIn))
                CameraViewWidth += CameraZoomSpeed * time;
                
            if (Keyboard.GetState().IsKeyDown(_controls.ZoomOut))
                CameraViewWidth = (CameraViewWidth - CameraZoomSpeed * time > 0)? CameraViewWidth - CameraZoomSpeed * time : 0;
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