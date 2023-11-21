using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexRunner.Graphics;

namespace TexRunner.Entities
{
    public abstract class Obstacle : IGameEntity
    {
        private Trex _trex;
        protected Sprite _sprite;
        public abstract Rectangle CollisionBox {  get; }

        public int DrawOrder {get; set;}
        public Vector2 Position { get; private set;}


        protected Obstacle(Trex trex,Vector2 position)
        {
            _trex = trex;
            Position= position; 

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite?.Draw(spriteBatch, Position);
        }

        public void Update(GameTime gameTime)
        {
            float posX = Position.X - _trex.Speed*(float)gameTime.ElapsedGameTime.TotalSeconds;
            Position=new Vector2 (posX, Position.Y);
        }
    }
}
