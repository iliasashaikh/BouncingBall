using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BouncingBall
{
  public interface ICollisionManager
  {
    void Update(GameTime gameTime);
  }

  public class CollisionManager
  {
    SoundManager soundManager;
    IList<ISprite> sprites;
    Rectangle bounds;

    public CollisionManager(List<ISprite> sprites, Rectangle bounds, SoundManager soundManager)
    {
      this.soundManager = soundManager;
      this.sprites = sprites;
      this.bounds = bounds;
    }

    List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

    protected internal void Update(GameTime gameTime)
    {
      pairs.Clear();
      // check the collisons amongst all the sprites
      for (int i = 0; i < sprites.Count; i++)
      {
        for (int j = 0; j < sprites.Count; j++)
        {
          if (!PairsTested(i, j) && i!=j)
          {
            CheckCollision(sprites[i], sprites[j]);
            pairs.Add(new Tuple<int,int>(i, j));
          }
        }
      }

      // now check for collisons with the walls
      for (int i = 0; i < sprites.Count; i++)
      {
        ISprite sprite = sprites[i];
        CheckCollision(sprites[i], bounds);
        sprite.Position += sprite.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        // Do some boundary checking to ensure that we do not move out of the window
        float x = sprite.Position.X;
        float y = sprite.Position.Y;
        if (sprite.Position.X < 0) x = 0;
        if (sprite.Position.Y < 0) y = 0;
        if (sprite.Position.X + sprite.Image.Width > sprite.MovementBounds.Width) x = sprite.MovementBounds.Width - sprite.Image.Width;
        if (sprite.Position.Y + sprite.Image.Height > sprite.MovementBounds.Height) y = sprite.MovementBounds.Height - sprite.Image.Height;
        
        sprite.Position = new Vector2(x, y);
      }
    }

    private bool PairsTested(int i, int j)
    {
      foreach (var pair in pairs)
      {
        if ((pair.Item1 == i && pair.Item2 == j) ||
            (pair.Item1 == j && pair.Item2 == i))

          return true;
      }

      return false;
    }

    private bool IsTouching(ISprite sprite1, ISprite sprite2)
    {
      var distance = (sprite1.Center - sprite2.Center).Length();
      var r1 = sprite1.BoundingBox.Width / 2;
      var r2 = sprite2.BoundingBox.Width / 2;

      var touching =  (distance <= (r1 + r2));
      if (touching)
        soundManager.Play(Sound.Collide);

      return touching;
    }

    private void CheckCollision(ISprite sprite1, ISprite sprite2)
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
      if (!IsTouching(sprite1, sprite2))
        return;

      var va = sprite1.Velocity;
      var vb = sprite2.Velocity;

      var ma = sprite1.Mass;
      var mb = sprite2.Mass;

      var normal = (sprite1.Center - sprite2.Center);
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

      sprite1.Velocity = vaf;
      sprite2.Velocity = vbf;
    }

    private void CheckCollision(ISprite sprite, Rectangle bounds)
    {
      if (IsTouchingEnclosure(sprite, bounds))
      {
        var normal = GetNormalWithWall(sprite.BoundingBox);
        // formula: V = (-2 x (V.N) x N) + V
        var velocity = sprite.Velocity;
        velocity = -2 * (Vector2.Dot(velocity, normal) * normal) + velocity;
        sprite.Velocity = velocity;
      }
    }
    private bool IsTouchingEnclosure(ISprite sprite, Rectangle bounds)
    {
      // cannot use Rectangle.contains, it misses the case where the we are just touching, i.e. x = 0 etc.
      if (sprite.BoundingBox.Left <= bounds.Left ||
          sprite.BoundingBox.Right >= bounds.Right ||
          sprite.BoundingBox.Bottom >= bounds.Bottom ||
          sprite.BoundingBox.Top <= bounds.Top)
      {
        soundManager.Play(Sound.Bounce);
        return true;
      }
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
}
