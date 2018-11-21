using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FootballApp
{
    class Player
    {
        public string playerCode { get; }
        public string teamCode { get; set; }
        public string fname;
        public string sname;
        public bool status;

        public Player(string pPlayerCode)
        {
            playerCode = pPlayerCode;
        }

        public Player()
        {
            playerCode = GeneratePlayerCode();
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

                        SqlCommand findPlayer = new SqlCommand("SELECT * FROM t_Players WHERE playercode = @playercode", conn);

                        findPlayer.Parameters.Add(new SqlParameter("playercode", playerCode));

                        return findPlayer.ExecuteNonQuery() > 0 ? true : false;
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

                        SqlCommand insertPlayer = new SqlCommand("INSERT INTO t_Players VALUES (@playercode, @teamcode, @fname, @sname, @status)", conn);

                        insertPlayer.Parameters.Add(new SqlParameter("playercode", playerCode));
                        insertPlayer.Parameters.Add(new SqlParameter("teamcode", teamCode));
                        insertPlayer.Parameters.Add(new SqlParameter("fname", fname));
                        insertPlayer.Parameters.Add(new SqlParameter("sname", sname));
                        insertPlayer.Parameters.Add(new SqlParameter("status", status));

                        return insertPlayer.ExecuteNonQuery() > 0 ? true : false;
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

                        SqlCommand updatePlayer = new SqlCommand("UPDATE t_Players SET teamcode = @teamcode, fname = @fname, sname = @sname, status = @status) WHERE playercode = @playercode", conn);

                        updatePlayer.Parameters.Add(new SqlParameter("playercode", playerCode));
                        updatePlayer.Parameters.Add(new SqlParameter("teamcode", teamCode));
                        updatePlayer.Parameters.Add(new SqlParameter("fname", fname));
                        updatePlayer.Parameters.Add(new SqlParameter("sname", sname));
                        updatePlayer.Parameters.Add(new SqlParameter("status", status));

                        return updatePlayer.ExecuteNonQuery() > 0 ? true : false;
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
    }
}
