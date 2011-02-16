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
//using System.Xml.Serialization;

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
            _xPeople.ItemsSource = SubOptions.Instance.UserLists;
            //_xPeople.ItemsSource = new UserModelContainer().UserModels;
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
       /*     IsolatedStorageSettings.ApplicationSettings["foo"] = SubOptions.Instance;
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isoStream = store.OpenFile(@"ApplicationSettings.xml", FileMode.Create))
                {
                    XmlSerializer s = new XmlSerializer(typeof(SubOptions));
                    TextWriter writer = new StreamWriter(isoStream);
                    s.Serialize(writer, SubOptions.Instance);
                    writer.Close();       
                }
            }*/

        }
    }
}
