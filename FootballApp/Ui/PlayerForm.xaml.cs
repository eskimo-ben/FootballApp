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
    /// Interaction logic for PlayerForm.xaml
    /// </summary>
    public partial class PlayerForm : Window
    {
        Player myPlayer;

        public PlayerForm()
        {
            if (myPlayer == null) myPlayer = new Player();
            List<Team> teamItems = new List<Team>(Data.teams);
            teamItems.Insert(0, new Team() { name = "Free Agent", teamCode = null });
            
            InitializeComponent();

            fieldTeam.SelectedValuePath = "teamCode";
            fieldTeam.DisplayMemberPath = "name";
            fieldTeam.ItemsSource = teamItems;
            fieldTeam.SelectedValue = null;
        }

        public PlayerForm(Player pPlayer) : this()
        {
            myPlayer = pPlayer;
            fieldFname.Text = pPlayer.fname;
            fieldSname.Text = pPlayer.sname;
            fieldTeam.SelectedValue = pPlayer.teamCode;
            fieldStatus.IsChecked = pPlayer.status;
        }

        public PlayerForm(string pTeamCode) : this()
        {
            fieldTeam.SelectedValue = pTeamCode;
        }

        private void b_Save_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("----------Save button clicked");
            if (Validate())
            {
                myPlayer.fname = fieldFname.Text;
                myPlayer.sname = fieldSname.Text;
                if (fieldTeam.SelectedValue != null)
                {
                    myPlayer.teamCode = fieldTeam.SelectedValue.ToString();
                }
                myPlayer.status = Convert.ToBoolean(fieldStatus.IsChecked);
                myPlayer.Save();
                this.Close();
                MessageBox.Show("Changes were made successfully!", "Success!");
            }

            bool Validate()
            {
                return vFname() && vSname();

                bool vFname()
                {
                    if (fieldFname.Text.Length > 2)
                    {
                        if (fieldFname.Text.Length < 51)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Player forename too long! The player's forename must be no more than 50 characters long.", "Invalid Data");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Player forename too short! The player's forename must be at least 3 characters long.", "Invalid Data");
                        return false;
                    }
                }

                bool vSname()
                {
                    if (fieldFname.Text.Length < 51)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Surname too long! The player's surname must be no more than 50 characters long.", "Invalid Data");
                        return false;
                    }
                }
            }
        }
    }
}
