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
using AuditoryGames.GameFramework;

namespace AuditoryTreasureHunter
{
    public partial class GameParameters : UserControl
    {
        public GameParameters()
        {
            InitializeComponent();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider ss = (sender as Slider);
            //if (ss!=null)
                //GameLevelInfo._nbTreasureZones = (int)ss.Value;
        }
    }
}
