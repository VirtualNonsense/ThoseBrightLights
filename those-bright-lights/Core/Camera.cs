using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Models;

namespace SE_Praktikum.Core
{
    public class Camera
    {
        private readonly CameraControls _controls;

        private Vector3 _position;
        public Vector3 Position
        {
            get => _position;
        }
        public float CameraViewWidth { get; }
        public float CameraViewHeight { get; }
        public float ZNearPlane { get; }
        public float ZFarPlane { get; }
        
        public float CameraSpeed { get; set; }
        
        public float CameraZoomSpeed { get; set; }

        public Camera(Vector3 position, float cameraViewWidth, float cameraViewHeight, float? cameraSpeed = null, float cameraZoomSpeed = .5f, float zNearPlane = 0f, float zFarPlane = -1f, CameraControls controls = null)
        {
            _position = position;
            CameraViewWidth = cameraViewWidth;
            CameraViewHeight = cameraViewHeight;
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
                                                       Keys.PageUp, 
                                                       Keys.PageDown );
        }

        public Matrix View()
        {
            return Matrix.CreateLookAt(Position, Position + Vector3.Forward, Vector3.Up);
        }
        
        public Matrix GetProjection()
        {
            return Matrix.CreateOrthographic(CameraViewWidth, CameraViewHeight, ZNearPlane, ZFarPlane);
        }

        public void Update(GameTime gameTime)
        {
            if (_controls != null)
            {
                var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Keyboard.GetState().IsKeyDown(_controls.Down))
                    _position.Y += CameraSpeed * time;
                
                if (Keyboard.GetState().IsKeyDown(_controls.Up))
                    _position.Y -= CameraSpeed * time;
                
                if (Keyboard.GetState().IsKeyDown(_controls.Left))
                    _position.X += CameraSpeed * time;
                
                if (Keyboard.GetState().IsKeyDown(_controls.Right))
                    _position.X -= CameraSpeed * time;

                if (Keyboard.GetState().IsKeyDown(_controls.ZoomIn))
                    _position.Z += CameraZoomSpeed * time;
                
                if (Keyboard.GetState().IsKeyDown(_controls.ZoomOut))
                    _position.Z += CameraZoomSpeed * time;

            }
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
        
    }
}