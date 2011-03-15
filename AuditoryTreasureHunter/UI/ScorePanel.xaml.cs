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

namespace LSRI.TreasureHunter.UI
{
    public partial class ScorePanel : UserControl
    {

        public delegate void OnCompleteTaskEvent();
        public event OnCompleteTaskEvent OnCompleteTask;


        public ScorePanel()
        {
            InitializeComponent();
        }

        public double Accuracy
        {
            set
            {
                _xAccBar.Maximum = 100;
                _xAccBar.Value = value;
                _nAccScore.Text = (150.0 * value / 100.00).ToString();
            }
        }

        public double Gold
        {
            set
            {
                _xGoldBar.Maximum = 100;
                _xGoldBar.Value = value;
                _ngoldScore.Text = (150.0 * value / 100.0).ToString();
            }
        }

        private void _xBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }
    }
}
