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
        [Required]
        public int GamesLost { get; set; }
        [Required]
        public int PointsScored { get; set; }
        public int PointsLost { get; set; }
        [Required]
        public double WinRate { get; set; }

        public string Rank;

        public void Update_Stats(double newrtaing)
        {
            GamesPlayed++;
            SkillRating += newrtaing;
        }
    }
}
