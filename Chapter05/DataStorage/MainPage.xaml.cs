using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataStorage
{
    public partial class MainPage : PhoneApplicationPage
    {
        IHighScoreRepository repository;
        ObservableCollection<HighScore> highscores;
        Random random = new Random();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //repository = new HighScoreSettingsRepository();
            //repository = new HighScoreFileRepository();
            repository = new HighScoreDatabaseRepository();
            highscores = new ObservableCollection<HighScore>(repository.Load());
            HighScoresList.ItemsSource = highscores;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            highscores.Clear();
            repository.Clear();
        }

        private void add_Click(object sender, EventArgs e)
        {
            int score = random.Next(100, 1000);
            int level = random.Next(1, 5);
            string name = string.Format("{0}{1}{2}", (char)random.Next(65, 90), (char)random.Next(65, 90), (char)random.Next(65, 90));

            var highscore = new HighScore { Name = name, Score = score, LevelsCompleted = level };

            bool added = false;
            for (int i = 0; i < highscores.Count; i++)
            {
                if (highscores[i].Score < highscore.Score)
                {
                    highscores.Insert(i, highscore);
                    added = true;
                    break;
                }
            }
            if (!added)
                highscores.Add(highscore);

            repository.Save(highscores.ToList());
        }

        private void save_Click(object sender, EventArgs e)
        {
            var nameInput = FocusManager.GetFocusedElement() as TextBox;
            if (nameInput != null)
                nameInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            repository.Save(highscores.ToList());
        }

    }
}