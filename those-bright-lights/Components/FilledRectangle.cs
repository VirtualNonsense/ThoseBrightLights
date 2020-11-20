using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace SE_Praktikum.Components
{
    public class FilledRectangle : IComponent, IDisposable
    {
        private readonly Texture2D _texture;
        private Vector2 _position;
        public Vector2 Origin { get; }
        public float Rotation { get; }
        public float Layer { get; }
        public Vector2 Position { get=>_position; set=>_position = value - Origin; }
        public int Width { get; }
        public int Height { get; }

        public Color Color { get; set; }
        public FilledRectangle(GraphicsDevice graphicsDevice,
                        Vector2 position,
                        Vector2? origin,
                        float rotation,
                        float layer,
                        int width,
                        int height,
                        Color color, 
                        Color? borderColor = null, 
                        bool roundEdges = false)
        {
            Position = position;
            Origin = origin ?? new Vector2(width/2f, height/2f);
            Rotation = rotation;
            Layer = layer;
            Width = width;
            Height = height;
            Color = Color.White;
            _texture = new Texture2D(graphicsDevice, Width, Height);
            SetColor(color, borderColor, roundEdges);
        }
        
        
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

        public void SetColor(Color color, Color? borderColor = null, bool roundEdges = false)
        {

            Color[] data = new Color[Width * Height];
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if(roundEdges && (row == 0 || row == Height - 1) && (column == 0 || column == Width - 1))
                        continue;
                    if (row == 0 || row == Height - 1 || column == 0 || column == Width - 1)
                        data[Width * row + column] = borderColor ?? color;
                    else
                        data[Width * row + column] = color;
                }
            }
            _texture.SetData(data);
        }
        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Position,
                null,
                Color,
                Rotation,
                Origin,
                1,
                SpriteEffects.None,
                Layer);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Dispose()
        {
            _texture?.Dispose();
        }
    }
}
