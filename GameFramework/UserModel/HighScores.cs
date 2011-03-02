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

        public HighScore Clone()
        {
            return new HighScore
            {
                Delta = this.Delta,
                Level = this.Level,
                Score = this.Score
            };
        }
    }

    public class HighScoreContainer
    {
        ObservableCollection<HighScore> _scores;

        public ObservableCollection<HighScore> Data
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

        public HighScoreContainer Clone()
        {
            HighScoreContainer tmp = HighScoreContainer.Default();
            foreach (HighScore rec in this.Data)
            {
                tmp.Data.Add(rec.Clone());
            }
            return tmp;
        }

        public HighScoreContainer()
        {
            //Data = new ObservableCollection<HighScore>();
        }

        public static HighScoreContainer Default()
        {
            return new HighScoreContainer
            {
                Data = new ObservableCollection<HighScore>()
            };
        }
    }
}
