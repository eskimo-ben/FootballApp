﻿using System;
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

            ComboList.DisplayMemberPath = "name";
            ComboList.SelectedValuePath = "teamCode";

            MainList.DisplayMemberPath = "gameName";
            MainList.SelectedValuePath = "gameCode";
            MainList.ItemsSource = Data.games;

            
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
            isTeam = true;
            Title = "Team Selector";
            foreach (Team team in Data.teams)
            {
                MainList.Items.Add(team.name);
            }
        }
        
        void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isPlayer)
            {
                switch (ComboList.SelectedValue)
                {
                    case "ALL":
                        MainList.ItemsSource = Data.players;
                        break;
                    case "FREE":
                        MainList.ItemsSource = Player.FreeAgents();
                        break;
                    default:
                        MainList.ItemsSource = Player.TeamPlayers(ComboList.SelectedValue.ToString());
                        break;
                }
            }

            if (isTeam)
            {

            }

            if (isGame)
            {

            }
            
        }

        private void b_Add_Click(object sender, RoutedEventArgs e)
        {
            if (isPlayer)
            {
                new PlayerForm().Show();
            }
        }

        private void b_Edit_Click(object sender, RoutedEventArgs e)
        {
            if(Player.checkByCode(MainList.SelectedValue.ToString()))
                new PlayerForm(Player.getByCode(MainList.SelectedValue.ToString())).Show();
        }
    }
}
