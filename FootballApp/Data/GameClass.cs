using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BenCsv;

namespace FootballApp
{
    public class Game
    {
        public string gameCode { get; set; }
        public string gameName { get { return String.Format("{0} (home) vs {1} (away)", homeTeamName, awayTeamName); } }
        public string homeTeamName { get { return Data.teams.Single(team => team.teamCode == homeTeamCode).name; } }
        public string awayTeamName { get { return Data.teams.Single(team => team.teamCode == awayTeamCode).name; } }
        public string homeTeamCode { get; set; }
        public string awayTeamCode { get; set; }
        public string[] homePlayerCodes { get; set; }
        public string[] awayPlayerCodes { get; set; }
        public DateTime date { get; set; }
        public int homeScore { get; set; }
        public int awayScore { get; set; }

        public Game(string pFixtureCode)
        {
            gameCode = pFixtureCode;
        }

        public Game()
        {
            
        }

        public bool Save()
        {
            if (gameCode == null) gameCode = GenerateGameCode();

            if (ExistsOnDb())
            {
                return Update() ? true : false;
            }
            else
            {
                return Insert() ? true : false;
            }

            bool ExistsOnDb()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand countGames = new SqlCommand("SELECT COUNT (*) FROM t_Games WHERE gamecode = @gamecode", conn);

                        countGames.Parameters.Add(new SqlParameter("gamecode", gameCode));

                        int noOfRecords = 0;

                        using (SqlDataReader reader = countGames.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                noOfRecords = Convert.ToInt16(reader[0]);
                            }
                        }

