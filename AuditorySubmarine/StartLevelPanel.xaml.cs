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

namespace LSRI.Submarine
{
    public partial class StartLevelPanel : UserControl
    {
        public StartLevelPanel()
        {
            InitializeComponent();
            _btnStart.Icon.Source = ResourceHelper.GetBitmap("Media/fullscreen.png");
            _btnStart.TextContent.Text = "Play";
            _btnStart.Icon.Height = 22;
            _btnStart.Icon.Width = 31;
            this.CurrentLevel = 0;
            this.CurrentGate = 2;

            //this.button1.Style = Application.Current.Resources["FullScreenButtonStyle"] as Style;

        }

        public int CurrentLevel
        {
            set
            {
                _txtLevel.Text = String.Format("Level {0}", value);
            }
        }

        public int CurrentGate
        {
            set
            {
               // _txtGates.Text = String.Format("You went through {0} gates out of 5", value);
                //_barGates.Value = value;
                _btnStart.TextContent.Text = (value == 0) ? "Play" : "Continue";
            }
        }

        public Button StartBtn
        {
            get
            {
                return _btnStart;
            }
        }
    }
}
