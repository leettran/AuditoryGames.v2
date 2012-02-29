/*  Auditory Training Games in Silverlight
    Copyright (C) 2008-2012 Nicolas Van Labeke (LSRI, Nottingham University)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSRI.AuditoryGames.GameFramework.Data
{
    public class HighScore
    {
        [Display(Name = "Level", Description = "Every level you won.")]
        public int Level { get; set; }
        [Display(AutoGenerateField = false)]
        public int Delta { get; set; }
        [Display(Name = "Score")]
        public int Score { get; set; }

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
