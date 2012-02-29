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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LSRI.AuditoryGames.GameFramework;

namespace LSRI.Submarine
{
    public class GateAnimatedObject : AnimatedGameObject
    {
        protected Rectangle _bkg;

        public int CanvasIndex { set; get; }


        public override void shutdown()
        {
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(_bkg);
            _bkg = null;

            base.shutdown();
        }

        override public Visibility Visibility
        {
            set
            {
                if (rect != null) rect.Visibility = value;
                if (_bkg != null) _bkg.Visibility = value;
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
                _bkg.SetValue(Canvas.LeftProperty, Math.Round(position.X));
                _bkg.SetValue(Canvas.TopProperty, Math.Round(position.Y));
            }
            get
            {
                return position;
            }
        }

        public void startupGameObject(Point dimensions, int zLayer)
        {
             
            base.startupAnimatedGameObject(dimensions, new AnimationData(
                    new string[] { 
                        "Media/warp1.png", 
                        "Media/warp2.png", 
                        "Media/warp3.png", 
                        "Media/warp4.png", 
                        "Media/warp5.png", 
                        "Media/warp6.png", 
                        "Media/warp7.png", 
                        "Media/asub8.png"
                    }, 50), zLayer, false);

            _bkg = new Rectangle()
            {
                Height = dimensions.Y,
                Width = dimensions.X
            };
            _bkg.SetValue(Canvas.LeftProperty, 0.0d);
            _bkg.SetValue(Canvas.TopProperty, 0.0d);
           // (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(_bkg);
            int index = -1;

            for (int i = 0; i < (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Count; ++i)
            {
                Rectangle childRect = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children[i] as Rectangle;
                if (childRect == this.Rect)
                {
                    index = i;
                    break;
                }
            }
            if (index!=-1)
                (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Insert(index, _bkg);
            this._collisionName = CollisionIdentifiers.ENEMY;
            this.Visibility = Visibility.Collapsed;
        }

        override protected void prepareImage(string image)
        {
            // get the embedded image and display it on the rectangle
            imageBrush = new ImageBrush
            {
                Stretch = Stretch.Fill,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };
            imageBrush.ImageSource = ResourceHelper.GetBitmap(image);
            rect.Fill = imageBrush;
            //rect.Fill = new SolidColorBrush(Colors.Blue);
            if (_bkg != null) _bkg.Fill = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background;
        }
        
    }

    public class GateObject : GameObject
    {
        protected Rectangle _top;
        protected Rectangle _bottom;

        public int CanvasIndex { set; get; }

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



       public void startupGameObject(Point dimensions, int zLayer)
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
            base.startupGameObject(dimensions, "Media/wall.png", zLayer);
            this._collisionName = CollisionIdentifiers.ENEMY;
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(_top);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(_bottom);
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
            rect.Fill = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background;
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
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(_bottom);
            _bottom = null;
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(_top);
            _top = null;
            //_imgBrush = null;
            base.shutdown();
        }
    }
}
