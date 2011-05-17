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
using System.Reflection;

namespace LSRI.TreasureHunter.UI
{
    public partial class TreasureToolbox : UserControl
    {
        public TreasureToolbox()
        {
            InitializeComponent();
        }

        private bool _fullMode = false;

        public bool FullMode
        {
            get
            {
                return _fullMode;
            }
            set
            {
                _fullMode = value;
                _SubLevelPanel.Visibility = (!_fullMode) ? Visibility.Collapsed : Visibility.Visible;
                _SubLifePanel.Visibility = (!_fullMode) ? Visibility.Collapsed : Visibility.Visible;
                _SubTitlePanel.Visibility = (_fullMode) ? Visibility.Collapsed : Visibility.Visible;

                AssemblyName assemblyName = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                _xVersion.Text = String.Format("v{0}.{1}.{2}",
                    assemblyName.Version.Major,
                    assemblyName.Version.Minor,
                    assemblyName.Version.Build);

            }

        }

        public int Life
        {
            set
            {
                _xLiveText.Text = value.ToString();
            }
        }
        public int Score
        {
            set
            {
                _xScoreText.Text = value.ToString();
                _scoreBar.Value = value;
                if (_scoreBar.Value < _targetBar.Value)
                {
                    _scoreBar.Foreground =  new SolidColorBrush(Colors.Red);
                }
                else
                {
                    _scoreBar.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
        }

        public int Level
        {
            set
            {
                _xLevelText.Text = value.ToString();
            }
        }
        public int Gold
        {
            set
            {
                _xGoldText.Text = value.ToString();
            }
        }

        public int MaxScore
        {
            set
            {
                _targetBar.Maximum = value;
                _scoreBar.Maximum = value;
            }
        }

        public int Target
        {
            set
            {
                _targetBar.Value = value;
            }
        }



    }
}
