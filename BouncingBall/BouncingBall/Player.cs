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
    private Vector2 userVelocity;
    private Dictionary<Keys, Vector2> moveLookup;

    public Player(Texture2D image, Vector2 initialPosition, Rectangle bounds, int speed, int mass, int cols, int rows, int frameRate)
      : base(image, initialPosition, bounds, mass, cols, rows, frameRate)
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
      userVelocity = Vector2.Zero;
      // if player has hit a wall, we use the computed velocity from the collision manager, do not compute velocities from the keyboard, otherwise we will keep going outside the bounds
      if (!HasHitWall)
      {
        var keyboardState = Keyboard.GetState();
        if (keyboardState.GetPressedKeys().Count() > 0)
        {
          foreach (var key in moveLookup.Keys)
          {
            if (keyboardState.IsKeyDown(key))
              userVelocity += moveLookup[key];
          }

          Velocity = Speed * userVelocity;
        }
      }
      Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      
      // reset hashitwall
      HasHitWall = false;
    }

    public override void Draw(SpriteBatch spriteBatch, float rotation = 0, Color? color = null)
    {
      if (userVelocity == Vector2.Zero)
        rotation = 0;
      
      if (userVelocity.X >= 1)
        rotation = 180;

      if (userVelocity.X == 0)
        rotation = 0;

      base.Draw(spriteBatch,rotation,null);
    }
  }
}
