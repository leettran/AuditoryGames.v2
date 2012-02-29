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
using LSRI.AuditoryGames.GameFramework.Data;

namespace LSRI.AuditoryGames.GameFramework.UI
{
    public partial class UserModelEditor : UserControl
    {

        public delegate void ValidateModel();
        public event ValidateModel _ValidateModelHook = null;

        public UserModelEditor()
        {
            InitializeComponent();
            var selected = LayoutRoot.DataContext as UserModelContainer;
            var name = selected.CurrentModel;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var selected = LayoutRoot.DataContext as UserModelContainer;
            var name = selected.CurrentModel;


            if (_ValidateModelHook != null)
                _ValidateModelHook();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (_ValidateModelHook != null)
                _ValidateModelHook();

        }

        public void AddModel(UserModel mod)
        {
            var selected = LayoutRoot.DataContext as UserModelContainer;
            selected.UserModels.Add(mod);



        }
    }
}
