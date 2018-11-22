using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FootballApp
{
    public class Player
    {
        public string playerCode { get; private set; }
        public string teamCode { get; set; }
        public string fname;
        public string sname;
        public bool status;

        public string fullName { get { return fname + " " + sname; } }

        public Player(string pPlayerCode)
        {
            playerCode = pPlayerCode;
        }

        public Player()
        {
            
        }

        public bool Save()
        {
            if (playerCode == null) playerCode = GeneratePlayerCode();

            if (ExistsOnDb())
            {
                return Update();
            }
            else
            {
                Data.players.Add(this);
                return Insert();
            }

            bool ExistsOnDb()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand findPlayer = new SqlCommand("SELECT COUNT (*) FROM t_Players WHERE playercode = @playercode", conn);

                        findPlayer.Parameters.Add(new SqlParameter("playercode", playerCode));

                        //int noOfRecords = findPlayer.ExecuteNonQuery();

                        int noOfRecords = 0;

                        using (SqlDataReader reader = findPlayer.ExecuteReader())
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

                        SqlCommand insertPlayer = new SqlCommand("INSERT INTO t_Players (playercode, fname, sname, status) VALUES (@playercode, @fname, @sname, @status)", conn);

                        insertPlayer.Parameters.Add(new SqlParameter("playercode", playerCode));
                        insertPlayer.Parameters.Add(new SqlParameter("fname", fname));
                        insertPlayer.Parameters.Add(new SqlParameter("sname", sname));
                        insertPlayer.Parameters.Add(new SqlParameter("status", status));

                        if(teamCode != null)
                        {
                            SqlCommand insertPlayerTeamCode = new SqlCommand("INSERT INTO t_Players (teamcode) VALUES (@teamcode) WHERE playercode = @playercode", conn);
                            insertPlayerTeamCode.Parameters.Add(new SqlParameter("playercode", playerCode));
                            insertPlayerTeamCode.Parameters.Add(new SqlParameter("teamcode", teamCode));
                        }
                        

                        return insertPlayer.ExecuteNonQuery() > 0 ? true : false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an exception whilst player ({0}) was being inserted into the database!", playerCode);
                    Console.WriteLine(e);
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

                        SqlCommand updatePlayer = new SqlCommand("UPDATE t_Players SET fname = @fname, sname = @sname, status = @status WHERE playercode = @playercode", conn);

                        updatePlayer.Parameters.Add(new SqlParameter("playercode", playerCode));
                        updatePlayer.Parameters.Add(new SqlParameter("fname", fname));
                        updatePlayer.Parameters.Add(new SqlParameter("sname", sname));
                        updatePlayer.Parameters.Add(new SqlParameter("status", status));

                        if (teamCode != null)
                        {
                            SqlCommand insertPlayerTeamCode = new SqlCommand("UPDATE t_Players (teamcode) VALUES (@teamcode) WHERE playercode = @playercode", conn);
                            insertPlayerTeamCode.Parameters.Add(new SqlParameter("playercode", playerCode));
                            insertPlayerTeamCode.Parameters.Add(new SqlParameter("teamcode", teamCode));
                        }

                        return updatePlayer.ExecuteNonQuery() > 0 ? true : false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an exception whilst player was being updated on the database!");
                    Console.WriteLine(e);
                    return false;
                }
            }


        }

        public bool Load()
        {
            if (GetData())
            {
                Data.players.Add(this);
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

                        SqlCommand selectPlayer = new SqlCommand("SELECT * FROM t_Players WHERE playercode = @playercode", conn);

                        selectPlayer.Parameters.Add(new SqlParameter("playercode", playerCode));

                        using (SqlDataReader reader = selectPlayer.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(1))
                                {
                                    teamCode = reader[1].ToString().Trim();
                                }
                                fname = reader[2].ToString().Trim();
                                sname = reader[3].ToString().Trim();
                                status = Convert.ToBoolean(reader[4]);
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

        private string GeneratePlayerCode()
        {
            Random r = new Random();
            return fname.Substring(0, 3) + r.Next(10) + r.Next(10) + r.Next(10) + r.Next(10);
        }


        #region Collections of players
        public static Player getByPlayerCode(string pPlayerCode)
        {
            return Data.players.Single(player => player.playerCode == pPlayerCode);
        }

        public static bool checkByPlayerCode(string pPlayerCode)
        {
            return Data.players.Count(player => player.playerCode == pPlayerCode) > 0 ? true : false;
        }

        public static List<Player> getFreeAgents()
        {
            return Data.players.Where(player => player.teamCode == null).ToList();
        }

        public static List<Player> getByTeamCode(string pTeamCode)
        {
            return Data.players.Where(player => player.teamCode == pTeamCode).ToList();
        }
        #endregion
    }
}
