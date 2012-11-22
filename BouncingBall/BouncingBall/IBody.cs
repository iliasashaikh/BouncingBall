using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BouncingBall
{
  public interface IBody
  { }

  public interface IEnclosure
  {
    void CheckCollision(ISprite sprite);
  }

  public interface ISprite : IBody
  {
    Vector2 Position { get; set; }
    int Speed { get; set; }
    Vector2 Velocity { get; set; }
    IEnclosure MovementBounds { get; }
    Rectangle BoundingBox { get; }
    void CheckCollision(ISprite another);
    void CheckCollisionWithEnclosure();
    void Draw(SpriteBatch spriteBatch);
  }

  public interface IUserSprite : ISprite
  {
    Vector2 Direction { get; set; }
    void Move(GameTime gametime);
  }

  public interface IAiSprite : ISprite
  {
    Vector2 InitialVelocity { get; }
    void Move(GameTime gametime);
  }

  /// <summary>
  /// A basic rectangular enclosure (I will can implement more complex enclosures later)
  /// </summary>
  public class RectangleEnclosure : IEnclosure
  {
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Rectangle bounds;
    public RectangleEnclosure(Rectangle bounds)
    {
      this.bounds = bounds;
    }

    public void CheckCollision(ISprite sprite)
    {
      if (IsTouching(sprite))
      {
        var normal = GetNormalWithWall(sprite.BoundingBox);
        // formula: V = (-2 x (V.N) x N) + V
        var velocity = sprite.Velocity;
        velocity = -2 * (Vector2.Dot(velocity, normal) * normal) + velocity;
        sprite.Velocity = velocity;
      }
    }

    private bool IsTouching(ISprite sprite)
    {
      return !bounds.Contains(sprite.BoundingBox);
    }

    /// <summary>
    /// Get the normal vector when the ball is touching a wall, if not touching return Vector2.Zero
    /// Assumption is that the bounding rectangle is not at an angle with the monitor.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetNormalWithWall(Rectangle boundingBox)
    {
      Vector2 normal = Vector2.Zero;
      if (boundingBox.Left <= bounds.Left) // touching left wall
        normal = new Vector2(-1, 0);

      if (boundingBox.Right >= bounds.Right) // touching right wall
        normal = new Vector2(1, 0);

      if (boundingBox.Top <= bounds.Top) // touching top wall
        normal = new Vector2(0, -1);

      if (boundingBox.Bottom >= bounds.Bottom) // touching top wall
        normal = new Vector2(0, 1);

      return normal; 
    }
  }

  public class Ball : ISprite
  {
    public Vector2 Position { get; set; }
    public int Speed { get; set; }
    public Vector2 Velocity { get; set; }
    public IEnclosure MovementBounds { get; private set; } // ennclosure

    protected Texture2D Image { get; private set; }
    protected float Radius { get; private set; }

    protected Vector2 Center 
    { 
      get
      {
        var x = Position.X + Radius;
        var y = Position.Y + Radius;

        return new Vector2(x, y);
      }
    }

    public Rectangle BoundingBox // bounding rectangle for the image
    { 
      get
      {
        var r = new Rectangle((int)Position.X, (int)Position.Y ,(int)(Radius * 2),(int)(Radius * 2));
        return r;
      }
    } 

    public Ball(Texture2D image, Vector2 initialPosition, IEnclosure bounds)
    {
      this.Image = image;
      this.Position = initialPosition;
      this.MovementBounds = bounds;
      this.Radius = image.Width / 2;
    }

    public void CheckCollision(ISprite another)
    { 
      
    }

    public void CheckCollisionWithEnclosure()
    {
      MovementBounds.CheckCollision(this);
    }



    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Image, this.Position, Color.White);
    }
  }

  public class Enemy : Ball, IAiSprite
  {
    private static Random r = new Random();

    public Enemy(Texture2D image, Vector2 initialPosition, IEnclosure bounds, int speed)
      :base(image,initialPosition,bounds)
    {
      this.Speed = speed;
      this.InitialVelocity = new Vector2(r.Next(-1, -1), r.Next(-1, -1));
      this.Velocity = this.InitialVelocity * speed;
    }

    public Vector2 InitialVelocity {get; private set;}
    

    public void Move(GameTime gametime)
    {
      CheckCollisionWithEnclosure();
      this.Position += Velocity * (float)gametime.ElapsedGameTime.TotalSeconds;
    }
  }



}
