using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
  public class Sprite : IComponent, ICloneable
  {
    // #################################################################################################################
    // Fields
    // #################################################################################################################
    protected readonly AnimationHandler _animationManager;

    protected readonly Dictionary<string, Animation> _animations;

    protected Texture2D _texture;

    protected Vector2 _position;

    protected IScreen _parent;
    
    // #################################################################################################################
    // Constructor
    // #################################################################################################################
    public Sprite(Texture2D texture)
    {
      _texture = texture;
      
      Rotation = 0;

      Opacity = 1f;

      Scale = 1f;

      Origin = new Vector2(0, 0);

      Colour = Color.White;

      TextureData = new Color[_texture.Width * _texture.Height];
      _texture.GetData(TextureData);
    }

    public Sprite(Dictionary<string, Animation> animations)
    {
      _animations = animations;
      _animationManager = new AnimationHandler(_animations.First().Value);
      
      Rotation = 0;

      Opacity = 1f;

      Scale = 1f;

      Colour = Color.White;
    }
    // #################################################################################################################
    // Properties
    // #################################################################################################################
    protected float _layer { get; set; }

    public Color Colour { get; set; }
    public float Opacity { get; set; }
    public Vector2 Origin { get; set; }
    public float Rotation { get; set; }


    public readonly Color[] TextureData;
    public float Scale { get; set; }

    public Vector2 Position
    {
      get => _position;
      set
      {
        _position = value;

        if (_animationManager != null)
          _animationManager.Position = _position;
      }
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
      get => _layer;
      set
      {
        _layer = value;

        if (_animationManager != null)
          _animationManager.Layer = _layer;
      }
    }
    public Matrix Transform =>
      Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
      Matrix.CreateRotationZ(Rotation) *
      Matrix.CreateTranslation(new Vector3(Position, 0));

    public Rectangle Rectangle
    {
      get
      {
        int width = 0;
        int height = 0;

        if (_texture != null)
        {
          width = _texture.Width;
          height = _texture.Height;
        }
        else if (_animationManager != null)
        {
          width = _animationManager.FrameWidth;
          height = _animationManager.FrameHeight;
        }

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

    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (_texture != null)
        spriteBatch.Draw(_texture, Position, null, Colour * Opacity, Rotation, Origin, Scale, SpriteEffects.None,
          Layer);

      _animationManager?.Draw(spriteBatch);
    }
    
    // TODO: Maybe Introduce event for that as well
    public virtual void OnCollide(Sprite sprite)
    {

    }
    
    protected virtual bool IsTouchingLeft(Sprite sprite)
    {
      return Rectangle.Right  > sprite.Rectangle.Left &&
             Rectangle.Left < sprite.Rectangle.Left &&
             Rectangle.Bottom > sprite.Rectangle.Top &&
             Rectangle.Top < sprite.Rectangle.Bottom;
    }

    protected virtual  bool IsTouchingRight(Sprite sprite)
    {
      return Rectangle.Left  < sprite.Rectangle.Right &&
             Rectangle.Right > sprite.Rectangle.Right &&
             Rectangle.Bottom > sprite.Rectangle.Top &&
             Rectangle.Top < sprite.Rectangle.Bottom;
    }

    protected virtual  bool IsTouchingTop(Sprite sprite)
    {
      return Rectangle.Bottom  > sprite.Rectangle.Top &&
             Rectangle.Top < sprite.Rectangle.Top &&
             Rectangle.Right > sprite.Rectangle.Left &&
             Rectangle.Left < sprite.Rectangle.Right;
    }

    protected virtual bool IsTouchingBottom(Sprite sprite)
    {
      return Rectangle.Top  < sprite.Rectangle.Bottom &&
             Rectangle.Bottom > sprite.Rectangle.Bottom &&
             Rectangle.Right > sprite.Rectangle.Left &&
             Rectangle.Left < sprite.Rectangle.Right;
    }
    
    // TODO: Return Coordinate for effect Placement
    public bool Intersects(Sprite sprite)
    {
      // Calculate a matrix which transforms from A's local space into
      // world space and then into B's local space
      var transformAToB = Transform * Matrix.Invert(sprite.Transform);

      // When a point moves in A's local space, it moves in B's local space with a
      // fixed direction and distance proportional to the movement in A.
      // This algorithm steps through A one pixel at a time along A's X and Y axes
      // Calculate the analogous steps in B:
      var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
      var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

      // Calculate the top left corner of A in B's local space
      // This variable will be reused to keep track of the start of each row
      var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

      for (int yA = 0; yA < Rectangle.Height; yA++)
      {
        // Start at the beginning of the row
        var posInB = yPosInB;

        for (int xA = 0; xA < Rectangle.Width; xA++)
        {
          // Round to the nearest pixel
          var xB = (int)Math.Round(posInB.X);
          var yB = (int)Math.Round(posInB.Y);

          if (0 <= xB && xB < sprite.Rectangle.Width &&
              0 <= yB && yB < sprite.Rectangle.Height)
          {
            // Get the colors of the overlapping pixels
            var colourA = TextureData[xA + yA * Rectangle.Width];
            var colourB = sprite.TextureData[xB + yB * sprite.Rectangle.Width];

            // If both pixel are not completely transparent
            if (colourA.A != 0 && colourB.A != 0)
            {
              return true;
            }
          }

          // Move to the next pixel in the row
          posInB += stepX;
        }

        // Move to the next row
        yPosInB += stepY;
      }

      // No intersection found
      return false;
    }

    public object Clone()
    {
      return MemberwiseClone();
    }
  }
}