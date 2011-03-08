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
using LSRI.AuditoryGames.GameFramework;
using LSRI.AuditoryGames.GameFramework.Data;

namespace LSRI.AuditoryGames.GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GameParameters : UserControl
    {
        public delegate void OnCompleteTaskEvent();
        public event OnCompleteTaskEvent OnCompleteTask;

        public GameParameters()
        {
            InitializeComponent();
        }

        public GameParameters(UserModelEntity um, UserModelEntity am, UserModelEntity gm)
        {
            InitializeComponent();
            _xPeople.CurrentItem = um;
            _xStaircase.CurrentItem = am;
            _xGameOption.CurrentItem = gm;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCompleteTask != null) this.OnCompleteTask();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
    

        }

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
