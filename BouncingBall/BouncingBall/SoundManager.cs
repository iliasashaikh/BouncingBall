using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace BouncingBall
{
  public enum Sound
  {
    Background,
    Bounce,
    Collide,
    Hurt
  }

  public interface ISoundManager
  {
    void Play(Sound sound);
  }

  public class SoundManager : ISoundManager
  {
    Song background;
    private ContentManager content;
    private SoundEffect collide;
    private SoundEffect bounce;
    private SoundEffect hurt;

    public SoundManager(ContentManager content)
    {
      this.content = content;
      this.background = content.Load<Song>(@"Sounds\background");
      this.hurt = content.Load<SoundEffect>(@"Sounds\hurt");
      this.bounce = content.Load<SoundEffect>(@"Sounds\bounce");
      this.collide = content.Load<SoundEffect>(@"Sounds\collide");
    }

    public void Play(Sound sound)
    {
        switch(sound)
        {
          case Sound.Background:
            MediaPlayer.Play(background);
            MediaPlayer.IsRepeating = true;
            break;
          case Sound.Bounce:
            bounce.Play(0.1f,0,0);
            break;
          case Sound.Collide:
            collide.Play(0.5f, 0, 0);
            break;
          case Sound.Hurt:
            hurt.Play();
            break;
        }
      
    }
  }
}
