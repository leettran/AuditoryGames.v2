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

namespace LSRI.Submarine
{
    public partial class SubmarineScorePanel : UserControl
    {

        public delegate void OnCompleteTaskEvent();
        public event OnCompleteTaskEvent OnCompleteTask;

        public void UpdateScores()
        {
            double maxpos = SubOptions.Instance.Game.GateSize;
            //double dartScore = Math.Max(0, 1 - deltapos / (maxpos + 1)) * baseScore;



            for (int i = 0; i < SubOptions.Instance._scoreBuffer.Count; i++)
            {
                SubOptions.ScorePattern pt = SubOptions.Instance._scoreBuffer[i];
                TextBlock tt = this.LayoutRoot.FindName("_nScore" + (i + 1)) as TextBlock;
                if (tt != null)
                {
                    tt.Visibility = Visibility.Visible;
                    tt.Text = "" + (int)(pt.GateAccuracy + pt.TimeLeft);
                }

                ProgressBar accBar = this.LayoutRoot.FindName("_accBar" + (i + 1)) as ProgressBar;
                if (accBar != null)
                {
                    accBar.Visibility = Visibility.Visible;
                    accBar.Maximum = maxpos + 1;
                    accBar.Minimum = 0;
                    accBar.Value = (maxpos + 1) - (int)pt.GatePosition;
                    //accBar.Value = "" + (int)(pt.GateAccuracy + pt.TimeLeft);
                }

                accBar = this.LayoutRoot.FindName("_timeBar" + (i + 1)) as ProgressBar;
                if (accBar != null)
                {
                    accBar.Visibility = Visibility.Visible;
                    accBar.Maximum = 100;
                    accBar.Minimum = 0;
                    accBar.Value = (int)pt.TimeLeft;
                    //accBar.Value = "" + (int)(pt.GateAccuracy + pt.TimeLeft);
                }
            }

            _nTotalScore.Text = "" + SubOptions.Instance.User.CurrentScore;
        }

        public SubmarineScorePanel()
        {
            InitializeComponent();

            UpdateScores();

            
        }

        private void _xBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }
    }
}
