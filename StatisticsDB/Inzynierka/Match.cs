﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLiteNetExtensions.Attributes;
using System.Text.Json;

namespace Inzynierka
{
    class MatchTeam
    {
        [Key]
        public int GameId { get; set; }
        public string Teams { get; set; }
        public bool RankedGame { get; set; }
        [Required]
        public DateTime GameDate { get; set; }
        [Required]
        public string Scores { get; set; }
        public bool Finished { get; set; }

        public void Add_Result(List<int> scores)
        {
            Scores = JsonSerializer.Serialize(scores);
            Finished = true;
        }

        private double Count_Probablity(double rival_skill ,double skill)
        {
            double result = 1.0 / (1.0 + Math.Pow(10, (skill - rival_skill) / 400));
            return result;
        }

        public double Count_Skills(double result, double skillrival, double skill)
        {
            int K = 32;
            return K * (result - Count_Probablity(skillrival, skill));
        }

        public List<double> CountRanking(List<int> scores, List<double> rankings)
        {
            List<double>new_rankings = new List<double>();
            for (int i = 0; i < scores.Count; i++)
            {
                double new_skill = 0;
                for (int j=0; j < scores.Count; j++)
                {
                    if (j!= i)
                    {
                        if (scores[i] > scores[j]) new_skill += Count_Skills(1, rankings[j], rankings[i]);
                        if (scores[i] < scores[j]) new_skill += Count_Skills(0, rankings[j], rankings[i]);
                        else new_skill += Count_Skills(0.5, rankings[j], rankings[i]);
                    }
                }
                new_rankings.Add(new_skill / scores.Count);
            }
            return new_rankings;
        }
    }

    class MatchSolo
    {
        [Key]
        public int GameId { get; set; }
        public string Players { get; set; }
        public bool RankedGame { get; set; }
        [Required]
        public DateTime GameDate { get; set; }
        [Required]
        public string Scores { get; set; }
        public bool Finished { get; set; }

        public void Add_Result(List<int> scores)
        {
            Scores = JsonSerializer.Serialize(scores);
            Finished = true;
        }

        private double Count_Probablity(double rival_skill, double skill)
        {
            double result = 1.0 / (1.0 + Math.Pow(10, (skill - rival_skill) / 400));
            return result;
        }

        public double Count_Skills(double result, double skillrival, double skill)
        {
            int K = 32;
            return K * (result - Count_Probablity(skillrival, skill));
        }

        public List<double> CountRanking(List<int> scores, List<double> rankings)
        {
            List<double> new_rankings = new List<double>();
            for (int i = 0; i < scores.Count; i++)
            {
                double new_skill = 0;
                for (int j = 0; j < scores.Count; j++)
                {
                    if (j != i)
                    {
                        if (scores[i] > scores[j]) new_skill += Count_Skills(1, rankings[j], rankings[i]);
                        if (scores[i] < scores[j]) new_skill += Count_Skills(0, rankings[j], rankings[i]);
                        else new_skill += Count_Skills(0.5, rankings[j], rankings[i]);
                    }
                }
                new_rankings.Add(new_skill / scores.Count);
            }
            return new_rankings;
        }
    }
}
