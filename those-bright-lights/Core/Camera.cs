using Microsoft.Xna.Framework;

namespace SE_Praktikum.Core
{
    public class Camera
    {
        public Vector3 Position { get; }
        public float CameraViewWidth { get; }
        public float CameraViewHeight { get; }
        public float ZNearPlane { get; }
        public float ZFarPlane { get; }

        public Camera(Vector3 position, float cameraViewWidth, float cameraViewHeight, float zNearPlane = 0f, float zFarPlane = -1f)
        {
            Position = position;
            CameraViewWidth = cameraViewWidth;
            CameraViewHeight = cameraViewHeight;
            ZNearPlane = zNearPlane;
            ZFarPlane = zFarPlane;
        }

        public Matrix View()
        {
            return Matrix.CreateLookAt(Position, Position + Vector3.Forward, Vector3.Up);
        }
        
        public Matrix GetProjection()
        {
            return Matrix.CreateOrthographic(CameraViewWidth, CameraViewHeight, ZNearPlane, ZFarPlane);
        }
        
        public Matrix
        
        
    }
}