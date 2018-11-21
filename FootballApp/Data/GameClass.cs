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

                        SqlCommand findFixture = new SqlCommand("SELECT * FROM t_Fixtures WHERE fixturecode = @fixturecode", conn);

                        findFixture.Parameters.Add(new SqlParameter("fixturecode", gameCode));

                        return findFixture.ExecuteNonQuery() > 0 ? true : false;
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
            }

            bool Insert()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand insertGame = new SqlCommand("INSERT INTO t_Games VALUES (@gamecode, @hometeamcode, @awayteamcode, @homeplayercodes, @awayplayercodes, @date, @homescore, @awayscore)", conn);

                        insertGame.Parameters.Add(new SqlParameter("gamecode", gameCode));
                        insertGame.Parameters.Add(new SqlParameter("hometeamcode", homeTeamCode));
                        insertGame.Parameters.Add(new SqlParameter("awayteamcode", awayTeamCode));
                        insertGame.Parameters.Add(new SqlParameter("homeplayercodes", Csv.GenCsvString(homePlayerCodes)));
                        insertGame.Parameters.Add(new SqlParameter("awayplayercodes", Csv.GenCsvString(awayPlayerCodes)));
                        insertGame.Parameters.Add(new SqlParameter("date", date));
                        insertGame.Parameters.Add(new SqlParameter("homescore", homeScore));
                        insertGame.Parameters.Add(new SqlParameter("awayscore", awayScore));

                        return insertGame.ExecuteNonQuery() > 0 ? true : false;
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
            }

            bool Update()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand updateGame = new SqlCommand("UPDATE t_Games SET gamecode = @gamecode, hometeamcode = @hometeamcode, " +
                            "awayteamcode = @awayteamcode, homeplayercodes = @homeplayercodes, awayplayercodes = @awayplayercodes, date = @date, " +
                            "homescore=@homescore, awayscore=@awayscore WHERE fixturecode = @fixturecode", conn);

                        updateGame.Parameters.Add(new SqlParameter("gamecode", gameCode));
                        updateGame.Parameters.Add(new SqlParameter("hometeamcode", homeTeamCode));
                        updateGame.Parameters.Add(new SqlParameter("awayteamcode", awayTeamCode));
                        updateGame.Parameters.Add(new SqlParameter("homeplayercodes", Csv.GenCsvString(homePlayerCodes)));
                        updateGame.Parameters.Add(new SqlParameter("awayplayercodes", Csv.GenCsvString(awayPlayerCodes)));
                        updateGame.Parameters.Add(new SqlParameter("date", date));
                        updateGame.Parameters.Add(new SqlParameter("homescore", homeScore));
                        updateGame.Parameters.Add(new SqlParameter("awayscore", awayScore));

                        return updateGame.ExecuteNonQuery() > 0 ? true : false;
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

        public string GenerateGameCode()
        {
            return String.Format("{0}v{1}{2}", homeTeamCode, awayTeamCode, date.ToString("ddmmyy"));
        }
    }
}
