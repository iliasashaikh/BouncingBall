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
    public Color Color { get; set; }
    public Vector2 InitialVelocity { get; private set; }
    public bool HasCollided { get; set; }

    public Bubble(Texture2D image, Vector2 initialPosition, Rectangle bounds, int speed, int mass, Color color)
      : base(image, initialPosition, bounds, mass)
    {
      this.Speed = speed;
      this.InitialVelocity = new Vector2(Ball.Random.Next(-1, -1), Ball.Random.Next(-1, -1));
      this.Velocity = this.InitialVelocity * speed;
      this.Color = color;
    }

    public override void Update(GameTime gameTime)
    {
      if (!HasCollided)
       this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      // Do some boundary checking to ensure that we do not bounce out of the window
      float x = this.Position.X;
      float y = this.Position.Y;
      if (this.Position.X < 0) x = 0;
      if (this.Position.Y < 0) y = 0;
      if (this.Position.X + this.ImageWidth > this.MovementBounds.Width) x = this.MovementBounds.Width - this.Image.Width;
      if (this.Position.Y + this.ImageHeight > this.MovementBounds.Height) y = this.MovementBounds.Height - this.Image.Height;

      this.Position = new Vector2(x, y);
      this.HasCollided = false;
    }

    public override void Draw(SpriteBatch spriteBatch, float rotation = 0, Color? color = null)
    {
      //spriteBatch.Draw(Image, Position, null, Color);
      base.Draw(spriteBatch, 0, Color);
    }

  }
}
