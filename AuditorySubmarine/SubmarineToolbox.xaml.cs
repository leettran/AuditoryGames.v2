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
        private bool _fullMode = false;

        public bool FullMode
        {
            get
            {
                return _fullMode;
            }
            set
            {
                _fullMode = value;
                _SubGatePanel.Visibility = (!_fullMode) ? Visibility.Collapsed : Visibility.Visible;
                _SubLevelPanel.Visibility = (!_fullMode) ? Visibility.Collapsed : Visibility.Visible;
                _SubLifePanel.Visibility = (!_fullMode) ? Visibility.Collapsed : Visibility.Visible;
                _SubTitlePanel.Visibility = (_fullMode) ? Visibility.Collapsed : Visibility.Visible;

            }

        }

        public int Life
        {
            set
            {
                for (int i = 0; i < value; i++)
                    _SubLifePanel.Children.Add(new Image()
                    {
                        Name = "_xSubLife" + i,
                        Height = 18,
                        Margin = (i == 0) ? new Thickness(16, 0, 0, 0) : new Thickness(0),
                        Source = new BitmapImage()
                        {
                            UriSource = new Uri(@"/AuditorySubmarine;component/Media/asub1.png", UriKind.RelativeOrAbsolute)
                        }
                    });

            }
        }

        public int Gates
        {
            set
            {
                _xGateProgessBar.Maximum = value;
            }
        }

        public int Gate
        {
            set
            {
                _xGateProgessBar.Value = value;
            }
        }


        public int Level
        {
            set
            {
                _xLevel.Text = value.ToString();
            }
        }

        public int Score
        {
            set
            {
                _xScore.Text = value.ToString();
            }
        }

        public SubmarineToolbox()
        {
            InitializeComponent();
            while (_SubLifePanel.Children.Count != 0)
                _SubLifePanel.Children.RemoveAt(0);
            //LayoutRoot.DataContext = SubOptions.Instance.User;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
