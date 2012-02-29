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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Controls.Data;
using LSRI.AuditoryGames.GameFramework.Data;
using System.Collections.ObjectModel;

namespace LSRI.AuditoryGames.GameFramework.UI
{
    public class SuperDataForm : DataForm
    {
        List<DataForm> Controls = new List<DataForm>();

        public bool CompositeItemIsValid;
        public SuperDataForm()
        {
            this.Loaded += new RoutedEventHandler(SuperDataForm_Loaded);

        }

        void SuperDataForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.CurrentItem == null)
            {
                this.CurrentItem = this.DataContext;
            }
        }

        protected override void OnAutoGeneratingField(DataFormAutoGeneratingFieldEventArgs e)
        {
            // normally change this condition to check it is one of your complex types
            // or that it is not a basic type.
            if (e.Field.IsReadOnly == true)
            {
                e.Field.DescriptionViewerVisibility = Visibility.Visible;
            }
            
            if (e.PropertyType.BaseType == typeof(UserModelEntity))
            {
                var control = new SuperDataForm();

                control.CommandButtonsVisibility = DataFormCommandButtonsVisibility.None;
                control.IsReadOnly = false;
                control.AutoCommit = false;
                
                Controls.Add(control);
                Binding binding = new Binding(e.PropertyName);
                binding.Mode = BindingMode.TwoWay;
                binding.ValidatesOnExceptions = true;
                binding.NotifyOnValidationError = false;
                control.SetBinding(DataForm.CurrentItemProperty, binding);
                e.Field.IsReadOnly = false;
                e.Field.Content = control;
            }

        }

        protected override void OnValidatingItem(System.ComponentModel.CancelEventArgs e)
        {
            // this is needed to have validation on the children controls
            foreach (var control in Controls)
            {
                if (!control.ValidateItem())
                {
                    CompositeItemIsValid = false;
                    break;
                }
            }
        }

        protected override void OnCurrentItemChanged(EventArgs e)
        {

        }

        protected override void OnItemEditEnding(DataFormEditEndingEventArgs e)
        {
        }


    }
}
