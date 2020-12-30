using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Inzynierka
{
    class GameConfigs
    {
        [Key]
        public int ConfigId { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public bool TeamPlays { get; set; }
        public bool TieGames { get; set; }
        public int NumberOfRanks { get; set; }
        public int KValue { get; set; }
        public bool PktsRatio { get; set; }
        public int StartRating { get; set; }
        public int MatchmakingLimit { get; set; }
        public List<int>RanksLimit { get; set; }
    }
}
