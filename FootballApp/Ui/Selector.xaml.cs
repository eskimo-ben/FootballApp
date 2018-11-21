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
                MainList.Items.Add(String.Format("{0} vs {1}",
                    Data.teams.Single(team => team.teamCode == game.homeTeamCode).name,
                    Data.teams.Single(team => team.teamCode == game.awayTeamCode).name
                ));
            }
        }

        public Selector(Player pPlayer) : this()
        {
            Title = "Player Selector";

            ComboList.DisplayMemberPath = "name";//These are coming up blank for some reason
            ComboList.SelectedValuePath = "teamCode";


            /*
            ComboList.Items.Add("All Players");
            ComboList.Items.Add("Free Agents");
            */

            Team tempAllPlayers = new Team() { name = "All Players", teamCode = "ALL" };
            Team tempFreeAgents = new Team() { name = "Free Agents", teamCode = "FREE" };

            ComboList.Items.Add(tempAllPlayers);
            ComboList.Items.Add(tempFreeAgents);

            foreach (Team team in Data.teams)
            {
                ComboList.Items.Add(team);
            }

            AllPlayers();

            MainList.DisplayMemberPath = "fullName";
            MainList.SelectedValuePath = "playerCode";

            void AllPlayers()
            {
                foreach (Player player in Data.players)
                {
                    MainList.Items.Add(player);
                }
            }

            void FreeAgents()
            {

            }

            void TeamPlayers(string pTeamCode)
            {

            }

            void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                switch (ComboList.SelectedValue)
                {
                    case "ALL":
                        AllPlayers();
                            break;
                    case "FREE":
                        FreeAgents();
                        break;
                    default:
                        //TeamPlayers();
                        break;
                }
            }

            //ComboList.SelectionChanged


        }

    public Selector(Team pTeam) : this()
        {
            Title = "Team Selector";
            foreach (Team team in Data.teams)
            {
                MainList.Items.Add(team.name);
            }
        }

        void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboList.SelectedValue)
            {
                case "ALL":
                    //AllPlayers();
                    break;
                case "FREE":
                    //FreeAgents();
                    break;
                default:
                    //TeamPlayers();
                    break;
            }
        }
    }
}
