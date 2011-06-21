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

namespace LSRI.AuditoryGames.GameFramework.UI
{
    public partial class AboutPanel : UserControl
    {
        public delegate void OnCompleteTaskEvent();
        public event OnCompleteTaskEvent OnCompleteTask = null;


        public AboutPanel()
        {
            InitializeComponent();
            _xBtnOK.Focus();
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }

        private void _xBtnOK_Loaded(object sender, RoutedEventArgs e)
        {
            _xBtnOK.Focus();
        }

    }
}
