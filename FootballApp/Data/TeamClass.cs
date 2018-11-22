using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FootballApp
{
    public class Team
    {
        public string teamCode { get; set; }
        public string name { get; set; }
        //public string name;
        public string venue;

        public Team(string pTeamCode)
        {
            teamCode = pTeamCode;
        }

        public Team()
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

                        SqlCommand findTeam = new SqlCommand("SELECT * FROM t_Teams WHERE teamcode = @teamcode", conn);

                        findTeam.Parameters.Add(new SqlParameter("teamcode", teamCode));

                        return findTeam.ExecuteNonQuery() > 0 ? true : false;
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

                        SqlCommand insertTeam = new SqlCommand("INSERT INTO t_Users VALUES (@teamcode, @name, @venue)", conn);

                        insertTeam.Parameters.Add(new SqlParameter("teamcode", teamCode));
                        insertTeam.Parameters.Add(new SqlParameter("name", name));
                        insertTeam.Parameters.Add(new SqlParameter("venue", venue));

                        return insertTeam.ExecuteNonQuery() > 0 ? true : false;
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

                        SqlCommand updateTeam = new SqlCommand("UPDATE t_Users SET name = @name, venue = @venue WHERE teamcode = @teamcode", conn);

                        updateTeam.Parameters.Add(new SqlParameter("teamcode", teamCode));
                        updateTeam.Parameters.Add(new SqlParameter("name", name));
                        updateTeam.Parameters.Add(new SqlParameter("venue", venue));
                        return updateTeam.ExecuteNonQuery() > 0 ? true : false;
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
                Data.teams.Add(this);
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

                        SqlCommand selectTeam = new SqlCommand("SELECT * FROM t_Teams WHERE teamcode = @teamcode", conn);

                        selectTeam.Parameters.Add(new SqlParameter("teamcode", teamCode));

                        using (SqlDataReader reader = selectTeam.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                name = reader[1].ToString().Trim();
                                venue = reader[2].ToString().Trim();
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

        public string GenerateTeamCode()
        {
            Random r = new Random();
            return r.Next(10) + r.Next(10) + r.Next(10) + name.Substring(0, 3);
        }
    }
}
