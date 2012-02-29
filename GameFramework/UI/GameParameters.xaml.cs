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
using LSRI.AuditoryGames.GameFramework;
using LSRI.AuditoryGames.GameFramework.Data;

namespace LSRI.AuditoryGames.GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GameParameters : UserControl
    {
        public delegate void OnTaskEvent();
        public event OnTaskEvent OnCompleteTask;
        public event OnTaskEvent OnCalibrateTask;
        public event OnTaskEvent OnCommitParamsTask;

        public class Calibration
        {
            public int Frequency { get; set; }
            public int Duration { get; set; }
        }

        public Calibration _calibration = null; 

        public GameParameters()
        {
            _calibration = new Calibration
            {
                Duration = 1000,
                Frequency = 1500
            };
            InitializeComponent();

            _xCalDuration.DataContext = _calibration;
            _xCalFreq.DataContext = _calibration;

        }

        public GameParameters(UserModelEntity um, UserModelEntity am, UserModelEntity gm)
        {
            _calibration = new Calibration
            {
                Duration = 800,
                Frequency = 1500
            }; 
            
            InitializeComponent();
            _xPeople.CurrentItem = um;
            _xStaircase.CurrentItem = am;
            _xGameOption.CurrentItem = gm;

            _xCalDuration.DataContext = _calibration;
            _xCalFreq.DataContext = _calibration;
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

 
        private void BtnQuota_Click(object sender, RoutedEventArgs e)
        {
            GameLogger.IncreaseStorageQuota(500);
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog exportDialog = new SaveFileDialog();
            exportDialog.Filter = "Text Files | *.txt";
            exportDialog.DefaultExt = "txt";
            bool? result = exportDialog.ShowDialog();
            if (result == true)
            {
                System.IO.Stream fileStream = exportDialog.OpenFile();
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fileStream);
                sw.WriteLine("Writing some text in the file.");
                sw.Flush();
                sw.Close();
            }
        }

        private void _xFullScreen_Click(object sender, RoutedEventArgs e)
        {
            AuditoryGameApp.Current.Host.Content.IsFullScreen = !AuditoryGameApp.Current.Host.Content.IsFullScreen;
        }

        private void btnCalibrate_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCalibrateTask != null) this.OnCalibrateTask();
        }

        private void _xStaircase_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (e.EditAction == DataFormEditAction.Commit)
            {
                if (this.OnCommitParamsTask != null) this.OnCommitParamsTask();
            }

        }

    }
}
