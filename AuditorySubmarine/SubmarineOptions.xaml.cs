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
    public partial class SubmarineOptions : UserControl
    {
        public SubmarineOptions()
        {
            InitializeComponent();
        }

        private void onChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void hyperlinkButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            this._hyperlinkButton1.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(
                    new Uri(@"/GameFramework;component/Media/buttonIn.jpg", UriKind.Relative)
                )

            };

            Thickness? dd = this._hyperlinkButton1.GetValue(Grid.MarginProperty) as Thickness?;
            if (dd.HasValue)
            {
                Thickness cc = dd.Value;
                cc.Top -= 10;
                this._hyperlinkButton1.SetValue(Grid.MarginProperty, cc);
            }
        }

        private void _hyperlinkButton1_MouseLeave(object sender, MouseEventArgs e)
        {
            this._hyperlinkButton1.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(
                    new Uri(@"/GameFramework;component/Media/buttonOut.jpg", UriKind.Relative)
                )

            };
            Thickness? dd = this._hyperlinkButton1.GetValue(Grid.MarginProperty) as Thickness?;
            if (dd.HasValue)
            {
                Thickness cc = dd.Value;
                cc.Top += 10;
                this._hyperlinkButton1.SetValue(Grid.MarginProperty, cc);
            }

        }

        private void _hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
