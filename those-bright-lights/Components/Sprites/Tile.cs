﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.Sprites
{
    public class Tile:IComponent
    {
        Texture2D _texture;
        private readonly Rectangle _frame;
        Vector2 _position;
        private float _layer;

        public Tile(Texture2D texture, Rectangle frame, Vector2 position, float layer)
        {
            _texture = texture;
            _frame = frame;
            _position = position;
            _layer = layer;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _frame, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, _layer);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
