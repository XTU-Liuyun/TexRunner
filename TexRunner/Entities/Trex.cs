﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexRunner.Graphics;

namespace TexRunner.Entities
{
    public class Trex : IGameEntity
    {
        private const int TREX_IDLE_BACKGOURND_SPRITE_POS_X=40;
        private const int TREX_IDLE_BACKGOURND_SPRITE_POS_Y=0;
        public const int TREX_DEFAULT_SPRITE_POS_X = 848;
        public const int TREX_DEFAULT_SPRITE_POS_Y = 0;
        public const int TREX_DEFAULT_SPRITE_WIDTH = 44;
        public const int TREX_DEFAULT_SPRITE_HEIGHT = 47;
        public const int WINDOW_WIDTH = 600;
        public const int WINDOW_HEIGHT = 150;
        private const float BLINK_ANIMATION_RANDOM_MIN = 2f;
        private const float BLINK_ANIMATION_RANDOM_MAX = 10f;
        private const float BLINK_ANIMATION_EYE_CLOSE_TIME = 0.5f;
        private Sprite _idleSprite;
        private Sprite _idleBlinkSprite;

        private SpriteAnimation _blinkAnimation;
        private Random _random;
        public TrexState State { get; private set; }    
        public Vector2 Position { get; set; }
        public bool IsAlive {  get; private set; }
        public float Speed {  get; private set; }   
        public int DrawOrder {get; set;}
        public Trex(Texture2D spriteSheet,Vector2 position)
        {

            Position = position;
            _idleSprite = new Sprite(spriteSheet, TREX_IDLE_BACKGOURND_SPRITE_POS_X, TREX_IDLE_BACKGOURND_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
            State = TrexState.Idle;
            _random = new Random();
            _idleSprite=new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
            _idleBlinkSprite=new Sprite(spriteSheet,
                TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_WIDTH,
                TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
            CreateBlinkAnimation();
            _blinkAnimation.Play();

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(State== TrexState.Idle)
            {
                _idleSprite.Draw(spriteBatch, this.Position);
                _blinkAnimation.Draw(spriteBatch, this.Position);
            }
            
        }

        public void Update(GameTime gameTime)
        {
            if (State == TrexState.Idle)
            {
                _blinkAnimation.Update(gameTime);
                if(!_blinkAnimation.IsPlaying)
                {
                    CreateBlinkAnimation();
                    _blinkAnimation.Play();
                }
                _blinkAnimation.Update(gameTime);
            }
        }
        private void CreateBlinkAnimation()
        {
            _blinkAnimation = new SpriteAnimation
            { loop = true };
            

            double blinkTimeStamp = BLINK_ANIMATION_RANDOM_MIN + _random.NextDouble() * (BLINK_ANIMATION_RANDOM_MAX - BLINK_ANIMATION_RANDOM_MIN);
            Debug.WriteLine("blink:"+blinkTimeStamp);
            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlinkSprite,(float)blinkTimeStamp);
            _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimeStamp+ BLINK_ANIMATION_EYE_CLOSE_TIME);
            
        }
    }
}
