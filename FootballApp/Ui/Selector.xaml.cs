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
        private bool isGame;
        private bool isPlayer;
        private bool isTeam;

        private Selector()
        {
            InitializeComponent();
        }

        public Selector(Game pGame):this()
        {
            isGame = true;
            Title = "Game Selector";

            /*ComboList.DisplayMemberPath = "name";
            ComboList.SelectedValuePath = "teamCode";*/

            MainList.ItemsSource = Data.games;
            MainList.DisplayMemberPath = "gameName";
            MainList.SelectedValuePath = "gameCode";
            

            
        }

        public Selector(Player pPlayer) : this()
        {
            isPlayer = true;
            Title = "Player Selector";

            ComboList.DisplayMemberPath = "name";
            ComboList.SelectedValuePath = "teamCode";
            List<Team> ComboListItems = new List<Team>(Data.teams);
            ComboListItems.Insert(0, new Team() { name = "All Players", teamCode = "ALL" });
            ComboListItems.Insert(1, new Team() { name = "Free Agents", teamCode = "FREE" });
            ComboList.ItemsSource = ComboListItems;
            ComboList.SelectedIndex = 0; 


            MainList.ItemsSource = Data.players;
            MainList.DisplayMemberPath = "fullName";
            MainList.SelectedValuePath = "playerCode";
        }

        public Selector(Team pTeam) : this()
        {

            ComboList.ItemsSource = new List<string>() { "Sort by: Name", "Sort By: Venue name" };
            ComboList.SelectedIndex = 0;
            isTeam = true;
            Title = "Team Selector";


            //MainList.ItemsSource = Data.teams;
            MainList.ItemsSource = Team.sortByName();
            MainList.DisplayMemberPath = "name";
            MainList.SelectedValuePath = "teamCode";
        }

        public Selector(string pTeamCode) : this(new Player())
        {
            ComboList.SelectedValue = pTeamCode;
            MainList.ItemsSource = Player.getByTeamCode(pTeamCode);
        }
        
        void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshMain();
            
        }

        void RefreshMain()
        {
            MainList.ItemsSource = null;

            if (isPlayer)
            {
                switch (ComboList.SelectedValue)
                {
                    case "ALL":
                        MainList.ItemsSource = Data.players;
                        break;
                    case "FREE":
                        MainList.ItemsSource = Player.getFreeAgents();
                        break;
                    default:
                        MainList.ItemsSource = Player.getByTeamCode(ComboList.SelectedValue.ToString());
                        break;
                }
            }

            if (isTeam)
            {
                switch (ComboList.SelectedValue.ToString())
                {
                    case "Sort by: Name":
                        MainList.ItemsSource = Team.sortByName();
                        break;
                    case "Sort By: Venue name":
                        MainList.ItemsSource = Team.sortByVenue();
                        break;
                    default:
                        MainList.ItemsSource = Team.sortByName();
                        break;
                }
            }

            if (isGame)
            {
                MainList.ItemsSource = Data.games;
            }
        }

        private void b_Add_Click(object sender, RoutedEventArgs e)
        {
            if (isPlayer)
            {
                if(ComboList.SelectedValue.ToString() != "FREE" && ComboList.SelectedValue.ToString() != "ALL")
                {
                    new PlayerForm(ComboList.SelectedValue.ToString()).ShowDialog();
                }
                else new PlayerForm().ShowDialog();

                RefreshMain();
            }

            if (isTeam)
            {
                new TeamForm().ShowDialog();
                RefreshMain();
            }

            if (isGame)
            {
                new GameForm().ShowDialog();
                RefreshMain();
            }
        }

        private void b_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (isPlayer)
            {
                if (Player.checkByPlayerCode(MainList.SelectedValue.ToString()))
                    new PlayerForm(Player.getByPlayerCode(MainList.SelectedValue.ToString())).ShowDialog();

                RefreshMain();
            }
            if (isTeam)
            {
                if (Team.checkByTeamCode(MainList.SelectedValue.ToString()))
                    new TeamForm(Team.getByTeamCode(MainList.SelectedValue.ToString())).ShowDialog();

                RefreshMain();
            }

            if (isGame)
            {
                if (Game.checkByGameCode(MainList.SelectedValue.ToString()))
                    new GameForm(Game.getByGameCode(MainList.SelectedValue.ToString())).ShowDialog();

                RefreshMain();
            }
            
        }

        private void b_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (isPlayer)
            {
                if (Player.checkByPlayerCode(MainList.SelectedValue.ToString()))
                    Player.getByPlayerCode(MainList.SelectedValue.ToString()).Delete();

                RefreshMain();
                MessageBox.Show("Player was deleted successfully!");
            }
            if (isTeam)
            {
                if (Team.checkByTeamCode(MainList.SelectedValue.ToString()))
                    Team.getByTeamCode(MainList.SelectedValue.ToString()).Delete();

                RefreshMain();
                MessageBox.Show("Team was deleted successfully! Team's players are now 'Free Agents'");
            }

            if (isGame)
            {

            }
        }
    }
}
