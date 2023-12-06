﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Entities
{
    public class SkyManager : IGameEntity
    {
        private const int CLOUD_DRAW_ORDER = -1;
        private const int STAR_DRAW_ORDER = -2;
        private const int CLOUD_MIN_POS_Y = 20;
        private const int CLOUD_MAX_POS_Y = 70;

        private const int STAR_MIN_POS_Y = 10;
        private const int STAR_MAX_POS_Y = 60;

        private const int CLOUD_MIN_DISTANCE = 150;
        private const int CLOUD_MAX_DISTANCE = 200;

        private const int STAR_MIN_DISTANCE = 120;
        private const int STAR_MAX_DISTANCE = 380;

        public int DrawOrder => 0;

        private readonly EntityManager _entityManager;
        private readonly ScoreBoard _scoreBoard;
        private readonly Trex _trex;
        private Texture2D _spriteSheet;
        private double _lastCloudSpawnScore = -1;
        private int _targetCloudDistance;
        private int _targetStarDistance;
        private Random _random;

        public SkyManager(Trex trex,Texture2D spriteSheet,EntityManager entityManager,ScoreBoard scoreBoard) 
        {
            _entityManager = entityManager;
            this._scoreBoard = scoreBoard;
            _random = new Random();
            this._trex = trex;
            _spriteSheet = spriteSheet;
        } 

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            HandleCloudSpawning();
            HandleStarSpawning();
            foreach(SkyObject skyObject in _entityManager.GetEntitiesOfType<SkyObject>().Where(s=>s.Position.X<-200)) 
            {
                _entityManager.RemoveEntity(skyObject);
            }
        }
        private void HandleCloudSpawning()
        {
            IEnumerable<Cloud> clouds = _entityManager.GetEntitiesOfType<Cloud>();
            if (clouds.Count() <= 0 || (TexGunnerGame.WINDOW_WIDTH - clouds.Max(c => c.Position.X)) >= _targetCloudDistance)
            {
                _targetCloudDistance = _random.Next(CLOUD_MIN_DISTANCE, CLOUD_MAX_DISTANCE + 1);
                int posY = _random.Next(CLOUD_MIN_POS_Y, CLOUD_MAX_POS_Y + 1);
                Cloud cloud = new Cloud(_spriteSheet, _trex, new Vector2(TexGunnerGame.WINDOW_WIDTH, posY));
                cloud.DrawOrder = CLOUD_DRAW_ORDER;
                _entityManager.AddEntity(cloud);
            }
        }
        private void HandleStarSpawning()
        {
            IEnumerable<Star> stars = _entityManager.GetEntitiesOfType<Star>();
            if(stars.Count()<=0||(TexGunnerGame.WINDOW_WIDTH -stars.Max(c=> c.Position.X))>=_targetStarDistance)
            {
                _targetStarDistance = _random.Next(STAR_MIN_DISTANCE, STAR_MAX_DISTANCE + 1);
                int posY = _random.Next(STAR_MIN_POS_Y, STAR_MAX_POS_Y + 1);
                Star star = new Star(_spriteSheet, _trex, new Vector2(TexGunnerGame.WINDOW_WIDTH, posY));
                star.DrawOrder = CLOUD_DRAW_ORDER;  
                _entityManager.AddEntity(star);
            }
        }
    }
}
