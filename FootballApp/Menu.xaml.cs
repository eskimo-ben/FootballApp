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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FootballApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            Data.Load();
            InitializeComponent();
        }

        private void b_Games_Click(object sender, RoutedEventArgs e)
        {
            new Selector(new Game()).ShowDialog();
        }

        private void b_Teams_Click(object sender, RoutedEventArgs e)
        {
            new Selector(new Team()).ShowDialog();
        }

        private void b_Players_Click(object sender, RoutedEventArgs e)
        {
            new Selector(new Player()).ShowDialog();
        }

        private void b_Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To manage players within the league, click 'Players'." + Environment.NewLine +
                "To manage teams within the league, click 'Teams'" + Environment.NewLine +
                "To manage games within the league, click 'Games'", "Help");
        }
    }
}
