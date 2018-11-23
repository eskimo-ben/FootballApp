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
            if (nameField.Text.Length < 3)
            {
                MessageBox.Show("Team name too short! You must give your team an appropriate name before editing it's players! The team's name must be at least 3 characters long.", "Invalid Data");
            }
            else if (nameField.Text.Length > 50)
            {
                MessageBox.Show("Team name too long! You must give your team an appropriate name before editing it's players! The team's name must be no more than 50 characters long.", "Invalid Data");
            }
            else
            {
                myTeam.name = nameField.Text;
                Console.WriteLine("-----------------------------myTeam.name = nameField.Text = " + nameField.Text);
                if (myTeam.Save())
                    new Selector(myTeam.teamCode).ShowDialog();
                else
                    MessageBox.Show("Failed to save! There was an error whilst trying to save your team!", "Error");
            }
        }

        private void b_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                myTeam.name = nameField.Text;
                myTeam.venue = venueField.Text;
                myTeam.Save();
                this.Close();
                MessageBox.Show("Changes were made successfully!", "Success!");
            }

            bool Validate()
            {
                return vName() && vVenue();

                bool vName()
                {
                    if (nameField.Text.Length > 2)
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
                        MessageBox.Show("Team name too short! The team's name must be at least 3 characters long.", "Invalid Data");
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
