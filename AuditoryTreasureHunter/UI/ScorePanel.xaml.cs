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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LSRI.TreasureHunter.Model;

namespace LSRI.TreasureHunter.UI
{
    public partial class ScorePanel : UserControl
    {
        bool _success = true;
        public delegate void OnCompleteTaskEvent();
        public event OnCompleteTaskEvent OnCompleteTask;


        public  ScorePanel()
        {
            InitializeComponent();
            _success = TreasureOptions.Instance.User.CurrentScore >= TreasureOptions.Instance.User.CurrentTarget;

            int score = TreasureOptions.Instance.User.CurrentScore;
            int gold = TreasureOptions.Instance.User.CurrentGold;
            int target = TreasureOptions.Instance.User.CurrentTarget;
            int maxT = TreasureOptions.Instance.User.MaxTarget;
            int charges = TreasureOptions.Instance.Game.Charges;

            String txtMsg = null;
            if (_success)
            {
                txtMsg = (string)Resources["Txt.Message.Success"];
                _txtHint.Text = (string)Resources["Txt.Hint.Accuracy"];
            }
            else
            {
                txtMsg = (string)Resources["Txt.Message.Failure"];
                if (gold < (charges / 2.0))
                    _txtHint.Text = (string)Resources["Txt.Hint.Failed.Gold"];
                else
                    _txtHint.Text = (string)Resources["Txt.Hint.Failed.Target"];
            }
            _txtMessage.Text = String.Format(txtMsg, TreasureOptions.Instance.User.CurrentLevel);


            if (score >= target)
                _xTextTarget.Text = (string)Resources["Txt.Message.Target.Success"];
            else
                _xTextTarget.Text = (string)Resources["Txt.Message.Target.Failed"];

 
            _xNuggets.Text = gold.ToString();
            _xNuggetsGold.Text = charges.ToString();

            _xScore.Text = score.ToString();
            _xScoreMax.Text = maxT.ToString();

            _xTarget.Text = target.ToString();

            int baseScore = 100;

            double sAccuracy = 100.0 * (double)gold / (double)charges;
            double sGold = 100.0 * (double)score / (double)maxT;
            double sTarget = 100.0 * (double)target / (double)maxT;

            int tAcc = (int)(baseScore * sAccuracy / 100.00);
            _nAccScore.Text = tAcc.ToString();
            //_xAccBar.Value = sAccuracy;

            int tGold = (int)(baseScore * sGold / 100.00);
            _ngoldScore.Text = tGold.ToString();
            //_xGoldBar.Value = sGold;

            FinalScore = (int)(tAcc + tGold);
            if (score < target)
            {
                FinalScore = 0;
                _nTargetScore.Text = @"0 %";
                _nTargetScore.Foreground = new SolidColorBrush(Colors.Red); //#FFFFA7A7
            }

            _nTotalScore.Text = FinalScore.ToString();
        }

        public int FinalScore { set; get; }

        private void _xBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }
    }
}
