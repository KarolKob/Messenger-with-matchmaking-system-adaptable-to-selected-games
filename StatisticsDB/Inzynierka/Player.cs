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
        [Required]
        public int GamesTied { get; set; }
        [Required]
        public int GamesLost { get; set; }
        [Required]
        public int PointsScored { get; set; }
        [Required]
        public int PointsLost { get; set; }
        [Required]
        public double WinRate { get; set; }

        private double Count_Probablity(double s)
        {
            double result = 1.0 / (1.0 + Math.Pow(10, (SkillRating - s) / 400));
            return result;
        }

        public void Count_Skills(double result, double s)
        {
            //ta stała powinna być dobrana w devie
            int K = 32;
            WinRate = GamesWon / GamesPlayed;
            SkillRating = SkillRating + K * (result - Count_Probablity(s));
        }

        public void Count_Skills(double result, double s, int pts, int rival_pts)
        {
            //ta stała powinna być dobrana w devie
            int K = 32;
            WinRate = GamesWon / GamesPlayed;
            SkillRating = SkillRating + K * (double)(pts/rival_pts) * (result - Count_Probablity(s));
        }

        public void Update_Stats(int result, int rival_result, double rival_skill)
        {
            //ta też
            bool GoalRatio = false;
            GamesPlayed++;
            PointsScored += result;
            PointsLost += rival_result;
            if (result > rival_result)
            {
                GamesWon++;
                if (GoalRatio) Count_Skills(1, rival_skill, result, rival_result);
                else Count_Skills(1, rival_skill);
            }
            else if (rival_result > result)
            {
                GamesLost++;
                if (GoalRatio) Count_Skills(0, rival_skill, result, rival_result);
                else Count_Skills(0, rival_skill);
            }
            else
            {
                GamesTied++;
                if (GoalRatio) Count_Skills(0.5, rival_skill, result, rival_result);
                else Count_Skills(0.5, rival_skill);
            }
        }
    }
}
