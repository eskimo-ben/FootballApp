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
    /// Interaction logic for GameForm.xaml
    /// </summary>
    public partial class GameForm : Window
    {
        Game myGame;

        public GameForm()
        {
            myGame = new Game();
            InitializeComponent();

            fieldHomeTeam.ItemsSource = Team.sortByName();
            fieldAwayTeam.ItemsSource = Team.sortByName();

            fieldHomeTeam.SelectedValuePath = "teamCode";
            fieldHomeTeam.DisplayMemberPath = "name";

            fieldAwayTeam.SelectedValuePath = "teamCode";
            fieldAwayTeam.DisplayMemberPath = "name";

            fieldHomeScore.Text = "0";
            fieldAwayScore.Text = "0";
        }

        public GameForm(Game pGame) : this()
        {
            myGame = Game.getByGameCode(pGame.gameCode);

            fieldHomeTeam.SelectedValue = pGame.homeTeamCode;
            fieldAwayTeam.SelectedValue = pGame.awayTeamCode;
            fieldDatePicker.SelectedDate = pGame.date;
            fieldHomeScore.Text = pGame.homeScore.ToString();
            fieldAwayScore.Text = pGame.awayScore.ToString();
        }

        private void b_Save_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("----------Save button clicked");
            if (Validate())
            {
                myGame.homeTeamCode = fieldHomeTeam.SelectedValue.ToString();
                myGame.awayTeamCode = fieldAwayTeam.SelectedValue.ToString();
                
                if(fieldDatePicker.SelectedDate != null)
                    myGame.date = Convert.ToDateTime(fieldDatePicker.SelectedDate);
                   

                if(fieldHomeScore.Text != "")
                    myGame.homeScore = Convert.ToInt16(fieldHomeScore.Text);
                
                if(fieldAwayScore.Text != "")
                    myGame.awayScore = Convert.ToInt16(fieldAwayScore.Text);

                myGame.Save();
                this.Close();
                MessageBox.Show("Changes were made successfully!", "Success!");
            }

            bool Validate()
            {
                if(fieldHomeTeam.SelectedValue == null)
                {
                    MessageBox.Show("No home team selected! The game must have a home team in order to be saved.", "Invalid Data");
                    return false;
                }
                    

                if (fieldAwayTeam.SelectedValue == null)
                {
                    MessageBox.Show("No away team selected! The game must have an away team in order to be saved.", "Invalid Data");
                    return false;
                }
                    

                if(fieldDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("No date selected! The game must have a date in order to be saved.", "Invalid Data");
                    return false;
                }
                    
                return true;
            }
        }
    }
}
