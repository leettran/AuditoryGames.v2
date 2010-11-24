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

namespace AuditoryGames.GameFramework
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
        protected string collisionName = CollisionIdentifiers.NONE;
        public static List<GameObject> gameObjects = new List<GameObject>();

        public Stretch ImageStretch { set; get; }

        override public Visibility Visibility
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
                return collisionName;
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
            for (int i = 0; i < (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Count; ++i)
            {
                Rectangle childRect = (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children[i] as Rectangle;
                if (childRect != null)
                {
                    GameObject gameObject = childRect.Tag as GameObject;

                    if (gameObject == null || gameObject.ZLayer > zLayer)
                    {
                        (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Insert(i, rect);
                        inserted = true;
                        break;
                    }
                }
                else
                {
                    (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Insert(i, rect);
                    inserted = true;
                    break;
                }
            }

            if (!inserted)
                (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Add(rect);
          
            prepareImage(image);
        }

        public override void shutdown()
        {
            (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Remove(rect);
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
            if (inUse)
            {
                if (Position.X > (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualWidth ||
                    Position.X < -dimensions.X ||
                    Position.Y > (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualHeight ||
                    Position.Y < -dimensions.Y)
                {
                    shutdown();
                }

                cropToWindow();
            }
        }

        protected void cropToWindow()
        {
            // resize the rectangles so the game object doesn't appear to go off the screen
            if (Position.Y > (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualHeight - dimensions.Y)
            {
                double height = (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualHeight - Position.Y;
                if (height <= 0)
                    shutdown();
                else
                    if (rect!=null) rect.Height = height;
            }

            if (Position.X > (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualWidth - dimensions.X)
            {
                double width = (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualWidth - Position.X;
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
