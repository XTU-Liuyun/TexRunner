using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Entities
{
    public class SkyManager : IGameEntity
    {
        private const int CLOUD_MIN_POS_Y = 20;
        private const int CLOUD_MAX_POS_Y = 70;

        private const int CLOUD_MIN_DISTANCE = 150;
        private const int CLOUD_MAX_DISTANCE = 200;

        public int DrawOrder => 0;

        private readonly EntityManager _entityManager;
        private readonly ScoreBoard _scoreBoard;
        private readonly Trex _trex;
        private Texture2D _spriteSheet;
        private double _lastCloudSpawnScore = -1;
        private int _targetCloudDistance;
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
            IEnumerable<Cloud> clouds=_entityManager.GetEntitiesOfType<Cloud>();    
            if(clouds.Count()<=0||(TexGunnerGame.WINDOW_WIDTH-clouds.Max(c=>c.Position.X))>=_targetCloudDistance)
            {
                _targetCloudDistance = _random.Next(CLOUD_MIN_DISTANCE, CLOUD_MAX_DISTANCE + 1);
                int posY = _random.Next(CLOUD_MIN_POS_Y, CLOUD_MAX_POS_Y + 1);
                Cloud cloud = new Cloud(_spriteSheet, _trex, new Vector2(TexGunnerGame.WINDOW_WIDTH, posY));
                _entityManager.AddEntity(cloud);    
            }
            foreach(Cloud cloud in clouds.Where(c=>c.Position.X<-200)) 
            {
                _entityManager.RemoveEntity(cloud); 
            }
        }
    }
}
