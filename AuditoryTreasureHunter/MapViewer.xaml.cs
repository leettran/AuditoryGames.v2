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
using System.Diagnostics;

namespace LSRI.TreasureHunter
{
    public partial class MapViewer : UserControl
    {
        public MapViewer()
        {
            InitializeComponent();
        }

        private void image1_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(_viewBox);
            if (pt.X < 40 || pt.X > (_viewBox.Width - 40))
            {
                String ff = (pt.X < 40) ? "Left" : "Right";
                Thickness tt = _image.Margin;
                tt.Left += (pt.X < 40) ? 20 : -20;
                tt.Left = Math.Min(0, tt.Left);
                _image.Margin = tt;
                // Debug.WriteLine("pos({2}) = {0} - {1}", pt.X, pt.Y,ff);

            }
        }

        private void _image_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
