using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TexRunner.Graphics;

namespace TexRunner.Entities
{
    public class GroundTile:IGameEntity
    {
        public GroundTile(float positionX, float positionY, Sprite sprite)
        {
            PositionX = positionX;
            PositionY = positionY;
            Sprite = sprite;
        }

        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public Sprite Sprite { get; }

        public int DrawOrder { get; set; }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            Sprite.Draw(spriteBatch,new Vector2(PositionX, PositionY)); 
        }
    }
}
