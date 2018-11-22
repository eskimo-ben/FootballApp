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
    /// Interaction logic for TeamForm.xaml
    /// </summary>
    public partial class TeamForm : Window
    {
        Team myTeam;
        public TeamForm()
        {
            if (myTeam == null) myTeam = new Team();
            InitializeComponent();
        }

        public TeamForm(Team pTeam):this()
        {
            myTeam = pTeam;
            nameField.Text = pTeam.name;
            venueField.Text = pTeam.venue;

        }

        private void b_EditPlayers_Click(object sender, RoutedEventArgs e)
        {
            new Selector(myTeam.teamCode).Show();
        }

        private void b_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                myTeam.name = nameField.Text;
                myTeam.venue = venueField.Text;
                myTeam.Save();
                MessageBox.Show("Changes were made successfully!", "Success!");
            }

            bool Validate()
            {
                return vName() && vVenue();

                bool vName()
                {
                    if (nameField.Text.Length > 3)
                    {
                        if(nameField.Text.Length < 51)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Team name too long! The team's name must be no more than 50 characters long.", "Invalid Data");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Team name too short! The team's name must be at least 4 characters long.", "Invalid Data");
                        return false;
                    }
                }

                bool vVenue()
                {
                    if (nameField.Text.Length < 51)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Venue name too long! The name of the team's venue must be no more than 50 characters long.", "Invalid Data");
                        return false;
                    }
                }
            }
        }
    }
}
