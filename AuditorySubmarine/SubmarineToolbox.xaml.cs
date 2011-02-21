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

namespace LSRI.Submarine
{
    public partial class SubmarineToolbox : UserControl
    {
        public SubmarineToolbox()
        {
            InitializeComponent();
            while (_SubLifePanel.Children.Count != 0)
                _SubLifePanel.Children.RemoveAt(0);
            Image subLife = new Image()
            {
                Height = 20,
                Margin = new Thickness(1),
                Source = new BitmapImage()
                {
                    UriSource = new Uri(@"/AuditorySubmarine;component/Media/asub1.png", UriKind.RelativeOrAbsolute)
                }
            };

            for (int i=0;i<3;i++)
                _SubLifePanel.Children.Add(new Image()
                {
                    Name = "_xSubLife" + i,
                    Height = 18,
                    Margin = (i == 0) ? new Thickness(16,2,2,2) : new Thickness(2),
                    Source = new BitmapImage()
                    {
                        UriSource = new Uri(@"/AuditorySubmarine;component/Media/asub1.png", UriKind.RelativeOrAbsolute)
                    }
                });
        }
    }
}
