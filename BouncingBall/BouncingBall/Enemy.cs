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
    }
  }
}
