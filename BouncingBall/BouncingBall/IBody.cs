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
    void CheckCollision(ISprite sprite);
  }

  public interface ISprite : IBody
  {
    Texture2D Image { get;  }
    Vector2 Position { get; set; }
    Vector2 Velocity { get; set; }
    int Speed { get; }
    int Mass { get; } 
    
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
    public int Width { get { return bounds.Width; } }
    public int Height { get { return bounds.Height; } }
    
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
      // cannot use Rectangle.contains, it misses the case where the we are just touching, i.e. x = 0 etc.
      if (sprite.BoundingBox.Left <= bounds.Left ||
          sprite.BoundingBox.Right >= bounds.Right ||
          sprite.BoundingBox.Bottom >= bounds.Bottom ||
          sprite.BoundingBox.Top <= bounds.Top)

        return true;

      return false;
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
        normal += new Vector2(-1, 0);

      if (boundingBox.Right >= bounds.Right) // touching right wall
        normal += new Vector2(1, 0);

      if (boundingBox.Top <= bounds.Top) // touching top wall
        normal += new Vector2(0, -1);

      if (boundingBox.Bottom >= bounds.Bottom) // touching top wall
        normal += new Vector2(0, 1);

      normal.Normalize();

      return normal; 
    }
  }

  public class Ball : ISprite
  {
    public Vector2 Position { get; set; }
    public int Speed { get; set; }

    private Vector2 velocity;
    public Vector2 Velocity 
    {
      get
      {
        return velocity;
      }
      set
      {
        velocity = value;
      }
    }
    
    public IEnclosure MovementBounds { get; private set; } // ennclosure
    public int Mass { get; private set; }

    public Texture2D Image { get; private set; }
    protected float Radius { get; private set; }

    protected List<ISprite> AllSprites { get; private set; }

    protected Vector2 Center 
    { 
      get
      {
        var x = Position.X + Image.Width / 2;
        var y = Position.Y + Image.Height / 2;

        return new Vector2(x, y);
      }
    }

    public Rectangle BoundingBox // bounding rectangle for the image
    { 
      get
      {
        var r = new Rectangle((int)Position.X, (int)Position.Y, this.Image.Width, this.Image.Height);
        return r;
      }
    } 

    public Ball(Texture2D image, Vector2 initialPosition, IEnclosure bounds, int mass, List<ISprite> allSprites)
    {
      this.Image = image;
      this.Position = initialPosition;
      this.MovementBounds = bounds;
      this.Radius = image.Width / 2;
      this.Mass = mass;
      this.AllSprites = allSprites;
    }

    private bool IsTouching(ISprite another)
    {
      var distance = (this.Center - ((Ball)another).Center).Length();
      var r1 = this.BoundingBox.Width / 2;
      var r2 = another.BoundingBox.Width / 2;

      return (distance <= (r1 + r2));

      //return ((this.Center - ((Ball)another).Center).Length() <= (this.Radius + ((Ball)another).Radius));
    }

    public void CheckCollision(ISprite another)
    {
      /*
       * calculation (ref - http://en.wikipedia.org/wiki/Elastic_collision, http://www.vobarian.com/collisions/2dcollisions2.pdf)
       * -----------
       * Va, Vb -> original velocities
       * Ma, Mb -> original masses
       * 
       * 1. calculate the collision normal & tangential vectors
       * N = Normalise(Va - Vb) 
       * T = Vector(N.x, -N.y)
       * 
       * 2. split each vector into its normal and tangential components around the normal vector
       * Van = Va.N
       * Vat = Va.T
       * 
       * 3. compute the new normal vector post an elastic collision
       * Van' = [Van x (Ma - Mb) + 2 x Mb x Vbn] / [Ma + Mb]
       * Vbn' = [Vbn x (Mb - Ma) + 2 x Ma x Van] / [Ma + Mb]
       * 
       * 4. add the normal and tangential vectors to get the final new velocities
       * Vaf = Van' + Vat
       * Vbf = Vbn' + Vbt
       */
      if (!IsTouching(another))
        return;

      var va = this.Velocity;
      var vb = another.Velocity;

      var ma = this.Mass;
      var mb = another.Mass;

      var normal = (this.Center - ((Ball)another).Center);
      if (normal == Vector2.Zero)
      {
        normal = new Vector2(1, 1);
      }
      normal.Normalize();

      var tangential = new Vector2(-1 * normal.Y, normal.X);

      var van = Vector2.Dot(va, normal);
      var vbn = Vector2.Dot(vb, normal);

      var vat = Vector2.Dot(va, tangential) * tangential;
      var vbt = Vector2.Dot(vb, tangential) * tangential;

      var van_ = (((van * (ma - mb)) + (2 * mb * vbn)) / (ma + mb)) * normal;
      var vbn_ = (((vbn * (mb - ma)) + (2 * ma * van)) / (ma + mb)) * normal;

      var vaf = van_ + vat;
      var vbf = vbn_ + vbt;

      this.Velocity = vaf;
     another.Velocity = vbf;
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

    public Enemy(Texture2D image, Vector2 initialPosition, IEnclosure bounds, int speed, int mass, List<ISprite> allSprites)
      :base(image,initialPosition,bounds,mass, allSprites)
    {
      this.Speed = speed;
      this.InitialVelocity = new Vector2(r.Next(-1, -1), r.Next(-1, -1));
      this.Velocity = this.InitialVelocity * speed;
    }

    public Vector2 InitialVelocity {get; private set;}
    

    public void Move(GameTime gametime)
    {
      foreach (var sprite in AllSprites)
      {
        if (sprite == this) // cannot collide with self.
          continue;
        CheckCollision(sprite);
      }
      CheckCollisionWithEnclosure();
      Position += Velocity * (float)gametime.ElapsedGameTime.TotalSeconds;

      // Do some boundary checking to ensure that we do not move out of the window
      float x = Position.X;
      float y = Position.Y;
      if (Position.X < 0) x = 0;
      if (Position.Y < 0) y = 0;
      if (Position.X + Image.Width > MovementBounds.Width) x = MovementBounds.Width - Image.Width;
      if (Position.Y + Image.Height > MovementBounds.Height) y = MovementBounds.Height - Image.Height;
      Position = new Vector2(x, y);
    }
  }
}

