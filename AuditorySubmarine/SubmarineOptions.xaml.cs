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

namespace LSRI.Submarine
{
    public partial class SubmarineOptions : UserControl
    {
        public delegate void OnCompleteTaskEvent();
        public event OnCompleteTaskEvent OnCompleteTask;

        public SubmarineOptions()
        {
            InitializeComponent();
           // _xPeople.ItemsSource = new UserModelContainer().UserModels;
            
            
        }


        private void userModelEditor1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }
    }
}
