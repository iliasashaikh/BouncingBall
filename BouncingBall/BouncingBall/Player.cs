using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BouncingBall
{
  public class Player : Ball
  {
    private Dictionary<Keys, Vector2> moveLookup;

    public Player(Texture2D image, Vector2 initialPosition, Rectangle bounds, int speed, int mass)
      : base(image, initialPosition, bounds, mass)
    {
      moveLookup = new Dictionary<Keys, Vector2> 
        {
          {Keys.Left, new Vector2(-1, 0)},
          {Keys.Right, new Vector2(1, 0)},
          {Keys.Up, new Vector2(0, -1)},
          {Keys.Down, new Vector2(0, 1)}
        };
      this.Speed = speed;
    }

    public override void Update(GameTime gameTime)
    {
      var direction = Vector2.Zero;
            
      var keyboardState = Keyboard.GetState();
      if (keyboardState.GetPressedKeys().Count() > 0)
      {
        foreach (var key in moveLookup.Keys)
        {
          if (keyboardState.IsKeyDown(key))
            direction += moveLookup[key];
        }

        Velocity = Speed * direction;
      }

      Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

    }
  }
}
