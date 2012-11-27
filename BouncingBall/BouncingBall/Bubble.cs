using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BouncingBall
{
  public class Bubble : Ball
  {
    public bool IsHit = false;

    public Bubble(Texture2D image, Vector2 initialPosition, Rectangle bounds, int speed, int mass, Color color)
      : base(image, initialPosition, bounds, mass)
    {
      this.Speed = speed;
      this.InitialVelocity = new Vector2(Ball.Random.Next(-1, -1), Ball.Random.Next(-1, -1));
      this.Velocity = this.InitialVelocity * speed;
      this.Color = color;
    }

    public Color Color { get; set; }
    public bool HasHitWall { get; set; }
    public Vector2 InitialVelocity { get; private set; }


    public override void Update(GameTime gameTime)
    {
      this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      // Do some boundary checking to ensure that we do not bounce out of the window
      float x = this.Position.X;
      float y = this.Position.Y;
      if (this.Position.X < 0) x = 0;
      if (this.Position.Y < 0) y = 0;
      if (this.Position.X + this.Image.Width > this.MovementBounds.Width) x = this.MovementBounds.Width - this.Image.Width;
      if (this.Position.Y + this.Image.Height > this.MovementBounds.Height) y = this.MovementBounds.Height - this.Image.Height;

      this.Position = new Vector2(x, y);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Image, Position, null, Color);
    }
  }
}
