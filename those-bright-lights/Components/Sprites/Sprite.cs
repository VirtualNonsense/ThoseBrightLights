using System;
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
    protected readonly AnimationHandler _animationHandler;


    // #################################################################################################################
    // Constructor
    // #################################################################################################################
    /// <summary>
    /// Provides everything that's necessary to handle an animated object without collision
    /// </summary>
    /// <param name="animationHandler"></param>
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
    public event EventHandler OnLayerChanged;
    
    // #################################################################################################################
    // Properties
    // #################################################################################################################
    /// <summary>
    /// Origin offset to the zero point in body coordinates
    /// </summary>
    protected virtual Vector2 Origin { 
      get => _animationHandler.Origin;
    }
    
    public virtual Vector2 Position 
    { 
      // currently considering to invert the dependency 
      // the body should know the position not it's animation
      // I might rewrite this part in the future
      get => _animationHandler.Position;
      set
      {
        _animationHandler.Position = value;
        // trigger event
        InvokeOnPositionChanged();
      } 
    }
    
    /// <summary>
    /// shortcut to Position.X
    /// </summary>
    public float X
    {
      get => Position.X;
      set => Position = new Vector2(value, Position.Y);
    }
    
    /// <summary>
    /// shortcut to Position.Y
    /// </summary>
    public float Y
    {
      get => Position.Y;
      set => Position = new Vector2(Position.X, value);
    }
    
    /// <summary>
    /// This determines the perspective depth under the current rendering settings
    /// The z coordinates so to speak
    /// </summary>
    public virtual float Layer
    {
      get => _animationHandler.Layer;
      set { _animationHandler.Layer = value; InvokeOnLayerChanged(); }
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
    
    /// <summary>
    /// Give stuff a name it makes debugging easier.
    /// </summary>
    public string NameTag
    {
      get;
      protected set;
    }

    /// <summary>
    /// Multiplies the size by a constant
    /// </summary>
    public float Scale
    {
      get => _animationHandler.Scale;
      set => _animationHandler.Scale = value;
    }
    
    /// <summary>
    /// the bounding box of the image
    /// </summary>
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
    
    /// <summary>
    /// Property that's used to remove an object after it livecycle has ended
    /// </summary>
    public virtual bool IsRemoveAble { get; set; }

    /// <summary>
    /// Speed vector
    /// </summary>
    public Vector2 Velocity { get; set; }
    
    /// <summary>
    /// The space moved in one tick
    /// the use of this is not obvious. It's
    /// </summary>
    public Vector2 DeltaPosition { get; set; }


    // #################################################################################################################
    // Methods
    // #################################################################################################################
    public virtual void Update(GameTime gameTime)
    { 
      // Update animation if necessary
      _animationHandler.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      // animation frame
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

    protected virtual void InvokeOnLayerChanged()
    {
      OnLayerChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}