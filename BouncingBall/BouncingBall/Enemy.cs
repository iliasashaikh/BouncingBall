using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BouncingBall
{
  public class Enemy : Ball
  {
    private static Random r = new Random();

    public Enemy(Texture2D image, Vector2 initialPosition, Rectangle bounds, int speed, int mass)
      : base(image, initialPosition, bounds, mass)
    {
      this.Speed = speed;
      this.InitialVelocity = new Vector2(r.Next(-1, -1), r.Next(-1, -1));
      this.Velocity = this.InitialVelocity * speed;
    }

    public Vector2 InitialVelocity { get; private set; }


    public override void Update(GameTime gametime)
    {
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
