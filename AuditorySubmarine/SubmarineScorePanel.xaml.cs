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

        public bool Win { set; get; }

        public void UpdateScores()
        {
            int gateFail = SubOptions.Instance.Game.MaxGates;
            double acctotal = 0;
            double accmax = 0;
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
                    if (pt.GateAccuracy == 0)
                    {
                        this.Win = false;
                        accBar.Value = 0;
                        if (gateFail == SubOptions.Instance.Game.MaxGates) gateFail = i + 1;
                    }
                    else
                    {
                        accBar.Value = (maxpos + 1) - (int)pt.GatePosition;
                    }
                    acctotal += accBar.Value;
                    accmax += maxpos + 1;

                    //accBar.Value = "" + (int)(pt.GateAccuracy + pt.TimeLeft);
                    if (this.Win==false)
                        accBar.Background = new SolidColorBrush(Colors.Red);
                }

                accBar = this.LayoutRoot.FindName("_timeBar" + (i + 1)) as ProgressBar;
                if (accBar != null)
                {
                    accBar.Visibility = Visibility.Visible;
                    accBar.Maximum = 100;
                    accBar.Minimum = 0;
                    if (pt.GateAccuracy == 0)
                    {
                        this.Win = false;
                        accBar.Value = 0;
                    }
                    else
                        accBar.Value = (int)pt.TimeLeft;
                    //accBar.Value = "" + (int)(pt.GateAccuracy + pt.TimeLeft);
                    if (this.Win == false)
                        accBar.Background = new SolidColorBrush(Colors.Red);
                }

                tt = this.LayoutRoot.FindName("_nLife" + (i + 1)) as TextBlock;
                if (tt != null)
                {
                    accBar.Visibility = Visibility.Visible;
                    tt.Text = "" + (int)(pt.LifeLost);
                    if (pt.LifeLost != 0)
                    {
                        tt.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }

            if (this.Win)
            {
                String tt = (string)Resources["Txt.Message.Success"];
                _txtMsgMain.Text = String.Format(tt, SubOptions.Instance.User.CurrentLevel);
                if (acctotal <= (2*accmax/3))
                    _txtMsgHint.Text = (string)Resources["Txt.Hint.Accuracy"];
                else
                    _txtMsgHint.Text = (string)Resources["Txt.Hint.Time"];
                _nTotalScore.Text = "" + SubOptions.Instance.User.CurrentScore;
            }
            else
            {
                String tt = (string)Resources["Txt.Message.Failure"];
                _txtMsgMain.Text = String.Format(tt, SubOptions.Instance.User.CurrentLevel);

                if (gateFail == SubOptions.Instance.Game.MaxGates)
                    tt = (string)Resources["Txt.Hint.Failure.Level"];
                else
                    tt = (string)Resources["Txt.Hint.Failure.Gates"];
                _txtMsgHint.Text = String.Format(tt, gateFail);

                _nTotalScore.Text = "0";
            }
        }

        public SubmarineScorePanel()
        {
            InitializeComponent();
            Win = true;

            UpdateScores();

            
        }

        private void _xBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }
    }
}
