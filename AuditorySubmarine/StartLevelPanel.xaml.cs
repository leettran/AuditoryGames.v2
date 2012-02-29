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
using LSRI.AuditoryGames.GameFramework;

namespace LSRI.Submarine
{
    public partial class StartLevelPanel : UserControl
    {
        public StartLevelPanel()
        {
            InitializeComponent();
            _btnStart.Icon.Source = ResourceHelper.GetBitmap("/GameFramework;component/Media/btn_play.png");
            _btnStart.TextContent.Text = "Play";
            _btnStart.Icon.Height = 32;
            _btnStart.Icon.Width = 32;
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
