using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace LSRI.AuditoryGames.GameFramework.Data
{
    public class HighScore
    {
        public int Level { get; set; }
        public double Delta { get; set; }
        public double Score { get; set; }
    }

    public class HighScoreContainer
    {
        ObservableCollection<HighScore> _scores;

        public ObservableCollection<HighScore> HighScores
        {
            get { return _scores; }
            set
            {
                if (_scores != value)
                {
                    _scores = value;
                }
            }
        }

        public HighScoreContainer()
        {
            HighScores = new ObservableCollection<HighScore>
            {
                new HighScore(){ Level = 1, Delta=200, Score=50},
                new HighScore(){ Level = 2, Delta=180, Score=50},
                new HighScore(){ Level = 3, Delta=170, Score=0},
                new HighScore(){ Level = 3, Delta=175, Score=0},
                new HighScore(){ Level = 3, Delta=180, Score=70},
                new HighScore(){ Level = 4, Delta=170, Score=50},
                new HighScore(){ Level = 5, Delta=160, Score=50},
                new HighScore(){ Level = 6, Delta=150, Score=50}
            };
        }
    }
}
