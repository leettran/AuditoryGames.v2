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
            if (e.PropertyType == typeof(ObservableCollection<string>))
            {
               e.Field.Label = e.PropertyName;
       
            }
            else if (e.PropertyType.BaseType == typeof(UserModelEntity))
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
