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

        public bool ExistsOnDb()
        {
            return ExistsOnDb(teamCode);
        }

        public static bool ExistsOnDb(string pTeamCode)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                {
                    conn.Open();

                    SqlCommand findTeam = new SqlCommand("SELECT COUNT (*) FROM t_Teams WHERE teamcode = @teamcode", conn);

                    findTeam.Parameters.Add(new SqlParameter("teamcode", pTeamCode));

                    int noOfRecords = 0;

                    using (SqlDataReader reader = findTeam.ExecuteReader())
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

        public bool Save()
        {
            if (ExistsOnDb())
                return Update() ? true : false;
            else
            {
                
                return Insert() ? true : false;
            }

            

            bool Insert()
            {

                bool success;
                if(teamCode == null) teamCode = GenerateTeamCode();
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand insertTeam = new SqlCommand("INSERT INTO t_Teams VALUES (@teamcode, @name, @venue)", conn);

                        insertTeam.Parameters.Add(new SqlParameter("teamcode", teamCode));
                        insertTeam.Parameters.Add(new SqlParameter("name", name));
                        insertTeam.Parameters.Add(new SqlParameter("venue", venue));

                        success = insertTeam.ExecuteNonQuery() > 0 ? true : false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an exception whilst team ({0}) was being inserted into the database!", teamCode);
                    Console.WriteLine(e);
                    return false;
                }
                if (success)
                {
                    Data.teams.Add(this);
                    return true;
                }
                else return false;
            }

            bool Update()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Data.OnlineConnStr))
                    {
                        conn.Open();

                        SqlCommand updateTeam = new SqlCommand("UPDATE t_Teams SET name = @name, venue = @venue WHERE teamcode = @teamcode", conn);

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

        private string GenerateTeamCode()
        {
            Random r = new Random();
            return r.Next(10) + r.Next(10) + r.Next(10) + name.Substring(0, 3);
        }

        public static Team getByCode(string pTeamCode)
        {
            return Data.teams.Single(team => team.teamCode == pTeamCode);
        }

        public static bool checkByCode(string pTeamCode)
        {
            return Data.teams.Count(team => team.teamCode == pTeamCode) > 0 ? true : false;
        }
    }
}
