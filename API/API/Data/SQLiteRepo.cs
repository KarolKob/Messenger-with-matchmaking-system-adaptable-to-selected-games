using API.DTO;
using API.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class SQLiteRepo : ILiteRepo
    {
        private readonly SQLiteContext _context;

        //dostęp do bazy danych poprzez dependency injection
        public SQLiteRepo(SQLiteContext context)
        {
            _context = context;
        }

        public async Task<int> AddPlayer(Player player)
        {
            await _context.PlayersDB.AddAsync(new Player { NickName = player.NickName, SkillRating = 0.0, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, GamesTied = 0, WinRate = 0.0 });
            return await _context.SaveChangesAsync();
            
        }

        public async Task<Player> GetPlayerInfo(string nick)
        {
            return await _context.PlayersDB.FirstOrDefaultAsync(p => p.NickName == nick);
        }

        public IEnumerable<Player> MatchPlayers()
        {
            throw new NotImplementedException();
        }

        public void RemovePlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}