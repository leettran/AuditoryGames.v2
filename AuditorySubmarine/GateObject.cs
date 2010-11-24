using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AuditoryGames.GameFramework;

namespace AuditoryGames.Submarine
{
    public class GateObject : GameObject
    {
        protected Rectangle _top;
        protected Rectangle _bottom;

        public GateObject()
        {

        }

        override public Visibility Visibility
        {
            set
            {
                if (rect != null) rect.Visibility = value;
                if (_top != null) _top.Visibility = value;
                if (_bottom != null) _bottom.Visibility = value;
            }
        }


       new public Point Position
        {
            set
            {
                position = value;

                // make sure we are only moving in whole pixels (i.e. integer values)
                // this stops the display from looking fuzzy
                rect.SetValue(Canvas.LeftProperty, Math.Round(position.X));
                rect.SetValue(Canvas.TopProperty, Math.Round(position.Y));
                _top.SetValue(Canvas.LeftProperty, Math.Round(position.X-10));
                _top.SetValue(Canvas.TopProperty, Math.Round(position.Y -16));
                _bottom.SetValue(Canvas.LeftProperty, Math.Round(position.X-10));
                _bottom.SetValue(Canvas.TopProperty, Math.Round(position.Y+rect.Height));
            }
            get
            {
                return position;
            }
        }



       new  public void startupGameObject(Point dimensions, string image, int zLayer)
        {
            _top = new Rectangle()
            {
                Height = 16,
                Width = 40
            };
            _top.SetValue(Canvas.LeftProperty, 0.0d);
            _top.SetValue(Canvas.TopProperty, 0.0d);
            _bottom = new Rectangle()
            {
                Height = 16,
                Width = 40
            };
            _bottom.SetValue(Canvas.LeftProperty, 0.0d);
            _bottom.SetValue(Canvas.TopProperty, 0.0d);
            _bottom.Tag = this;
            base.startupGameObject(dimensions, image, zLayer);
            this.collisionName = CollisionIdentifiers.ENEMY;
            (App.Current.RootVisual as Page).LayoutRoot.Children.Add(_top);
            (App.Current.RootVisual as Page).LayoutRoot.Children.Add(_bottom);
            this.Visibility = Visibility.Collapsed;

        }

        override protected void prepareImage(string image)
        {
            base.prepareImage(image);
           /* _imgBrush = new ImageBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };*/
            rect.Fill = new SolidColorBrush(Colors.Red);// 
            rect.Fill=(App.Current.RootVisual as Page).LayoutRoot.Background;
            _top.Fill = new SolidColorBrush(Colors.Green);
            _bottom.Fill = new SolidColorBrush(Colors.Green);

            ImageBrush topimg = new ImageBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };
            topimg.ImageSource = ResourceHelper.GetBitmap("Media/gate_top.png");
            _top.Fill = topimg;
            ImageBrush bottomimg = new ImageBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };
            bottomimg.ImageSource = ResourceHelper.GetBitmap("Media/gate_bottom.png");
            _bottom.Fill = bottomimg;
        }


        public override void shutdown()
        {
            (App.Current.RootVisual as Page).LayoutRoot.Children.Remove(_bottom);
            _bottom = null;
            (App.Current.RootVisual as Page).LayoutRoot.Children.Remove(_top);
            _top = null;
            //_imgBrush = null;
            base.shutdown();
        }
    }
}
