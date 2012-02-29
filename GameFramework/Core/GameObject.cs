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
using System.Collections.Generic;

namespace LSRI.AuditoryGames.GameFramework
{
    /*
        A game object with a static image
    */
    public class GameObject : BaseObject
    {
        protected Rectangle rect = null;
        protected ImageBrush imageBrush = null;
        protected Point position = new Point();
        protected Point dimensions = new Point();
        protected int zLayer = 0;
        protected string _collisionName = CollisionIdentifiers.NONE;
        protected string _collisionType = CollisionTypeIdentifiers.BOX;
        public static List<GameObject> gameObjects = new List<GameObject>();

        public Stretch ImageStretch { set; get; }

        virtual public Visibility Visibility
        {
            get
            {
                return (rect == null) ? Visibility.Collapsed : rect.Visibility;
            }

            set
            {
                if (rect != null) rect.Visibility = value;
            }
        }

        public enum CropTo
        {
            Both,           ///<
            WidthOnly,      ///< 
            HeightOnly,     ///<
            None            ///< 
        }

        public CropTo IsCroppable { get; set; }

        public Rectangle Rect
        {
            get
            {
                return rect;
            }
        }

        public string CollisionName
        {
            get
            {
                return _collisionName;
            }
        }

        public string CollisionType
        {
            get
            {
                return _collisionType;
            }
        }

        public int ZLayer
        {
            get
            {
                return zLayer;
            }
        }

        public Point Dimensions
        {
            get
            {
                return dimensions;
            }
        }
        
        public Point Position
        {
            set
            {
                position = value;

                // make sure we are only moving in whole pixels (i.e. integer values)
                // this stops the display from looking fuzzy
                if (rect != null)
                {
                    rect.SetValue(Canvas.LeftProperty, Math.Round(position.X));
                    rect.SetValue(Canvas.TopProperty, Math.Round(position.Y));
                }
            }
            get
            {
                return position;
            }
        }

        public GameObject()
        {
            ImageStretch = Stretch.None;
            IsCroppable = CropTo.Both;
        }

        public void startupGameObject(Point dimensions, string image, int zLayer)
        {
            base.startupBaseObject();

            gameObjects.Add(this);

            imageBrush = null;
            position.X = 0;
            position.Y = 0;
            this.dimensions = dimensions;
            this.zLayer = zLayer;

            rect = new Rectangle()
            {
                Height = dimensions.Y,
                Width = dimensions.X
            };
            rect.SetValue(Canvas.LeftProperty, 0.0d);
            rect.SetValue(Canvas.TopProperty, 0.0d);
            rect.Tag = this;

            // When inserting a GameObject into the parent cavnas children list we assume that any non GameObjects (like
            // gui elements) have been inserted at the end of the collection. This means that a GameObject is inserted
            // when it finds the first GameObject with a higher zOrder, or before any non GameObjects (which keeps
            // the gui elements at the end of the collection, and therefor they will be drawn above all the GameObjects)
            bool inserted = false;
            for (int i = 0; i < (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Count; ++i)
            {
                Rectangle childRect = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children[i] as Rectangle;
                if (childRect != null)
                {
                    GameObject gameObject = childRect.Tag as GameObject;

                    if (gameObject == null || gameObject.ZLayer > zLayer)
                    {
                        (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Insert(i, rect);
                        inserted = true;
                        break;
                    }
                }
                else
                {
                    (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Insert(i, rect);
                    inserted = true;
                    break;
                }
            }

            if (!inserted)
                (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(rect);
          
            prepareImage(image);
        }

        public override void shutdown()
        {
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(rect);
            rect = null;
            imageBrush = null;
            gameObjects.Remove(this);
            base.shutdown();
        }

        virtual protected void prepareImage(string image)
        {
            imageBrush = new ImageBrush
            {
                Stretch = this.ImageStretch, //Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };
            imageBrush.ImageSource = ResourceHelper.GetBitmap(image);
            if (rect!=null) rect.Fill = imageBrush;
        }

        protected void offscreenCheck()
        {
            if (_inUse)
            {
                if (Position.X > (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth ||
                    Position.X < -dimensions.X ||
                    Position.Y > (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight ||
                    Position.Y < -dimensions.Y)
                {
                    shutdown();
                }

                cropToWindow();
            }
        }

        protected void cropToWindow()
        {
            if (IsCroppable==CropTo.None) return;

            // resize the rectangles so the game object doesn't appear to go off the screen
            if ((IsCroppable==CropTo.HeightOnly || IsCroppable==CropTo.Both) && (Position.Y > (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - dimensions.Y))
            {
                double height = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - Position.Y;
                if (height <= 0)
                    shutdown();
                else
                    if (rect!=null) rect.Height = height;
            }

            if ((IsCroppable == CropTo.WidthOnly || IsCroppable == CropTo.Both) && (Position.X > (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X))
            {
                double width = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - Position.X;
                if (width <= 0)
                    shutdown();
                else
                    if (rect != null) rect.Width = width;
            }
        }

        virtual public void collision(GameObject other)
        {

        }
    }
}
