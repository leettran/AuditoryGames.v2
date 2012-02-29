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

        public Point Target
        {
            set
            {
                Point? pp = value;
                if (pp != null)
                {
                    _xTargetText.Text = pp.Value.X.ToString();
                    _xTargetBar.Maximum = pp.Value.Y;
                    _xTargetBar.Value = pp.Value.X;
                    _xMaxGoldText.Text = pp.Value.Y.ToString();
                }

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




    }
}
