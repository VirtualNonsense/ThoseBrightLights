using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites
{
  public abstract class Sprite : IComponent
  {
    // #################################################################################################################
    // Fields
    // #################################################################################################################
    protected AnimationHandler _animationHandler;
    
    
    // #################################################################################################################
    // Constructor
    // #################################################################################################################

    protected Sprite(AnimationHandler animationHandler)
    {
      _animationHandler = animationHandler;
    }
    // #################################################################################################################
    // Properties
    // #################################################################################################################
    
    public AnimationSettings AnimationSettings => _animationHandler.Settings;
    
    
    public List<Sprite> Children { get; set; }
    
    public Vector2 Origin { 
      get => _animationHandler.Origin;
      set => _animationHandler.Origin = value; 
    }

    public readonly Color[] TextureData;


    public Vector2 Position
    {
      get => _animationHandler.Position;
      set => _animationHandler.Position = value;
    }

    public float X
    {
      get => Position.X;
      set => Position = new Vector2(value, Position.Y);
    }

    public float Y
    {
      get => Position.Y;
      set => Position = new Vector2(Position.X, value);
    }

    public float Layer
    {
      get => _animationHandler.Settings.Layer;
      set => _animationHandler.Settings.Layer = value;
    }

    public float Rotation
    {
      get => _animationHandler.Settings.Rotation;
      set => _animationHandler.Settings.Rotation = value;
    }

    public float Scale
    {
      get => _animationHandler.Settings.Scale;
      set => _animationHandler.Settings.Scale = value;
    }

    public float Opacity
    {
      get => _animationHandler.Settings.Opacity;
      set => _animationHandler.Settings.Opacity = value;
    }
    public Matrix Transform =>
      Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
      Matrix.CreateRotationZ(Rotation) *
      Matrix.CreateTranslation(new Vector3(Position, 0));

    public Rectangle Rectangle
    {
      get
      {
        var width = _animationHandler.FrameWidth;
        var height = _animationHandler.FrameHeight;
        
        return new Rectangle((int) (Position.X - Origin.X), (int) (Position.Y - Origin.Y), (int) (width * Scale),
          (int) (height * Scale));
      }
    }
    

    #region VectorShortcuts

    public Vector2 TopLeft => new Vector2(Rectangle.X, Rectangle.Y);

    public Vector2 TopRight => new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y);

    public Vector2 BottomLeft => new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height);

    public Vector2 BottomRight => new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);

    public Vector2 Centre => new Vector2(Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + (Rectangle.Height / 2));

    public List<Vector2> Dots =>
      new List<Vector2>()
      {
        Centre,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
      };

    public List<Vector2> GetNormals()
    {
      var normals = new List<Vector2>();

      var dots = Dots;

      for (int i = 1; i < dots.Count - 1; i++)
      {
        normals.Add(Vector2.Normalize(new Vector2(dots[i + 1].X - dots[i].X, dots[i + 1].Y - dots[i].Y)));
      }

      normals.Add(
        Vector2.Normalize(new Vector2(dots[1].X - dots[dots.Count - 1].X, dots[1].Y - dots[dots.Count - 1].Y)));

      return normals;
    }

    #endregion

    public bool IsRemoveAble { get; set; }
    public Vector2 Velocity { get; set; }


    // #################################################################################################################
    // Methods
    // #################################################################################################################
    public virtual void Update(GameTime gameTime)
    { 
      _animationHandler.Update(gameTime);
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      _animationHandler.Draw(spriteBatch);
    }
    
  }
}