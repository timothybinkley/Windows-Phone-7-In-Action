using System.Collections.Generic;

namespace DataStorage
{
    interface IHighScoreRepository
    {
        List<HighScore> Load();
        void Save(List<HighScore> highScores);
        void Clear();
    }
}
