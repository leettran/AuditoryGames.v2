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
using LSRI.AuditoryGames.GameFramework;

namespace LSRI.TreasureHunter.UI
{
    public partial class StartLevelPanel : UserControl
    {
        public StartLevelPanel()
        {
            InitializeComponent();
            this.CurrentLevel = 1;
            this.Gold = 1;
            this.Metal = 1;
        }

        public int CurrentLevel
        {
            set
            {
                _txtLevel.Text = String.Format("Level {0}", value);
            }
        }

        public int Gold
        {
            set
            {
                _xGoldText.Text = value.ToString();
            }
        }

        public int Metal
        {
            set
            {
                _xMetalText.Text = value.ToString();
            }
        }

        public int Live
        {
            set
            {
                _xLiveText.Text = value.ToString();
            }
        }

        public Button StartBtn
        {
            get
            {
                return _btnStart;
            }
        }

        public Button RefreshBtn
        {
            get
            {
                return _btnRefresh;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


    }
}
