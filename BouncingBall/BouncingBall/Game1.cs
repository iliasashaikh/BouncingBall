using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BouncingBall
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    private Texture2D background;
    private CollisionManager collisionManager = null;
    private BubbleManager bubbleManager;
    GraphicsDeviceManager graphics;
    private Player playerBall;
    SpriteBatch spriteBatch;

    //List<ISprite> sprites = new List<ISprite>();

    private SoundManager soundManager;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      //this.Window.AllowUserResizing = true;

      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      soundManager = new SoundManager(Content);
      soundManager.Play(Sound.Background);

      bubbleManager = new BubbleManager(Content, graphics.GraphicsDevice.Viewport.Bounds, 15);

      var windowWidth = graphics.GraphicsDevice.Viewport.Width;
      var windowHeight = graphics.GraphicsDevice.Viewport.Height;
      
      background = Content.Load<Texture2D>(@"images\background");

            
      var playerImage = Content.Load<Texture2D>(@"images\birdsheet");
      playerBall = new Player(playerImage, new Vector2(windowWidth / 2, windowHeight - playerImage.Height - 10), graphics.GraphicsDevice.Viewport.Bounds, 0, 10, 2,2,14);

      //bubbleManager.MakeBubble(Color.Yellow);
      //bubbleManager.MakeBubble(Color.White);
      //bubbleManager.MakeBubble(Color.Black);
      //bubbleManager.MakeBubble(Color.Brown);

      var sprites = new List<ISprite>();
      sprites.Add(playerBall);
      sprites.AddRange(bubbleManager.Bubbles);
      collisionManager = new CollisionManager(sprites, graphics.GraphicsDevice.Viewport.Bounds, soundManager,bubbleManager);

    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      playerBall.UpdateCore(gameTime);
      collisionManager.Update(gameTime);
      bubbleManager.Update(gameTime);

      // var bubbles = bubbleManager.Bubbles;
      // check if the player has touched any enemies, if hit the enemies disappear
      for (int i = 0; i < bubbleManager.Bubbles.Count; i++)
      {
        var bubble = bubbleManager.Bubbles[i];
        if (bubble.IsHit)
        {
          bubbleManager.Pop(bubble);
          collisionManager.RemoveCollidable(bubble);
        }
        //if (bubble.HasHitWall)
        //{
        //  var newBubble = bubbleManager.MakeBubble(bubble.Color);
        //  collisionManager.AddCollidable(newBubble);
        //  bubble.HasHitWall = false;
        //  newBubble.HasHitWall = false;
        //}
      }

      
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();

      spriteBatch.Draw(background, graphics.GraphicsDevice.Viewport.Bounds, Color.White);
      playerBall.Draw(spriteBatch);
      bubbleManager.Draw(spriteBatch);

      spriteBatch.End();

      
      base.Draw(gameTime);
    }
  }
}
