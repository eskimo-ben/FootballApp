using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp
{
    class Fixture
    {
        public string gameCode { get; }
        public string homeTeamCode { get; set; }
        public string awayTeamCode { get; set; }
        public string[] homePlayerCodes { get; set; }
        public string[] awayPlayerCodes { get; set; }
        public DateTime date { get; set; }
        public int homeScore { get; set; }
        public int awayScore { get; set; }
    }
}
