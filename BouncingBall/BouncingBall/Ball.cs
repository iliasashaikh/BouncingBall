using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BouncingBall
{
  public abstract class Ball : ISprite
  {
    private int cols;
    private int currentFrame = 0;
    protected internal static Random Random = new Random();

    public Vector2 Position { get; set; }
    public int Speed { get; set; }
    public bool HasHitWall { get; set; }
    private int rows;
    private int totalFrames;
    private double frameRate;

    private Vector2 velocity;
    private double timeSinceLastframe;
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

    public Rectangle MovementBounds { get; private set; } // ennclosure
    public int Mass { get; private set; }

    public Texture2D Image { get; private set; }
    protected float Radius { get; private set; }

    protected List<ISprite> AllSprites { get; private set; }

    public Vector2 Center
    {
      get
      {
        var x = Position.X + ImageWidth / 2;
        var y = Position.Y + ImageHeight / 2;

        return new Vector2(x, y);
      }
    }

    private int imageHeight;
    public int ImageHeight
    {
      get
      {
        return imageHeight;
      }
    }

    private int imageWidth;
    public int ImageWidth
    {
      get
      {
        return imageWidth;
      }
    }

    public Rectangle BoundingBox // bounding rectangle for the image
    {
      get
      {
        var r = new Rectangle((int)Position.X, (int)Position.Y, this.ImageWidth, this.ImageHeight);
        return r;
      }
    }

    public Ball(Texture2D image, Vector2 initialPosition, Rectangle bounds, int mass)
      :this(image,initialPosition,bounds,mass,1,1,1)
    {

    }

    public Ball(Texture2D image, Vector2 initialPosition, Rectangle bounds, int mass, int cols, int rows, double frameRate)
    {
      this.Image = image;
      this.Position = initialPosition;
      this.MovementBounds = bounds;
      this.Mass = mass;
      this.cols = cols;
      this.rows = rows;
      this.frameRate = frameRate;
      this.totalFrames = rows * cols;
      this.imageWidth = image.Width / cols;
      this.imageHeight = image.Height / rows;

      this.Radius = imageWidth / 2;
    }


    //public void Draw(SpriteBatch spriteBatch)
    //{
    //  this.Draw(spriteBatch, 0, Color.White);
    //}

    public virtual void Draw(SpriteBatch spriteBatch, float rotation=0, Color? color = null)
    {
      var clr = color == null ? Color.White : color.Value;
      var row = currentFrame / cols;
      var col = currentFrame % cols;

      var destRect = new Rectangle((int)Position.X, (int)Position.Y, imageWidth, imageHeight);
      var sourceRect = new Rectangle(col * imageWidth, row * imageHeight, imageWidth, imageHeight);
      //spriteBatch.Draw(this.Image, destRect, sourceRect, clr, rotation, new Vector2(col * imageWidth, row * imageHeight), SpriteEffects.None, 0);
      spriteBatch.Draw(this.Image, destRect, sourceRect, clr, rotation, Vector2.Zero, SpriteEffects.None, 0);
    }

    public void UpdateCore(GameTime gameTime)
    {
      UpdateAnimation(gameTime);
      Update(gameTime);
    }

    private void UpdateAnimation(GameTime gameTime)
    {
      timeSinceLastframe += gameTime.ElapsedGameTime.TotalSeconds;
      var timeBetweenFrames = 1 / frameRate;
      if (timeSinceLastframe >= timeBetweenFrames)
      {
        currentFrame++;
        timeSinceLastframe = 0;
      }
      if (currentFrame == totalFrames)
        currentFrame = 0;
    }

    public abstract void Update(GameTime gameTime);




  }

}
