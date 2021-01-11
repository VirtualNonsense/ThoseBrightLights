using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components
{
  public class Sprite : IComponent
  {
    // #################################################################################################################
    // Fields
    // #################################################################################################################
    protected AnimationHandler _animationHandler;

    /// <summary>
    /// The time it takes the object to rotate 360 degrees in milliseconds
    /// </summary>
    protected int RotationSpeed = 1000; 
    
    
    // #################################################################################################################
    // Constructor
    // #################################################################################################################

    public Sprite(AnimationHandler animationHandler)
    {
      _animationHandler = animationHandler;
      DeltaPosition = Vector2.Zero;
    }
    // #################################################################################################################
    // Events
    // #################################################################################################################
    public event EventHandler OnPositionChanged;
    public event EventHandler OnRotationChanged;
    
    // #################################################################################################################
    // Properties
    // #################################################################################################################


    public List<Sprite> Children { get; set; }
    
    public virtual Vector2 Origin { 
      get => _animationHandler.Origin;
    }

    public readonly Color[] TextureData;

    
    public virtual Vector2 Position 
    { 
      get => _animationHandler.Position;
      set
      {
        _animationHandler.Position = value;
        InvokeOnPositionChanged();
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

    //TODO: does layer have to be float? maybe use int instead
    public virtual float Layer
    {
      get => _animationHandler.Layer;
      set => _animationHandler.Layer = value;
    }

    public virtual float Rotation
    {
      get => _animationHandler.Rotation;
      set
      {
        _animationHandler.Rotation = value;
        InvokeOnRotationChanged();
      }
    }

    public float Scale
    {
      get => _animationHandler.Scale;
      set => _animationHandler.Scale = value;
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

    public bool IsRemoveAble { get; set; }
    public Vector2 Velocity { get; set; }
    
    public Vector2 DeltaPosition { get; set; }


    // #################################################################################################################
    // Methods
    // #################################################################################################################
    public virtual void Update(GameTime gameTime)
    { 
      _animationHandler.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      _animationHandler.Draw(spriteBatch);
    }
    protected virtual void InvokeOnPositionChanged()
    {
      OnPositionChanged?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void InvokeOnRotationChanged()
    {
      OnRotationChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}