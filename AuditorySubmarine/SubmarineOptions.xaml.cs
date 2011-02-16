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

namespace LSRI.Submarine
{
    public partial class SubmarineOptionPanel : UserControl
    {
        public class CompleteTaskArgs : EventArgs
        {
            private int _currUser;

        }
        public delegate void OnCompleteTaskEvent(CompleteTaskArgs arg);
        public event OnCompleteTaskEvent OnCompleteTask;

        public SubmarineOptionPanel()
        {
            InitializeComponent();
            _xPeople.ItemsSource = GameLevelDescriptor.UserLists;
            //_xPeople.ItemsSource = new UserModelContainer().UserModels;
            _xStaircase.CurrentItem = GameLevelDescriptor.Auditory;
            _xGameOption.CurrentItem = GameLevelDescriptor.Game;
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
    }
}