                        return noOfRecords > 0 ? true : false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown whilst checking if game ({0}) exists on the database!", gameCode);
                    Console.WriteLine(e);
                    return false;
                }
            }

            bool Insert()
            {
                bool success;
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand insertGame = new SqlCommand("INSERT INTO t_Games (gamecode, hometeamcode, awayteamcode, date) VALUES (@gamecode, @hometeamcode, @awayteamcode, @date)", conn);

                        insertGame.Parameters.Add(new SqlParameter("gamecode", gameCode));
                        insertGame.Parameters.Add(new SqlParameter("hometeamcode", homeTeamCode));
                        insertGame.Parameters.Add(new SqlParameter("awayteamcode", awayTeamCode));
                        insertGame.Parameters.Add(new SqlParameter("date", date));

                        success = insertGame.ExecuteNonQuery() > 0 ? true : false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown whilst game ({0}) was being inserted into the database!", gameCode);
                    Console.WriteLine(e);
                    return false;
                }

                if (success)
                {
                    if (UpdateOptionals())
                    {
                        Data.games.Add(this);
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            bool UpdateOptionals()
            {
                bool success = true;
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        

                        if(homePlayerCodes != null)
                        {
                            Console.WriteLine("----------Updating homePlayerCodes");
                            SqlCommand updateOptionalHomePlayerCodes = new SqlCommand("UPDATE t_Games SET homeplayercodes = @homeplayercodes WHERE gamecode = @gamecode", conn);

                            updateOptionalHomePlayerCodes.Parameters.Add(new SqlParameter("gamecode", gameCode));
                            updateOptionalHomePlayerCodes.Parameters.Add(new SqlParameter("homeplayercodes", Csv.GenCsvString(homePlayerCodes)));

                            if (updateOptionalHomePlayerCodes.ExecuteNonQuery() <= 0) success = false;
                        }
                        
                        if(awayPlayerCodes != null)
                        {
                            Console.WriteLine("----------Updating awayPlayerCodes");
                            SqlCommand updateOptionalAwayPlayerCodes = new SqlCommand("UPDATE t_Games SET awayplayercodes = @awayplayercodes WHERE gamecode = @gamecode", conn);

                            updateOptionalAwayPlayerCodes.Parameters.Add(new SqlParameter("gamecode", gameCode));
                            updateOptionalAwayPlayerCodes.Parameters.Add(new SqlParameter("awayplayercodes", Csv.GenCsvString(awayPlayerCodes)));

                            if (updateOptionalAwayPlayerCodes.ExecuteNonQuery() <= 0) success = false;
                        }

                        if (homeScore != 0)
                        {
                            Console.WriteLine("----------Updating homeScore");
                            SqlCommand updateOptionalHomeScore = new SqlCommand("UPDATE t_Games SET homescore = @homescore WHERE gamecode = @gamecode", conn);

                            updateOptionalHomeScore.Parameters.Add(new SqlParameter("gamecode", gameCode));
                            updateOptionalHomeScore.Parameters.Add(new SqlParameter("homescore", homeScore));

                            if (updateOptionalHomeScore.ExecuteNonQuery() <= 0) success = false;
                        }

                        if (awayScore != 0)
                        {
                            Console.WriteLine("----------Updating awayScore");
                            SqlCommand updateOptionalAwayScore = new SqlCommand("UPDATE t_Games SET awayscore = @awayscore WHERE gamecode = @gamecode", conn);

                            updateOptionalAwayScore.Parameters.Add(new SqlParameter("gamecode", gameCode));
                            updateOptionalAwayScore.Parameters.Add(new SqlParameter("awayscore", awayScore));

                            if (updateOptionalAwayScore.ExecuteNonQuery() <= 0) success = false;
                        }
                        return success;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown whilst game ({0}) optionals were being updated on the database!", gameCode);
                    Console.WriteLine(e);
                    return false;
                }
            }

            bool Update()
            {
                if (UpdateOptionals())
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                        {
                            conn.Open();

                            SqlCommand updateGame = new SqlCommand("UPDATE t_Games SET hometeamcode = @hometeamcode, " +
                                "awayteamcode = @awayteamcode, date = @date " +
                                "WHERE gamecode = @gamecode", conn);

                            updateGame.Parameters.Add(new SqlParameter("gamecode", gameCode));
                            updateGame.Parameters.Add(new SqlParameter("hometeamcode", homeTeamCode));
                            updateGame.Parameters.Add(new SqlParameter("awayteamcode", awayTeamCode));
                            updateGame.Parameters.Add(new SqlParameter("date", date));

                            return updateGame.ExecuteNonQuery() > 0 ? true : false;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception was thrown whilst game ({0}) was being updated on the database!", gameCode);
                        Console.WriteLine(e);
                        return false;
                    }
                }
                else return false;
            }
        }

        public bool Load()
        {
            if (GetData())
            {
                Data.games.Add(this);
                return true;
            }
            else return false;

            bool GetData()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand selectGame = new SqlCommand("SELECT * FROM t_Games WHERE gamecode = @gamecode", conn);

                        selectGame.Parameters.Add(new SqlParameter("gamecode", gameCode));

                        using (SqlDataReader reader = selectGame.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                homeTeamCode = reader[1].ToString().Trim();
                                awayTeamCode = reader[2].ToString().Trim();
                                homePlayerCodes = Csv.ParseCsvLine(reader[3].ToString().Trim());
                                awayPlayerCodes = Csv.ParseCsvLine(reader[4].ToString().Trim());
                                date = Convert.ToDateTime(reader[5]);
                                homeScore = Convert.ToInt16(reader[6]);
                                awayScore = Convert.ToInt16(reader[7]);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
                return true;
            }
        }

        private string GenerateGameCode()
        {
            return String.Format("{0}v{1}{2}", homeTeamCode, awayTeamCode, date.ToString("ddmmyy"));
        }

        public static Game getByGameCode(string pGameCode)
        {
            return Data.games.Single(game => game.gameCode == pGameCode);
        }

        public static bool checkByGameCode(string pGameCode)
        {
            return Data.games.Count(game => game.gameCode == pGameCode) > 0 ? true : false;
        }
    }
}
