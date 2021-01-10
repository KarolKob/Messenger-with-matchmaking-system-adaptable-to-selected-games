using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopApp
{
    public partial class Form1 : Form
    {
        private List<Rank> ranks;
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

        private void button1_Click(object sender, EventArgs e)
        {
            GameConfig config = new GameConfig();

            //general settings
            config.Name = nameBox.Text;
            config.Description = descBox.Text;
            config.NumberOfPlayers = Int32.Parse(mPlayersBox.Text);
            config.Server = serverBox.Text;
            config.TeamPlays = teamsCheck.Checked;
            config.TieGames = tieCheck.Checked;
            config.AvgTime = Int32.Parse(avgTime.Text);

            //advanced settings
            config.KValue = KvalueBar.Value;
            config.StartRating = SratingBar.Value;
            config.PktsRatio = pRatioCheck.Checked;
            config.MatchmakingLimit = matchmakBar.Value;

            //ranks

        }

        private void addRank(string name)
        {
            Rank rank = new Rank(name, ranks[ranks.Count - 1].MinRating + 1000, -1);
            ranks[ranks.Count - 1].MaxRating = ranks[ranks.Count - 1].MinRating + 999;
            ranks.Add(rank);
            UpdateRanks();
        }

        private void removeRank()
        {
            ranks.RemoveAt(ranks.Count-1);
            ranks[ranks.Count - 1].MaxRating = -1;
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
                else
                {
                    eliteMinBox.Text = rank.MinRating.ToString();
                    if (rank.MaxRating < 0) eliteMaxBox.Text = "MAX";
                    else eliteMaxBox.Text = rank.MaxRating.ToString();
                }
            }
        }

        private void DiamondCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (DiamondCheck.Checked)
            {
                EliteCheck.Enabled = true;
                diamondPanel.Enabled = true;
                addRank("diamond");
            }
            else
            {
                diamondMinBox.Text = String.Empty;
                diamondMaxBox.Text = String.Empty;
                EliteCheck.Enabled = false;
                diamondPanel.Enabled = false;
                removeRank();
            }
        }

        private void EliteCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (EliteCheck.Checked)
            {
                elitePanel.Enabled = true;
                addRank("elite");
            }
            else
            {
                eliteMinBox.Text = String.Empty;
                eliteMaxBox.Text = String.Empty;
                elitePanel.Enabled = false;
                removeRank();
            }
        }

        private void PlatinCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (PlatinCheck.Checked)
            {
                DiamondCheck.Enabled = true;
                platinPanel.Enabled = true;
                addRank("platin");
            }
            else
            {
                DiamondCheck.Enabled = false;
                platinMinBox.Text = String.Empty;
                PlatinMaxBox.Text = String.Empty;
                platinPanel.Enabled = false;
                removeRank();
            }
        }
    }
}
