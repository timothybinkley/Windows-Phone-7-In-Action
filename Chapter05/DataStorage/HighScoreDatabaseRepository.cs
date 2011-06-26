using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;

namespace DataStorage
{
    public class HighScoreDatabaseRepository : IHighScoreRepository
    {
        HighScoresDataContext db;

        public HighScoreDatabaseRepository()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.DirectoryExists("HighScoreDatabase"))
                {
                    storage.CreateDirectory("HighScoreDatabase");
                }
            }
            db = new HighScoresDataContext(@"isostore:/HighScoreDatabase/highscores.sdf");
            if (!db.DatabaseExists())
            {
                db.CreateDatabase();
            }

            //var updater = db.CreateDatabaseSchemaUpdater();
            //if (updater.DatabaseSchemaVersion == 0)
            //{
            //    // add the columns introduced in version one
            //    updater.AddColumn<HighScore>("Difficulty");
            //    updater.DatabaseSchemaVersion = 1;
            //    updater.Execute();
            //}

        }

        public List<HighScore> Load()
        {
            var highscores = from score in db.HighScores
                             orderby score.Score descending
                             where score.LevelsCompleted < 2 
                             select score;
            return highscores.ToList();
        }

        public void Save(List<HighScore> highScores)
        {
            var newscores = highScores.Where(item => item.Id == 0);
            db.HighScores.InsertAllOnSubmit(newscores);
            db.SubmitChanges();
        }

        public void Clear()
        {
            var scores = from score in db.HighScores
                       select score;

            db.HighScores.DeleteAllOnSubmit(scores);
            db.SubmitChanges();

            //db.DeleteDatabase();
        }
    }
}
