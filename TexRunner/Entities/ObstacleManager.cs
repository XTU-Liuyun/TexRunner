using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TexRunner.Entities
{
    public class ObstacleManager : IGameEntity
    {
        private const float MIN_SPAWN_DISTANCE = 20;
        private const int MIN_OBSTACLE_DISTANCE = 100;
        private const int MAX_OBSTACLE_DISTANCE = 500;
        private double _lastSpawnScore=-1;
        private double _currentTargetDistance;
        private readonly EntityManager _entityManager;
        private readonly Trex _trex;
        private readonly ScoreBoard _scoreBoard;
        public bool IsEnabled {  get; set; }
        private readonly Random _random;
        public bool CanSpawnObstacles => IsEnabled && _scoreBoard.Score >= MIN_OBSTACLE_DISTANCE;
        public ObstacleManager(EntityManager entityManager, Trex trex,ScoreBoard scoreBoard)
        {
            _entityManager = entityManager;
            _trex = trex;
            _scoreBoard = scoreBoard;
            _random = new Random();
        }

        public int DrawOrder => 0;

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
            {
                return;
            }
            if(CanSpawnObstacles&&(_lastSpawnScore<=0||(_scoreBoard.Score-_lastSpawnScore>=_currentTargetDistance)))
            {
                _currentTargetDistance = _random.NextDouble() * (MAX_OBSTACLE_DISTANCE - MIN_OBSTACLE_DISTANCE)+ MIN_OBSTACLE_DISTANCE;
                _lastSpawnScore=_scoreBoard.Score;
                SpawnRandomObstacle();
            }
            foreach(Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                if(obstacle.Position.X<-200)
                {
                    _entityManager.RemoveEntity(obstacle);                
                }
            }

        }

        private void SpawnRandomObstacle()
        {
            
        }
    }
}
