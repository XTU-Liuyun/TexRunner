using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Entities
{
    public class ScoreBoard : IGameEntity
    {
        private const int TEXTURE_COORDS_NUMBER_X = 655;
        private const int TEXTURE_COORDS_NUMBER_Y = 0;
        private const int TEXTURE_COORDS_NUMBER_WIDTH = 10;
        private const int TEXTURE_COORDS_NUMBER_HEIGHT = 13;
        private const byte NUMBER_DIGITS_TO_DRAW = 5;
        private Texture2D _texture;

       
        public double Score { get; set; }   
        public int DisplayScore=>(int)Math.Floor((double)Score);    
        public int HighScore {  get; set; }

        public int DrawOrder => 100;
        public Vector2 Position { get; set; }   
        public ScoreBoard(Texture2D texture,Vector2 position)
        {
            _texture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int[] scoreDigits = SplitDigits(DisplayScore);
            float posX = Position.X;
            foreach (int scoreDigit in scoreDigits)
            {
                Rectangle textureCoords=GetDigitTextureCoords(scoreDigit);
                Vector2 screenPos=new Vector2(posX,Position.Y);
                spriteBatch.Draw(_texture, screenPos, textureCoords, Color.White);
                posX += TEXTURE_COORDS_NUMBER_WIDTH;
            }
        }

        public void Update(GameTime gameTime)
        {
            
        }
        private int[] SplitDigits(int input)
        {
            string inputStr=input.ToString().PadLeft(NUMBER_DIGITS_TO_DRAW,'0');
            int[] result=new int[inputStr.Length];
            for (int i =0; i < result.Length; i++) 
            {
                result[i] = (int)char.GetNumericValue(inputStr[i]);
            }
            return result;
        } 
        private Rectangle GetDigitTextureCoords(int digit)
        {
            if(digit<0||digit>9)
            {
                throw new ArgumentOutOfRangeException("digit", "The value of digit must be betwenn 0 and 9.");

            }
            int posX = TEXTURE_COORDS_NUMBER_X + digit * TEXTURE_COORDS_NUMBER_WIDTH;
            int posY = TEXTURE_COORDS_NUMBER_Y;
            return new Rectangle(posX, posY,TEXTURE_COORDS_NUMBER_WIDTH,TEXTURE_COORDS_NUMBER_HEIGHT);
        }
    }
}
