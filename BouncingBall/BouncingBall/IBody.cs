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
    int Width { get; }
    int Height { get; }
  }

  public interface ISprite : IBody
  {
    Vector2 Position { get; set; }
    int Speed { get; set; }
    Vector2 Velocity { get; set; }
    IEnclosure Bounds { get; }
    void CheckCollision(ISprite another);
    void CheckCollisionWithEnclosure();
    void Draw(SpriteBatch spriteBatch);
  }

  public interface IUserControlled : ISprite
  {
    Vector2 Direction { get; set; }
    void Move(GameTime gametime);
  }

  public interface IAiControlled : ISprite
  {
    Vector2 InitialVelocity { get; }
    void Move(GameTime gametime);
  }

  public class Ball : ISprite
  {
    public Vector2 Position { get; set; }
    public int Speed { get; set; }
    public Vector2 Velocity { get; set; }

    protected Texture2D Image { get; private set; }
    public IEnclosure Bounds { get; private set; } // ennclosure
    protected Rectangle BoundingBox { get; private set; } // rectangle for the enclosure
    protected float Radius { get; private set; }

    protected Vector2 Center 
    { 
      get
      {
        var x = Position.X + Image.Width / 2;
        var y = Position.Y + Image.Height / 2;

        return new Vector2(x, y);
      }
    }

    protected Rectangle BallBox // bounding rectangle for the image
    { 
      get
      {
        var r = new Rectangle((int)(Center.X - Radius),(int)(Center.Y - Radius),(int)(Radius * 2),(int)(Radius * 2));
        return r;
      }
    } 

    public Ball(Texture2D image, Vector2 initialPosition, IEnclosure bounds)
    {
      this.Image = image;
      this.Position = initialPosition;
      this.Bounds = bounds;
      this.BoundingBox = new Rectangle(0, 0, bounds.Width, bounds.Height);
      this.Radius = image.Width / 2;
    }

    public void CheckCollision(ISprite another)
    { }

    public void CheckCollisionWithEnclosure()
    {
      var normal = GetNormalWithWall();
      if (normal != Vector2.Zero)
      {
        Velocity = -2 * (Vector2.Dot(Velocity, normal) * normal) + Velocity; 
      }
    }

    /// <summary>
    /// Get the normal vector when the ball is touching a wall, if not touching return Vector2.Zero
    /// Assumption is that the bounding rectangle is not at an angle with the monitor.
    /// </summary>
    /// <returns></returns>
    protected Vector2 GetNormalWithWall()
    {
      Vector2 normal = Vector2.Zero;
      if (BallBox.Left <= BoundingBox.Left) // touching left wall
        normal = new Vector2(-1, 0);
      
      if (BallBox.Right >= BoundingBox.Right) // touching right wall
        normal = new Vector2(1, 0);

      if (BallBox.Top <= BoundingBox.Top) // touching top wall
        normal = new Vector2(0, -1);

      if (BallBox.Bottom >= BoundingBox.Bottom) // touching top wall
        normal = new Vector2(0, 1);

      return normal;
      // TODO check for corner collisions
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Image, this.Position, Color.White);
    }
  }

  public class Enemy : Ball, IAiControlled
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

  public class Enclosure : IEnclosure
  {
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public Enclosure(int width, int height)
    {
      this.Width = width;
      this.Height = height;
    }
  }


}
