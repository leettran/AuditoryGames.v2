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
            if (_success)
            {
                _txtMessage.Text = @"Congratulation!";
            }
            else
            {
                _txtMessage.Text = @"Too bad. Try again...";
            }

            _xAccBar.Maximum = 100;
            _xGoldBar.Maximum = 100;
            _targetBar.Maximum = 100;
            _scoreBar.Maximum = 100;

            int score = TreasureOptions.Instance.User.CurrentScore;
            int gold = TreasureOptions.Instance.User.CurrentGold;
            int target = TreasureOptions.Instance.User.CurrentTarget;
            int maxT = TreasureOptions.Instance.User.MaxTarget;
            int charges = TreasureOptions.Instance.Game.Charges;

            int baseScore = 100;

            double sAccuracy = 100.0 * (double)gold / (double)charges;
            double sGold = 100.0 * (double)score / (double)maxT;
            double sTarget = 100.0 * (double)target / (double)maxT;

            int tAcc = (int)(baseScore * sAccuracy / 100.00);
            _nAccScore.Text = tAcc.ToString();
            _xAccBar.Value = sAccuracy;

            int tGold = (int)(baseScore * sGold / 100.00);
            _ngoldScore.Text = tGold.ToString();
            _xGoldBar.Value = sGold;

            _targetBar.Value = sTarget;
            _scoreBar.Value = sGold;
            int tTarget = Math.Max(0, (int)(100.0 * (double)score / (double)target) - 100);
            _nTarget.Text = "x " + tTarget + "%";
            //_scoreBar.Value = sTarget;
            if (_success)
                tTarget = 100;
            else
                tTarget = 0;

            FinalScore = (int)(tAcc + tGold);
            _nTotalScore.Text = (FinalScore * ((tTarget) / 100.0)).ToString();
        }

        public int FinalScore { set; get; }

        private void _xBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }
    }
}
