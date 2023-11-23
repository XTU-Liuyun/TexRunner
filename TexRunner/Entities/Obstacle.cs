using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexRunner.Graphics;

namespace TexRunner.Entities
{
    public class CactusGroup : Obstacle
    {

        public enum GroupSize
        {
            Small,
            medium,
            Large
        }
        private const int SMALL_CACTUS_SPRITE_HEIGHT = 36;
        private const int SMALL_CACTUS_SPRITE_WIDTH = 17;

        private const int SMALL_CACTUS_TEXTURE_POS_X = 228;
        private const int SMALL_CACTUS_TEXTURE_POS_Y = 0;
        public override Rectangle CollisionBox => throw new global::System.NotImplementedException();
        public GroupSize Size { get; }
        public bool IsLarge {  get;  }  
        public CactusGroup(bool isLarge,GroupSize size,Trex trex,Vector2 position):base(trex,position)
        {
            IsLarge = isLarge;
            Size = size;
        }
        private Sprite GenerateSprite()
        {
            Sprite sprite = null;
            if(!IsLarge)
            {
                int offsetX = 0;
                int width = SMALL_CACTUS_SPRITE_WIDTH;
                if(Size==GroupSize.Small)
                {
                    offsetX = 0;
                    width = SMALL_CACTUS_SPRITE_WIDTH;
                }
                else if(Size==GroupSize.medium)
                {
                    offsetX = 1;
                    width = SMALL_CACTUS_SPRITE_WIDTH * 2;
                }
                else
                {
                    offsetX = 3;
                    width = SMALL_CACTUS_SPRITE_WIDTH * 3;
                }
            }
            else
            {

            }
            return sprite;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new global::System.NotImplementedException();
        }
    }
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
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        

        public void Update(GameTime gameTime)
        {
            float posX = Position.X - _trex.Speed*(float)gameTime.ElapsedGameTime.TotalSeconds;
            Position=new Vector2 (posX, Position.Y);
        }
    }
}
