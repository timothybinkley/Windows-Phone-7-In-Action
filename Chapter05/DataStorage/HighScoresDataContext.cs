using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace DataStorage
{
    [Database(Name="GameScores")]
    public class HighScoresDataContext : DataContext
    {
        public Table<HighScore> HighScores;

        public HighScoresDataContext(string path) : base(path) { }
    }
}
