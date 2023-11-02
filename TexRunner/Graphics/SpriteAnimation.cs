﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Graphics
{
    public class SpriteAnimation
    {
        private List<SpriteAnimationFrame>_frames=new List<SpriteAnimationFrame>();
        public SpriteAnimationFrame this[int index]
        {
            get
            {
                return GetFrame(index);
            }

        }
        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                return _frames.Where(f=>f.TimeStamp<=PlaybackProgress).OrderBy(f=>f.TimeStamp).LastOrDefault();
            }
        }
        public bool IsPlaying {  get; private set; }    
        public float PlaybackProgress { get; private set; }
        public void AddFrame(Sprite sprite, float timeStamp) 
        {
            SpriteAnimationFrame frame=new SpriteAnimationFrame(sprite,timeStamp);
            _frames.Add(frame); 
            SpriteAnimation anim=new SpriteAnimation();
        }
        public void Update(GameTime gameTime)
        {
            if(IsPlaying)
            {
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        public void Play()
        {
            IsPlaying = true;
        }
        public void Stop()
        {
            IsPlaying = false;
            PlaybackProgress= 0f;
        }
        public SpriteAnimationFrame GetFrame(int index)
        {
            if(index<0||index>=_frames.Count) throw new ArgumentOutOfRangeException(nameof(index),"A frame with index"+index+"does not exist in this animation.");    
            return _frames[index];
        }
    }
}
