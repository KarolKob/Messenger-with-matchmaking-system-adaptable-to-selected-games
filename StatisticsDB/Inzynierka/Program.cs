using System;
using System.IO;
using System.Linq;

namespace Inzynierka
{
    class Program
    {
        public static void Create_DataBase(string dbName)
        {
            if (File.Exists(dbName))
            {
                File.Delete(dbName);
            }
            using (var dbContext = new StatContext())
            {
                //Ensure database is created
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }
            Console.ReadLine();
        }

        public static void Add_Player(int id, string nick)
        {
            using (var dbContext = new StatContext())
            {
                dbContext.Players.Add(new Player { PlayerId = id, NickName = nick, SkillRating = 0.0, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, GamesTied = 0, WinRate = 0.0 });
                dbContext.SaveChanges();
            }
        }

        public static void Add_Game()
        {
            using (var dbContext = new StatContext())
            {
                dbContext.Games.Add(new Game { GameId = 1, Player_1 = 1, Player_2 = 2, GameDate = DateTime.Today, Score_1 = 0, Score_2 = 0, RankedGame = false, Finished = false });
                dbContext.SaveChanges();
            }
        }

        public static void Add_Result(int game_id)
        {
            using (var dbContext = new StatContext())
            {
                var game = dbContext.Games.First(g => g.GameId == game_id);
                game.Add_Result(1, 2);
                var player_1 = dbContext.Players.First(p => p.PlayerId == game.Player_1);
                var player_2 = dbContext.Players.First(p => p.PlayerId == game.Player_2);
                double skill_1 = player_1.SkillRating;
                double skill_2 = player_2.SkillRating;
                player_1.Update_Stats(1, 2, skill_2); 
                player_2.Update_Stats(2, 1, skill_1);
                dbContext.SaveChanges();
            }
        }
        static void Main(string[] args)
        {
            //string dbName = "Statistics.db";
            //Create_DataBase(dbName);
            //Add_Player();
            //Add_Game();
            //Add_Result();
            Console.WriteLine("OK");
        }
    }
}
