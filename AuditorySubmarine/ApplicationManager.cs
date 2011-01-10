using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AuditoryGames.Utils;
using AuditoryGames.GameFramework;
using AuditoryGames.Submarine;



namespace AuditoryGames.GameFramework
{
    public partial class IApplicationManager
    {
        public int dev2 = 2;
    }


    /// <summary>
    /// 
    /// </summary>
    /// @see States
    public static class SubmarineStates
    {
        public static string LEVEL_STATE = "start_Level";
        public static string OPTION_STATE = "start_option";
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApplicationManager
    {
        /// <summary>
        /// 
        /// </summary>
        private SubmarinePlayer _submarine = null;
        private WallObject _wall = null;
        private GateObject _gate = null;
        private Random _random;
        private double _posRatio = .5;

        private StopwatchPlus sp1;

        protected static ApplicationManager instance = null;
        
        /// <summary>
        /// 
        /// </summary>
       public static ApplicationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ApplicationManager();
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ApplicationManager()
        {
            KeyHandler.Instance.IskeyUpOnly = true;
            //_synthEx = new Frequency2IGenerator();
            _random = new Random();
        }

        /// <summary>
        /// 
        /// </summary>
        public void startupApplicationManager()
        {
            StateManager.Instance.registerStateChange(
                States.START_STATE,
                new StateChangeInfo.StateFunction(startMainMenu),
                new StateChangeInfo.StateFunction(endMainMenu));

            StateManager.Instance.registerStateChange(
                SubmarineStates.LEVEL_STATE,
                new StateChangeInfo.StateFunction(startGame),
                new StateChangeInfo.StateFunction(exitGame));

            //Score = SavedScore;

            //(GameApplication.Current.RootVisual as GamePage).GetPlayer().Loaded += delegate(object sender, RoutedEventArgs e) { Debug.WriteLine("SOUND LOADED"); };
            //(GameApplication.Current.RootVisual as GamePage).GetPlayer().CurrentStateChanged += delegate(object sender, RoutedEventArgs e) { Debug.WriteLine("CurrentStateChanged"); };
           //Page pp = (App.Current.RootVisual as Page);
           // (GameApplication.Current.RootVisual as GamePage).GetPlayer().SetSource(_synthEx);

        }

        public void enterFrame(double dt)
        {
            if (KeyHandler.Instance.isKeyPressed(Key.Escape) && StateManager.Instance.CurrentState.Equals(SubmarineStates.LEVEL_STATE))
                StateManager.Instance.setState(States.START_STATE);

          /* if (KeyHandler.Instance.isKeyPressed(Key.Up))
            {
                 //KeyHandler.Instance.clearKeyPresses();
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {

            }
            else*/ if (KeyHandler.Instance.isKeyPressed(Key.Space))
            {
                if (_gate==null) return;
                Visibility isVis = _gate.Visibility;
                /*if (_gate.InUse)
                {
                    _gate.shutdown();
                    KeyHandler.Instance.clearKeyPresses();
                }
                else
                {
                    Point tt = _gate.Position;
                    _gate.startupGameObject(
                          new Point(21, 50),
                          "Media/wall.png",
                          ZLayers.BACKGROUND_Z + 5);
                    _gate.Position = tt;
                    KeyHandler.Instance.clearKeyPresses();
                }*/
                if (isVis == Visibility.Visible)
                    _gate.Visibility = Visibility.Collapsed;
                else
                    _gate.Visibility = Visibility.Visible;
                KeyHandler.Instance.clearKeyPresses();
            }
            double s = _submarine.Position.X;
            double g = _gate.Position.X;
            double r = s / g;

            double K= 1;
            double gr = .1;
            //double lg = K * (1 - Math.Exp(-gr * _score/50));
           // Debug.WriteLine("##### SCORE : {0}",lg);

            if (r > _posRatio)
            {
                _gate.Visibility = Visibility.Visible;
            }
                            

        }

        public void shutdown()
        {
            //SavedScore = Score;
        }


        private void startMainMenu()
        {
            Button btnStart = new Button();
            btnStart.Content = "Start Game";
            btnStart.Width = 100;
            btnStart.Height = 35;
            btnStart.SetValue(Canvas.LeftProperty, 490.0);
            btnStart.SetValue(Canvas.TopProperty, 355.0);
            btnStart.Click += delegate(object sender, RoutedEventArgs e) { 
                StateManager.Instance.setState(SubmarineStates.LEVEL_STATE); 
            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnStart);

            //StatusPanelControl ctr = new StatusPanelControl();
            //(GameApplication.Current.RootVisual as GamePage).GetLayoutElt().Children.Add(ctr);

          /*  MediaElement media = new MediaElement();
            media.Name = "AudioPlayer";
            media.Loaded += delegate(object sender, RoutedEventArgs e) { Debug.WriteLine("SOUND LOADED"); };
            media.CurrentStateChanged += delegate(object sender, RoutedEventArgs e) { Debug.WriteLine("CurrentStateChanged"); };
            GamePage pp = (GameApplication.Current.RootVisual as GamePage);
            media.SetSource(_synth);

            _synth.Arpeggiator.Notes.Add(new Note(Notes.C, 3));
            _synth.Arpeggiator.Notes.Add(new Note(Notes.F, 3));
            _synth.Arpeggiator.Notes.Add(new Note(Notes.A, 4));
            _synth.Arpeggiator.Notes.Add(new Note(Notes.C, 4));
            _synth.Arpeggiator.Notes.Add(new Note(Notes.F, 4));
            _synth.Arpeggiator.Notes.Add(new Note(Notes.A, 5));
            _synth.Arpeggiator.Notes.Add(new Note(Notes.C, 5));

            _synth.Arpeggiator.Start();
            // _synth.TriggerNote(new Note());
            (GameApplication.Current.RootVisual as GamePage).GetLayoutElt().Children.Insert(
                 (GameApplication.Current.RootVisual as GamePage).GetLayoutElt().Children.Count, media);*/

        }

        protected void removeAllCanvasChildren()
        {
            UIElementCollection children = (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children;
            while (children.Count != 0)
                children.RemoveAt(0);
        }

        private void endMainMenu()
        {
            removeAllCanvasChildren();
        }
        private void startGame()
        {
            //ScorePanelControl score = new ScorePanelControl();
            //(GameApplication.Current.RootVisual as GamePage).GetTitleElt().Children.Add(score);
            //score.Width = (GameApplication.Current.RootVisual as GamePage).GetTitleElt().ActualWidth;

            sp1 = new StopwatchPlus(
                    sw => Debug.WriteLine("Game Started"),
                    sw => Debug.WriteLine("Time! {0}", sw.EllapsedMilliseconds),
                    sw => Debug.WriteLine("totot {0}", sw.EllapsedMilliseconds)

                );
            // initialise collisions
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMY);
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMYWEAPON);
            //CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYERWEAPON, CollisionIdentifiers.ENEMY);

            _submarine = new SubmarinePlayer();
            _submarine.startupSubmarine(
                new Point(48,32),
                new AnimationData(
                    new string[] { 
                        "Media/asub1.png", 
                        "Media/asub3.png", 
                        "Media/asub4.png", 
                        "Media/asub2.png"
                    },
                    50),
                ZLayers.PLAYER_Z);
            _submarine.Position = new Point(0, 75);

            Canvas zone = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot;

            Point dim = new Point(zone.ActualWidth,zone.ActualHeight);

            _wall = new WallObject();
            _wall.startupGameObject(
                new Point(20, dim.Y),
                "Media/wall.png",
                ZLayers.BACKGROUND_Z);
            _wall.Position = new Point(dim.X-20, 0);

            double perc = _random.NextDouble();
            _gate = new GateObject();
            _gate.startupGameObject(
                new Point(21, 50),
                "Media/wall.png",
                ZLayers.BACKGROUND_Z + 5);
            _gate.Position = new Point(dim.X - 21, (dim.Y - 50) * perc);
 

            TextBlock txtbScore = new TextBlock();
            txtbScore.Text = "150";// ApplicationManager.Instance.Score.ToString();
            txtbScore.Name = "txtbScore";
            txtbScore.Width = 100;
            txtbScore.Height = 35;
            txtbScore.FontSize = 20;
            txtbScore.Foreground = new SolidColorBrush(Colors.White);
            txtbScore.SetValue(Canvas.LeftProperty, 10.0);
            txtbScore.SetValue(Canvas.TopProperty, 10.0);
            // we have to insert any non GameObjects at the end of the children collection
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Insert(
                (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Count, txtbScore);

            ///_synthEx.Arpeggiator.Notes[0].Frequency = 5000;
            ///_synthEx.Arpeggiator.Notes[2].Frequency = (float)(5000 - 50*(_gate.Position.Y - _submarine.Position.Y)/10.0);
            ///_synthEx.Arpeggiator.Start();
            //_synth.TriggerNote(new Note(Notes.F, 5));



        }
        private void exitGame()
        {
            while (GameObject.gameObjects.Count != 0)
                GameObject.gameObjects[0].shutdown();

            removeAllCanvasChildren();
            UIElementCollection children = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children;
            while (children.Count != 0)
                children.RemoveAt(0);

            //_synthEx.Arpeggiator.Stop();
            //txtbScore = null;
            sp1.Stop();
        }

    }

}
