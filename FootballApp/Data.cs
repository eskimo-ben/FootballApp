using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BenCsv;

namespace FootballApp
{
    static class Data
    {
        public static string OnlineConnStr { get; }
        public static List<Player> players = new List<Player>();
        public static List<Team> teams = new List<Team>();
        public static List<Game> games = new List<Game>();

        public static bool Load()
        {
            using(SqlConnection conn = new SqlConnection(OnlineConnStr))
            {
                return SelectPlayers() && SelectTeams() && SelectGames();

                bool SelectPlayers()
                {
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
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }

                bool SelectTeams()
                {
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
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }

                bool SelectGames()
                {
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
                                myGame.homeScore = Convert.ToInt16(reader[6]);
                                myGame.awayScore = Convert.ToInt16(reader[7]);
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
