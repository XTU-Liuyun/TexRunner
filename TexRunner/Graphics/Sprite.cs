using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Input;



namespace TexRunner.Graphics
{
    public class Sprite
    {
        
        public Texture2D texture {  get; private set; } 
        public int X {  get; set; }
        public int Y { get; set; }
        public int Width { get; set; }  
        public int Height { get; set; }
        public Color TintColor {  get; set; }   =Color.White;
        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public void Draw(SpriteBatch spriteBatch,Vector2 position) 
        {
            spriteBatch.Draw(texture, position, new Rectangle(X, Y, Width, Height), TintColor);
        }
    }
}
