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

namespace LSRI.AuditoryGames.GameFramework
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
