using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BenCsv;

namespace FootballApp
{
    static class Data
    {
        public static string OnlineConnStr { get; } = String.Format(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename= {0}Data\FootballDb.mdf;Integrated Security=True", AppDomain.CurrentDomain.BaseDirectory);
        public static List<Player> players = new List<Player>();
        public static List<Team> teams = new List<Team>();
        public static List<Game> games = new List<Game>();

        public static bool Load()
        {
            using(SqlConnection conn = new SqlConnection(OnlineConnStr))
            {
                conn.Open();

                return SelectPlayers() && SelectTeams() && SelectGames();

                bool SelectPlayers()
                {
                    int playerCount = 0;
                    try
                    {
                        SqlCommand selectPlayers = new SqlCommand("SELECT * FROM t_Players", conn);

                        using (SqlDataReader reader = selectPlayers.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player tempPlayer = new Player(reader[0].ToString());
                                if (!reader.IsDBNull(1))
                                {
                                    tempPlayer.teamCode = reader[1].ToString().Trim();
                                }
                                tempPlayer.fname = reader[2].ToString().Trim();
                                tempPlayer.sname = reader[3].ToString().Trim();
                                tempPlayer.status = Convert.ToBoolean(reader[4]);
                                players.Add(tempPlayer);
                                playerCount++;
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("---------- LOADING ERROR! There was an exception thrown whilst loading players from the database.");
                        return false;
                    }
                    Console.WriteLine("---------- {0} player(s) loaded successfully.", playerCount);
                    return true;
                }

                bool SelectTeams()
                {
                    int teamCount = 0;
                    try
                    {
                        SqlCommand selectTeam = new SqlCommand("SELECT * FROM t_Teams", conn);

                        using (SqlDataReader reader = selectTeam.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Team tempTeam = new Team(reader[0].ToString());
                                tempTeam.name = reader[1].ToString().Trim();
                                tempTeam.venue = reader[2].ToString().Trim();
                                teams.Add(tempTeam);
                                teamCount++;
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("---------- LOADING ERROR! There was an exception thrown whilst loading teams from the database.");
                        Console.WriteLine(e);
                        return false;
                    }
                    Console.WriteLine("---------- {0} teams(s) loaded successfully.", teamCount);
                    return true;
                }

                bool SelectGames()
                {
                    int gameCount = 0;
                    try
                    {
                        SqlCommand selectGame = new SqlCommand("SELECT * FROM t_Games", conn);

                        using (SqlDataReader reader = selectGame.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Game myGame = new Game(reader[0].ToString());
                                myGame.homeTeamCode = reader[1].ToString().Trim();
                                myGame.awayTeamCode = reader[2].ToString().Trim();
                                myGame.homePlayerCodes = Csv.ParseCsvLine(reader[3].ToString().Trim());
                                myGame.awayPlayerCodes = Csv.ParseCsvLine(reader[4].ToString().Trim());
                                myGame.date = Convert.ToDateTime(reader[5]);
                                myGame.homeScore = reader[6] != DBNull.Value ? Convert.ToInt16(reader[6]) : 0;
                                myGame.homeScore = reader[7] != DBNull.Value ? Convert.ToInt16(reader[7]) : 0;
                                games.Add(myGame);
                                gameCount++;
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("---------- LOADING ERROR! There was an exception thrown whilst loading games from the database.");
                        Console.WriteLine(e);
                        return false;
                    }
                    Console.WriteLine("---------- {0} game(s) loaded successfully.", gameCount);
                    return true;
                }
            }
        }
    }
}

