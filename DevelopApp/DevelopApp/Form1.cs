using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System;
using System.IO;

namespace DevelopApp
{
    public partial class Form1 : Form
    {
        private readonly List<Rank> ranks;
        public Form1()
        {
            InitializeComponent();
            ranks = new List<Rank>();
            Rank bronze = new Rank("bronze", 0, 999);
            Rank silver = new Rank("silver", 1000, 1999);
            Rank gold = new Rank("gold", 2000, -1);
            ranks.Add(bronze);
            ranks.Add(silver);
            ranks.Add(gold);
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //ranks
            List<int> ranklimits = new List<int>();
            foreach (var rank in ranks)
            {
                if (rank.MaxRating > 0) ranklimits.Add(rank.MaxRating);
            }

            GameConfig new_game = new GameConfig
            {
                //general settings
                Name = nameBox.Text,
                Description = descBox.Text,
                NumberOfPlayers = Int32.Parse(mPlayersBox.Text),
                Server = serverBox.Text,
                TeamPlays = teamsCheck.Checked,
                TieGames = tieCheck.Checked,
                AvgTime = Int32.Parse(avgTime.Text),

                //advanced settings
                KValue = KvalueBar.Value,
                StartRating = SratingBar.Value,
                PktsRatio = pRatioCheck.Checked,
                MatchmakingLimit = matchmakBar.Value,

                //ranks
                RanksLimit = JsonSerializer.Serialize(ranklimits)
            };

            using (var dbContext = new ConfigContext())
            {
                dbContext.games.Add(new_game);
                dbContext.SaveChanges();
            }
        }

        private void AddRank(string name)
        {
            Rank rank = new Rank(name, ranks[ranks.Count - 1].MinRating + 1000, -1);
            ranks[ranks.Count - 1].MaxRating = ranks[ranks.Count - 1].MinRating + 999;
            ranks.Add(rank);
            UpdateRanks();
        }

        private void RemoveRank()
        {
            ranks.RemoveAt(ranks.Count-1);
            ranks[ranks.Count - 1].MaxRating = -1;
            UpdateRanks();
        }

        private void UpdateRanks()
        {
            foreach(var rank in ranks)
            {
                if (rank.Name == "gold")
                {
                    goldMinBox.Text = rank.MinRating.ToString();
                    if (rank.MaxRating < 0) goldMaxBox.Text = "MAX";
                    else goldMaxBox.Text = rank.MaxRating.ToString();
                }
                else if (rank.Name == "platin")
                {
                    platinMinBox.Text = rank.MinRating.ToString();
                    if (rank.MaxRating < 0) PlatinMaxBox.Text = "MAX";
                    else PlatinMaxBox.Text = rank.MaxRating.ToString();
                }
                else if (rank.Name == "diamond")
                {
                    diamondMinBox.Text = rank.MinRating.ToString();
                    if (rank.MaxRating < 0) diamondMaxBox.Text = "MAX";
                    else diamondMaxBox.Text = rank.MaxRating.ToString();
                }
                else if (rank.Name=="platin")
                {
                    eliteMinBox.Text = rank.MinRating.ToString();
                    if (rank.MaxRating < 0) eliteMaxBox.Text = "MAX";
                    else eliteMaxBox.Text = rank.MaxRating.ToString();
                }
            }
        }
        private void PlatinCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (PlatinCheck.Checked)
            {
                DiamondCheck.Enabled = true;
                platinPanel.Enabled = true;
                AddRank("platin");
            }
            else
            {
                DiamondCheck.Enabled = false;
                platinMinBox.Text = String.Empty;
                PlatinMaxBox.Text = String.Empty;
                platinPanel.Enabled = false;
                RemoveRank();
            }
        }

        private void DiamondCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (DiamondCheck.Checked)
            {
                EliteCheck.Enabled = true;
                diamondPanel.Enabled = true;
                PlatinCheck.Enabled = false;
                AddRank("diamond");
            }
            else
            {
                diamondMinBox.Text = String.Empty;
                diamondMaxBox.Text = String.Empty;
                EliteCheck.Enabled = false;
                diamondPanel.Enabled = false;
                PlatinCheck.Enabled = true;
                RemoveRank();
            }
        }

        private void EliteCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (EliteCheck.Checked)
            {
                elitePanel.Enabled = true;
                DiamondCheck.Enabled = false;
                AddRank("elite");
            }
            else
            {
                eliteMinBox.Text = String.Empty;
                eliteMaxBox.Text = String.Empty;
                DiamondCheck.Enabled = true;
                elitePanel.Enabled = false;
                RemoveRank();
            }
        }

        private void KvalueBar_Scroll(object sender, EventArgs e)
        {
            kvalueLabel.Text = KvalueBar.Value.ToString();
        }

        private void SratingBar_Scroll(object sender, EventArgs e)
        {
            startRLabel.Text = SratingBar.Value.ToString();
        }

        private void MatchmakBar_Scroll(object sender, EventArgs e)
        {
            MMlabel.Text = matchmakBar.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists("Games"))
            {
                File.Delete("Games");
            }

            using (var dbContext = new ConfigContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }
        }
    }
}
