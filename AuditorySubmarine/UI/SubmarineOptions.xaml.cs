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
using System.Windows.Media.Imaging;
using LSRI.AuditoryGames.GameFramework.Data;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
//using System.Xml.Serialization;

namespace LSRI.Submarine.UI
{
    /// <summary>
    /// @deprecated Replaced by the generic LSRI.AuditoryGames.GameFramework.GameParameters class
    /// </summary>
    public partial class SubmarineOptionPanel : UserControl
    {
        public class CompleteTaskArgs : EventArgs
        {
            //private int _currUser;

        }
        public delegate void OnCompleteTaskEvent(CompleteTaskArgs arg);
        public event OnCompleteTaskEvent OnCompleteTask;

        public SubmarineOptionPanel()
        {
            InitializeComponent();
            _xPeople.CurrentItem = SubOptions.Instance.User;
            _xStaircase.CurrentItem = SubOptions.Instance.Auditory;
            _xGameOption.CurrentItem = SubOptions.Instance.Game;
        }


        private void userModelEditor1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask(new CompleteTaskArgs());
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SubOptions.Instance.SaveConfiguration();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SubOptions.Instance.RetrieveConfiguration();
        }
    }
}
