using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BouncingBall
{
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

    public Rectangle MovementBounds { get; private set; } // ennclosure
    public int Mass { get; private set; }

    public Texture2D Image { get; private set; }
    protected float Radius { get; private set; }

    protected List<ISprite> AllSprites { get; private set; }

    public Vector2 Center
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

    public Ball(Texture2D image, Vector2 initialPosition, Rectangle bounds, int mass)
    {
      this.Image = image;
      this.Position = initialPosition;
      this.MovementBounds = bounds;
      this.Radius = image.Width / 2;
      this.Mass = mass;
    }


    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Image, this.Position, Color.White);
    }

    public virtual void Update(GameTime gameTime)
    {

    }

  }
}
