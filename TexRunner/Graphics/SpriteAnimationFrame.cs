using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Graphics
{
    public class SpriteAnimationFrame
    {
        private Sprite _sprite;
        private readonly float _timeStamp;
        public Sprite Sprite 
        {
            get
            {
                return _sprite;
            }
            set
            {
                if(value== null)
                {
                    throw new ArgumentNullException("value","The Sprite Can Not Be NULL.");
                }
                _sprite = value;
            }
        }  
        public float TimeStamp { get; }
        public SpriteAnimationFrame(Sprite sprite,float timeStamp) 
        {
            Sprite = sprite;
            this.TimeStamp = timeStamp;
        }
    }
}
