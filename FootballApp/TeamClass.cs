using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp
{
    class TeamClass
    {
        public string teamCode { get; }
        public string name;
        public string venue;

        public TeamClass()
        {
            teamCode = GenTeamCode();

            string GenTeamCode()
            {
                return "";
            }
        }
    }
}
