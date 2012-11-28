using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BouncingBall
{

  public class BubbleManager
  {
    private Rectangle bounds;
    public List<Bubble> Bubbles;
    private ContentManager content;
    public int MaxBubbles { get; set; }

    public BubbleManager(ContentManager content, Rectangle bounds, int maxBubbles)
    {
      Bubbles = new List<Bubble>();
      this.content = content;
      this.bounds = bounds;
      this.MaxBubbles = maxBubbles;
      //var greenBall = new Enemy(content.Load<Texture2D>(@"images\green"), new Vector2(0, 0), graphics.GraphicsDevice.Viewport.Bounds, 150, 3);
      //var peacockBall = new Enemy(content.Load<Texture2D>(@"images\peacock"), new Vector2(windowWidth / 2, windowHeight / 2), graphics.GraphicsDevice.Viewport.Bounds, 300, 1);
      //var pinkBall = new Enemy(content.Load<Texture2D>(@"images\pink"), new Vector2(windowWidth / 2, 0), graphics.GraphicsDevice.Viewport.Bounds, 300, 3);
      //var yellowBall = new Enemy(content.Load<Texture2D>(@"images\yellow"), new Vector2(0, windowHeight / 2), graphics.GraphicsDevice.Viewport.Bounds, 300, 4);
    }

    public Bubble MakeBubble(Color bubbleColor, Vector2? position = null, int speed = 0, int mass = 0)
    {
      if (Bubbles.Count >= MaxBubbles)
        return null;

      var image = content.Load<Texture2D>(@"images\bubble");
      var pos = (Vector2)(position ?? GetRandomPosition(image));
      speed = speed == 0 ? 50 : speed;
      mass = mass == 0 ? 1 : mass;

      var bubble = new Bubble(image, pos, bounds, speed, mass, bubbleColor);
      Bubbles.Add(bubble);
      return bubble;
    }

    private Vector2 GetRandomPosition(Texture2D image)
    {
      return new Vector2(Ball.Random.Next(0, bounds.Width - image.Width - 10), Ball.Random.Next(0, bounds.Height - image.Height - 10));
    }

    public void Pop(Bubble bubble)
    {
      Bubbles.Remove(bubble);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var bubble in Bubbles)
      {
        bubble.Draw(spriteBatch);
      }
    }

    public void Update(GameTime gameTime)
    {
      foreach (var bubble in Bubbles)
      {
        bubble.Update(gameTime);
      }
    }
  }
}
