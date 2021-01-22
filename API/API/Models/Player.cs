using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Player
    {

        [Key]
        public int PlayerId { get; set; }
        [Required]
        [MaxLength(16, ErrorMessage = "Maximum number of characters that can be entered is 16!")]
        public string NickName { get; set; }
        [Required]
        public double SkillRating { get; set; }

        [Required]
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesTied { get; set; }
        public int GamesLost { get; set; }
        [Required]
        public double WinRate { get; set; }
        public string Rank { get; set; }



        private void CountWinRate()
        {
            WinRate = (GamesWon / GamesPlayed) * 100;
        }

        public void Update_Stats(double newrtaing, int result)
        {
            GamesPlayed++;
            SkillRating += Math.Round(newrtaing, 1);
            if (SkillRating < 0) SkillRating = 0;
            if (result == 2) GamesLost++;
            else if (result == 0) GamesTied++;
            else GamesWon++;
            CountWinRate();
        }
    }
}
