using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace Inzynierka
{
    class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        [MaxLength(16)]
        public string NickName { get; set; }
        [Required]
        public double SkillRating { get; set; }
        [Required]
        public int GamesPlayed { get; set; }
        [Required]
        public int GamesWon { get; set; }
        public int GamesTied { get; set; }
        public int GamesLost { get; set; }
        [Required]
        public int PointsLost { get; set; }
        [Required]
        public double WinRate { get; set; }

        public string Rank;

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
