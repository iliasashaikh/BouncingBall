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
    private CollisionManager collisionManager = null;
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    List<ISprite> sprites = new List<ISprite>();

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

      var windowWidth = graphics.GraphicsDevice.Viewport.Width;
      var windowHeight = graphics.GraphicsDevice.Viewport.Height;
      
      var enemyGlassBall = new Enemy(Content.Load<Texture2D>("glassBall"), new Vector2(0,0), graphics.GraphicsDevice.Viewport.Bounds, 150, 3);

      var enemyHollowBall1 = new Enemy(Content.Load<Texture2D>("hollowBall"), new Vector2(windowWidth / 2, windowHeight / 2), graphics.GraphicsDevice.Viewport.Bounds, 300, 1);
      var enemyHollowBall2 = new Enemy(Content.Load<Texture2D>("hollowBall"), new Vector2(windowWidth / 2, 0), graphics.GraphicsDevice.Viewport.Bounds, 300, 3);
      var enemyHollowBall3 = new Enemy(Content.Load<Texture2D>("hollowBall"), new Vector2(0, windowHeight / 2), graphics.GraphicsDevice.Viewport.Bounds, 300, 4);
      
      var playerImage = Content.Load<Texture2D>("playerBall");
      var playerBall = new Player(playerImage, new Vector2(windowWidth / 2, windowHeight - playerImage.Height - 10), graphics.GraphicsDevice.Viewport.Bounds, 150, 10);

      sprites.Add(enemyGlassBall);
      sprites.Add(enemyHollowBall1);
      sprites.Add(enemyHollowBall2);
      sprites.Add(enemyHollowBall3);
      sprites.Add(playerBall);

      collisionManager = new CollisionManager(sprites, graphics.GraphicsDevice.Viewport.Bounds, soundManager);

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
      foreach (var sprite in sprites)
      {
        sprite.Update(gameTime);
      }
      collisionManager.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();
      foreach (var sprite in sprites)
      {
        sprite.Draw(spriteBatch);
      }
      spriteBatch.End();

      
      base.Draw(gameTime);
    }
  }
}
