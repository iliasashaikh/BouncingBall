using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BouncingBall
{

  public interface ISprite 
  {
    Vector2 Center { get; }
    Texture2D Image { get;  }
    Vector2 Position { get; set; }
    Vector2 Velocity { get; set; }
    int Speed { get; }
    int Mass { get; } 
    
    Rectangle MovementBounds { get; }
    Rectangle BoundingBox { get; }

    void Draw(SpriteBatch spriteBatch);
    void Update(GameTime gameTime);
  }
}