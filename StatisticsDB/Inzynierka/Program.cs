﻿using System;
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
        public static int Get_Version(int game_id)
        {
            int ver;
            using (var dbContext = new ConfigContext())
            {
                var game = dbContext.games.First(g => g.ConfigId == game_id);
                if (game.TeamPlays) ver = 1;
                else ver = 2;
            }
            return ver;
        }
        public static void Create_DataBase(string dbName, int game_id)
        {
            if (File.Exists(dbName))
            {
                File.Delete(dbName);
            }
            //get matchmaking version
            int ver = Get_Version(game_id);

            switch (ver)
            {
                case 1:
                    //create database
                    using (var dbContext = new StatContextTeam())
                    {
                        dbContext.Database.EnsureCreated();
                        dbContext.SaveChanges();
                    }
                    break;

                case 2:
                    //create database
                    using (var dbContext = new StatContextSolo())
                    {
                        dbContext.Database.EnsureCreated();
                        dbContext.SaveChanges();
                    }
                    break;
            }
        }
        public static void Add_Player(string nick, int game_id)
        {
            int ver = Get_Version(game_id);

            switch (ver)
            {
                case 1:
                    using (var dbContext = new StatContextTeam())
                    {
                        using (var config = new ConfigContext())
                        {
                            var game = config.games.First(g => g.ConfigId == game_id);
                            if (game.TieGames) dbContext.Players.Add(new Player { NickName = nick, SkillRating = game.StartRating, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, GamesTied = 0, WinRate = 0.0 });
                            else dbContext.Players.Add(new Player { NickName = nick, SkillRating = game.StartRating, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, WinRate = 0.0 });
                        }
                        //check if tie games are allowed
                        dbContext.SaveChanges();
                    }
                    break;

                case 2:
                    using (var dbContext = new StatContextSolo())
                    {
                        using (var config = new ConfigContext())
                        {
                            var game = config.games.First(g => g.ConfigId == game_id);
                            //check if tie games are allowed
                            if (game.TieGames) dbContext.Players.Add(new Player { NickName = nick, SkillRating = game.StartRating, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, GamesTied = 0, WinRate = 0.0 });
                            else dbContext.Players.Add(new Player { NickName = nick, SkillRating = game.StartRating, GamesPlayed = 0, GamesWon = 0, GamesLost = 0, WinRate = 0.0 });
                        }
                        dbContext.SaveChanges();
                    }
                    break;
            }
        }

        public static void Add_Team(List<int> players)
        {
            using (var dbContext = new StatContextTeam())
            {
                dbContext.Teams.Add(new Team { PlayersID = JsonSerializer.Serialize(players) });
                dbContext.SaveChanges();
            }
        }

        public static void Add_Game_Team(List<int> teams, bool ranked)
        {
            using (var dbContext = new StatContextTeam())
            {
                List<int> scores = new List<int>();
                foreach (var p in teams) scores.Add(0);
                dbContext.TeamGames.Add(new MatchTeam { Teams = JsonSerializer.Serialize(teams), GameDate = DateTime.Today, Scores = JsonSerializer.Serialize(scores), RankedGame = ranked, Finished = false });
                dbContext.SaveChanges();
            }
        }

        public static void Add_Game_Solo(List<int> players, bool ranked)
        {
            using (var dbContext = new StatContextSolo())
            {
                List<int> scores = new List<int>();
                foreach (var p in players) scores.Add(0);
                dbContext.SoloGames.Add(new MatchSolo { Players = JsonSerializer.Serialize(players), GameDate = DateTime.Today, Scores = JsonSerializer.Serialize(scores), RankedGame = ranked, Finished = false });
                dbContext.SaveChanges();
            }
        }

        public static void Add_Result_Team(int game_id, List<int>scores, int config_id)
        {
            using (var dbContext = new StatContextTeam())
            {
                var game = dbContext.TeamGames.First(g => g.GameId == game_id);
                game.Add_Result(scores);
                if (game.RankedGame)
                {
                    List<double> skills = new List<double>();
                    List<int> tids = JsonSerializer.Deserialize<List<int>>(game.Teams);
                    //fill skills list

                    foreach(var tid in tids)
                    {
                        var team = dbContext.Teams.First(t => t.TeamID == tid);
                        double avg_skills = 0;
                        List<int> team_players = JsonSerializer.Deserialize <List<int>>(team.PlayersID);
                        foreach(var pid in team_players)
                        {
                            var player = dbContext.Players.First(p => p.PlayerId == pid);
                            avg_skills += player.SkillRating;
                        }
                        avg_skills = avg_skills / team_players.Count;
                        skills.Add(avg_skills);
                    }
                    List<double> ranking_updates = new List<double>();
                    using (var config = new ConfigContext())
                    {
                        var game_config = config.games.First(g => g.ConfigId == config_id);
                        ranking_updates = game.CountRanking(scores, skills, game_config.KValue);
                    }
                    //count new skills
                    
                    int i = 0;
                    List<int> highest = new List<int>();
                    int hresult = scores[0];
                    highest.Add(0);

                    for (int j = 1; j < scores.Count; j++)
                    {
                        if (scores[j] > hresult)
                        {
                            highest.Clear();
                            highest.Add(j);
                            hresult = scores[j];
                        }
                        else if (scores[j] == hresult) highest.Add(j);
                    }
                    //update all stats
                    foreach (var tid in tids)
                    {
                        var team = dbContext.Teams.First(t => t.TeamID == tid);
                        List<int> team_players = JsonSerializer.Deserialize<List<int>>(team.PlayersID);

                        if (highest.Contains(i))
                        {
                            foreach (var pid in team_players)
                            {
                                var player = dbContext.Players.First(p => p.PlayerId == pid);
                                player.Update_Stats(ranking_updates[i], 1);
                            }
                            i++;
                        }
                        else
                        {
                            foreach (var pid in team_players)
                            {
                                var player = dbContext.Players.First(p => p.PlayerId == pid);
                                player.Update_Stats(ranking_updates[i], 2);
                            }
                            i++;
                        }
                    }
                }
                dbContext.SaveChanges();
            }
        }

        public static void Add_Result_Solo(int game_id, List<int> scores, int config_id)
        {
            using (var dbContext = new StatContextSolo())
            {
                var game = dbContext.SoloGames.First(g => g.GameId == game_id);
                game.Add_Result(scores);
                if (game.RankedGame)
                {
                    List<double> skills = new List<double>();
                    List<int> pids = JsonSerializer.Deserialize<List<int>>(game.Players);
                    //fill skills list
                    foreach (var pid in pids)
                    {
                        var player = dbContext.Players.First(p => p.PlayerId == pid);
                        skills.Add(player.SkillRating);
                    }
                    List<double> ranking_updates = new List<double>();

                    using (var config = new ConfigContext())
                    {
                        var game_config = config.games.First(g => g.ConfigId == config_id);
                        ranking_updates = game.CountRanking(scores, skills, game_config.KValue);
                    }

                    //count new skills
                    int i = 0;

                    //update ratings and stats
                    List<int> highest = new List<int>();
                    int hresult = scores[0];
                    highest.Add(0);

                    for (int j = 1; j < scores.Count; j++)
                    {
                        if (scores[j] > hresult)
                        {
                            highest.Clear();
                            highest.Add(j);
                            hresult = scores[j];
                        }
                        else if (scores[j] == hresult) highest.Add(j);
                    }

                    foreach (var pid in pids)
                    {
                        var player = dbContext.Players.First(p => p.PlayerId == pid);

                        if (highest.Contains(i)) player.Update_Stats(ranking_updates[i++], 1);
                        else player.Update_Stats(ranking_updates[i++], 2);
                    }

                }
                dbContext.SaveChanges();
            }
        }

        public static int Find_Opponent_Solo(int id, List<Lobby>rooms, int skilllimit)
        {
            int result = -1;
            using (var dbContext = new StatContextSolo())
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

        public static int Find_Opponent_Team(int id, List<Lobby> rooms, int skilllimit)
        {
            int result = -1;
            using (var dbContext = new StatContextTeam())
            {
                var player = dbContext.Players.First(p => p.PlayerId == id);
                foreach (var r in rooms)
                {
                    double skillMean = 0;
                    double skillSum = 0;
                    foreach (var p in r.players) skillSum += p.SkillRating;
                    skillMean = skillSum / r.players.Count;
                    if (skillMean - player.SkillRating < skilllimit)
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
            string dbName = "C:/DataBase/Statistics.db";
            Create_DataBase(dbName, 1);
            
            Add_Player("roman", 1);
            Add_Player("arek", 1);
            Add_Player("krzychu", 1);
            Add_Player("luj", 1);
            
            
            //solo test
            List<int> players = new List<int>();
            players.Add(1);
            players.Add(2);
            players.Add(3);

            List<int> scores = new List<int>();
            scores.Add(10);
            scores.Add(5);
            scores.Add(5);

            Add_Game_Solo(players, true);
            Add_Result_Solo(1, scores, 1);

            //team test
            /*
            List<int> teamA = new List<int>();
            List<int> teamB = new List<int>();

            teamA.Add(1);
            teamA.Add(2);
            Add_Team(teamA);

            teamB.Add(3);
            teamB.Add(4);
            Add_Team(teamB);

            List<int> teams = new List<int>();
            teams.Add(1);
            teams.Add(2);

            Add_Game_Team(teams, true);

            List<int> scores = new List<int>();
            scores.Add(10);
            scores.Add(5);

            Add_Result_Team(1, scores, 1);
            */

            Console.WriteLine("OK");
        }
    }
}
