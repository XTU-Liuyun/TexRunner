using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexRunner.Entities
{
    [Serializable]
    public class SaveState
    {
        public int Highscore {  get; set; } 
        public DateTime HighscoreDate {  get; set; }    

    }
}
