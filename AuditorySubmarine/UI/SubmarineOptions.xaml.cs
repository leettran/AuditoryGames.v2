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
