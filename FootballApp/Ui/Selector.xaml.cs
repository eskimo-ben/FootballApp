using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FootballApp
{
    /// <summary>
    /// Interaction logic for GamesSelect.xaml
    /// </summary>
    public partial class Selector : Window
    {
        private Selector()
        {
            InitializeComponent();
        }

        public Selector(Game pGame):this()
        {
            Title = "Game Selector";
            foreach (Game game in Data.games)
            {
                MainList.Items.Add(String.Format("{0} {1}",
                    Data.teams.Single(team => team.teamCode == game.homeTeamCode),
                    Data.teams.Single(team => team.teamCode == game.awayTeamCode)
                ));
            }
        }

        public Selector(Player pPlayer) : this()
        {
            Title = "Player Selector";
            foreach(Player player in Data.players)
            {
                MainList.Items.Add(player.fullName);
            }
        }

        public Selector(Team pTeam) : this()
        {
            Title = "Team Selector";
            foreach (Team team in Data.teams)
            {
                MainList.Items.Add(team.name);
            }
        }
    }
}
