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

namespace LSRI.AuditoryGames.GameFramework
{
    /// <summary>
    /// Common UI page for all auditory games.
    /// 
    /// @author Matthew Casperson  &lt; http://www.brighthub.com/hubfolio/matthew-casperson.aspx &gt;
    /// @author Nicolas Van Labeke &lt; http://www.lsri.nottingham.ac.uk/nvl/ &gt;
    /// </summary>
    public partial class GamePage : UserControl
    {
        public delegate void EnterFrame(double dt);
        public event EnterFrame enterFrame;
        
        protected DateTime lastTick;

        /// <summary>
        /// Public access to the root canvas of the game
        /// </summary>
        public Canvas LayoutRoot
        {
            get
            {
                return this._LayoutRoot;
            }
        }
        
        /// <summary>
        /// Public access to the media player
        /// </summary>
        public MediaElement AudioPlayer
        {
            get
            {
                return this._AudioPlayer;
            }
        }

        /// <summary>
        /// Public access to the title bar of the game
        /// </summary>
        public Canvas LayoutTitle
        {
            get
            {
                return this._LayoutTitle;
            }
        }

        public GamePage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(CompositionTarget_onLoaded);
            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
            //Application.Current.Host.Content.Resized +=new EventHandler(Content_Resized);


            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            lastTick = DateTime.Now;
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan elapsed = now - lastTick;
            lastTick = now;

            if (enterFrame != null)
            {
                IAppManager.Instance.enterFrame(elapsed.TotalSeconds);
                CollisionManager.Instance.enterFrame(elapsed.TotalSeconds);
                if (enterFrame != null) enterFrame(elapsed.TotalSeconds);
            }
        }

        void CompositionTarget_onLoaded(object sender, EventArgs e)
        {
            AuditoryGameApp.Current.Host.Content.IsFullScreen = true;
        }

        void Content_FullScreenChanged(object sender, EventArgs e)
        {
            //scale content if we are in full screen.
            if (Application.Current.Host.Content.IsFullScreen)
            {
                double heightRatio = Application.Current.Host.Content.ActualHeight / this.Height;
                double widthRatio = Application.Current.Host.Content.ActualWidth / this.Width;

                double _oldHeight = this.Height;
                double _oldWidth = this.Width; 
                double currentWidth = Application.Current.Host.Content.ActualWidth;
                double currentHeight = Application.Current.Host.Content.ActualHeight;

                double uniformScaleAmount = Math.Min((currentWidth / _oldWidth), (currentHeight / _oldHeight));


                ScaleTransform scale = new ScaleTransform();
                scale.ScaleX = uniformScaleAmount;
                scale.ScaleY = uniformScaleAmount;
                this.RenderTransform = scale;
            }
            else
            {
                this.RenderTransform = null;
            }
        }

        void Content_Resized(object sender, EventArgs e)
        {
            if (!Application.Current.Host.Content.IsFullScreen)
            {
                double heightRatio = Application.Current.Host.Content.ActualHeight / this.Height;
                double widthRatio = Application.Current.Host.Content.ActualWidth / this.Width;
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleX = widthRatio;
                scale.ScaleY = heightRatio;
                this.RenderTransform = scale;
            }
           //scale content if we are in full screen.
        }


    }
}
