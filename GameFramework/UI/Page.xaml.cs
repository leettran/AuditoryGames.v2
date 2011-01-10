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

namespace AuditoryGames.GameFramework
{
    /// <summary>
    /// Common UI page for all auditory games.
    /// </summary>
    public partial class GamePage : UserControl
    {
        public delegate void EnterFrame(double dt);
        public event EnterFrame enterFrame;
        
        protected DateTime lastTick;


        public Canvas LayoutRoot
        {
            get
            {
                return this._LayoutRoot;
            }
        }


        public MediaElement AudioPlayer
        {
            get
            {
                return this._AudioPlayer;
            }
        }


        

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
    }
}
