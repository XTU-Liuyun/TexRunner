using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Entities
{
    public interface ICollidable
    {
        Rectangle CollisionBox {  get; }
    }
}
