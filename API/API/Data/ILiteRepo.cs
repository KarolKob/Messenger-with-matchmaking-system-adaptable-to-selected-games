using API.DTO;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public interface ILiteRepo
    {
        bool SaveChanges();

        IEnumerable<Player> MatchPlayers();
        Task<Player> GetPlayerInfo(string nick);
        Task<int> AddPlayer(Player player);
        void UpdatePlayer(Player player);
        void RemovePlayer(Player player);
    }
}
