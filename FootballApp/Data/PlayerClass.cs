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
            Console.WriteLine("----------Saving player...");
            if (playerCode == null) playerCode = GeneratePlayerCode();

            if (ExistsOnDb())
            {
                Console.WriteLine("----------Player already exists on the database. Using UPDATE statement...");
                return Update();
            }
            else
            {
                Console.WriteLine("----------Player does not exist on the database. Using INSERT statement...");
                Data.players.Add(this);
                return Insert() && UpdateOptionals();
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

                        return insertPlayer.ExecuteNonQuery() > 0 ? true : false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown whilst player ({0}) was being inserted into the database!", playerCode);
                    Console.WriteLine(e);
                    return false;
                }
            }

            bool Update()
            {
                UpdateOptionals();
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

                        

                        return updatePlayer.ExecuteNonQuery() > 0 ? true : false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown whilst player was being updated on the database!");
                    Console.WriteLine(e);
                    return false;
                }
            }

            bool UpdateOptionals()
            {
                Console.WriteLine("----------Updating optionals...");
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();
                        if (teamCode != null)
                        {
                            Console.WriteLine("----------teamCode was NOT blank");
                            SqlCommand updatePlayerTeamCode = new SqlCommand("UPDATE t_Players SET teamcode = @teamcode WHERE playercode = @playercode", conn);
                            updatePlayerTeamCode.Parameters.Add(new SqlParameter("playercode", playerCode));
                            updatePlayerTeamCode.Parameters.Add(new SqlParameter("teamcode", teamCode));

                            updatePlayerTeamCode.ExecuteNonQuery();
                        }
                        else
                        {
                            Console.WriteLine("----------teamCode was blank!!!");
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown whilst player's optionals were being updated on the database!");
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

        public bool Delete()
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                {
                    conn.Open();
                    SqlCommand deletePlayer = new SqlCommand("DELETE FROM t_Players WHERE playerCode = @playercode", conn);
                    deletePlayer.Parameters.Add(new SqlParameter("playercode", playerCode));
                    deletePlayer.ExecuteNonQuery();
                    Data.players.Remove(this);
                    return true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("An exception was thrown whilst player ({0}) was being deleted from the database!", playerCode);
                Console.WriteLine(e);
                return false;
            }
        }

        private string GeneratePlayerCode()
        {
            Random r = new Random();
            return String.Format("{0}{1}{2}{3}", fname.Substring(0, 3).ToUpper(), r.Next(10), r.Next(10), r.Next(10), r.Next(10));
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
