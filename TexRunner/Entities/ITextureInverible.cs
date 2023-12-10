using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Entities
{
    public interface ITextureInverible:IGameEntity
    {
        void UpdateTexture(Texture2D newTexture);
    }
}
