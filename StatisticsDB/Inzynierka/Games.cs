using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Inzynierka
{
    class Game
    {
        [Key]
        public int GameId { get; set; }
        [Required]
        public int Player_1 { get; set; }
        [Required]
        public int Player_2 { get; set; }
        [Required]
        public bool RankedGame { get; set; }
        [Required]
        public DateTime GameDate { get; set; }
        [Required]
        public int Score_1 { get; set; }
        [Required]
        public int Score_2 { get; set; }
        [Required]
        public bool Finished { get; set; }

        public void Add_Result(int s1, int s2)
        {
            Score_1 = s1;
            Score_2 = s2;
            Finished = true;
        }
    }
}
