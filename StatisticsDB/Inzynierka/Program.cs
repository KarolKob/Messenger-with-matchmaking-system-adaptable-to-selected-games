using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace Inzynierka
{
    class Lobby
    {
        public int LobbyID;
        public List<Player> players;
    }

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
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }
        }
        public static void Add_Player(string nick, GameConfigs config)
        {
            using (var dbContext = new StatContext())
            {
                if(config.TieGames) dbContext.Players.Add(new Player {NickName = nick, SkillRating = 0.0, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, GamesTied = 0, WinRate = 0.0 });
                else dbContext.Players.Add(new Player { NickName = nick, SkillRating = 0.0, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, WinRate = 0.0 });
                dbContext.SaveChanges();
            }
        }

        public static void Add_Game(List<int> teams, bool ranked)
        {
            using (var dbContext = new StatContext())
            {
                List<int> scores = new List<int>();
                foreach (var p in teams) scores.Add(0);
                dbContext.Games.Add(new Match { Teams = JsonSerializer.Serialize(teams), GameDate = DateTime.Today, Scores = JsonSerializer.Serialize(scores), RankedGame = ranked, Finished = false });
                dbContext.SaveChanges();
            }
        }

        public static void Add_Result(int game_id, List<int>scores, GameConfigs config)
        {
            using (var dbContext = new StatContext())
            {
                var game = dbContext.Games.First(g => g.GameId == game_id);
                game.Add_Result(scores);
                if (game.RankedGame)
                {
                    List<double> skills = new List<double>();
                    List<int> pids = JsonSerializer.Deserialize<List<int>>(game.Teams);
                    //fill skills list
                    foreach (var pid in pids)
                    {
                        var player = dbContext.Players.First(p => p.PlayerId == pid);
                        skills.Add(player.SkillRating);
                    }

                    //count new skills
                    List<double> ranking_updates = game.CountRanking(scores, skills);
                    int i = 0;
                    //update all stats
                    foreach (var pid in pids)
                    {
                        var player = dbContext.Players.First(p => p.PlayerId == pid);

                        if(config.TieGames) player.Update_Stats(ranking_updates[i++]);
                        else player.Update_Stats(ranking_updates[i++]);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        public static int Find_Opponent(int id, List<Lobby>rooms, int skilllimit)
        {
            int result = -1;
            using (var dbContext = new StatContext())
            {
                var player = dbContext.Players.First(p => p.PlayerId == id);
                foreach(var r in rooms)
                {
                    double skillMean = 0;
                    double skillSum = 0;
                    foreach (var p in r.players) skillSum += p.SkillRating;
                    skillMean = skillSum / r.players.Count;
                    if(skillMean-player.SkillRating < skilllimit)
                    {
                        result = r.LobbyID;
                        break;
                    }
                }
            }
            return result;
        }
        static void Main(string[] args)
        {
            GameConfigs config = new GameConfigs();
            config.TieGames = true;
            string dbName = "Statistics.db";
            Create_DataBase(dbName);
            Add_Player("roman", config);
            //Add_Game();
            //Add_Result();
            Console.WriteLine("OK");
        }
    }
}
