using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LSRI.AuditoryGames.Utils;
using LSRI.AuditoryGames.GameFramework;
using LSRI.Submarine;
using LSRI.AuditoryGames.AudioFramework;
using LSRI.AuditoryGames.GameFramework.Data;



namespace LSRI.Submarine
{

    /// <summary>
    /// 
    /// </summary>
    /// @see States
    public class SubmarineStates : States
    {
        public const string LEVEL_STATE = "start_Level";
        public const string OPTION_STATE = "start_option";
        public const string LOG_STATE = "start_log";
    }

    public static class GameLevelDescriptor
    {
        private static TextBlock _debugUI = null;
        public static int CurrentLevel { get; set; }
        public static int CurrentGate { get; set; }
        public static int TrainingFrequency { get; set; }
        public static int ThresholdFrequency { get; set; }

        public static void Attach(GamePage pg)
        {
            if (pg == null) return;
            if (_debugUI == null)
            {
                _debugUI = new TextBlock();
                _debugUI.Text = "150";// SubmarineApplicationManager.Instance.Score.ToString();
                _debugUI.Name = "txtbScore";
                _debugUI.Width = 100;
                _debugUI.Height = 35;
                _debugUI.FontSize = 11;
                _debugUI.FontFamily = new FontFamily("Courier New");
                _debugUI.Foreground = new SolidColorBrush(Colors.White);
                _debugUI.SetValue(Canvas.LeftProperty, 10.0);
                _debugUI.SetValue(Canvas.TopProperty, pg.LayoutRoot.ActualHeight - 75);
            }
            // we have to insert any non GameObjects at the end of the children collection
            pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, _debugUI);
        }

        public static void Debug()
        {
            if (_debugUI == null) return;
            _debugUI.Text = String.Format(
                "Training Fq : {0} Hz\n"+
                "Delta       : {1} Hz\n-----\n"+
                "Level       : {2}\n"+
                "Gates       : {3}",
                TrainingFrequency, 
                ThresholdFrequency,
                CurrentLevel,
                CurrentGate);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubmarineApplicationManager : IAppManager
    {
        /// <summary>
        /// 
        /// </summary>
        public SubmarinePlayer _submarine = null;
        private WallObject _wall = null;
        private GateObject _gate = null;
        private Random _random;
        private double _posRatio = .15;

        private StopwatchPlus sp1;

        public Frequency2IGenerator _synthEx = null;

        private Brush bg = null;

        //protected static SubmarineApplicationManager instance = null;
        
        /// <summary>
        /// 
        /// </summary>
        public new static IAppManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SubmarineApplicationManager();
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected SubmarineApplicationManager()
        {
            KeyHandler.Instance.IskeyUpOnly = true;
            //_synthEx = new Frequency2IGenerator();
            _random = new Random();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void startupApplicationManager()
        {
            StateManager.Instance.registerStateChange(
                States.START_STATE,
                new StateChangeInfo.StateFunction(startMainMenu),
                new StateChangeInfo.StateFunction(endMainMenu));

            StateManager.Instance.registerStateChange(
                SubmarineStates.LEVEL_STATE,
                new StateChangeInfo.StateFunction(startGame),
                new StateChangeInfo.StateFunction(exitGame));


            StateManager.Instance.registerStateChange(
                SubmarineStates.OPTION_STATE,
                new StateChangeInfo.StateFunction(startOptions),
                new StateChangeInfo.StateFunction(exitOptions));

            GameLevelDescriptor.CurrentLevel = 1;
            GameLevelDescriptor.CurrentGate = 5;
            GameLevelDescriptor.TrainingFrequency = 5000;
            GameLevelDescriptor.ThresholdFrequency = 200;



            //Score = SavedScore;

            //(GameApplication.Current.RootVisual as GamePage).GetPlayer().Loaded += delegate(object sender, RoutedEventArgs e) { Debug.WriteLine("SOUND LOADED"); };
            //(GameApplication.Current.RootVisual as GamePage).GetPlayer().CurrentStateChanged += delegate(object sender, RoutedEventArgs e) { Debug.WriteLine("CurrentStateChanged"); };
           //Page pp = (App.Current.RootVisual as Page);
           // (GameApplication.Current.RootVisual as GamePage).GetPlayer().SetSource(_synthEx);
            MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
            _synthEx = new Frequency2IGenerator(children);

        }

        public override void enterFrame(double dt)
        {
            if (KeyHandler.Instance.isKeyPressed(Key.Q) && StateManager.Instance.CurrentState.Equals(SubmarineStates.LEVEL_STATE))
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

        public override void shutdown()
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
            bg = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background;

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

            ButtonIcon btnFull = new ButtonIcon();
            btnFull.TextContent.Text = "Full Screen Mode";
            btnFull.Icon.Source = ResourceHelper.GetBitmap("Media/fullscreen.png");
            btnFull.Icon.Height = 22;
            btnFull.Icon.Width = 31;
            btnFull.Width = 150;
            btnFull.Height = 40;
            btnFull.SetValue(Canvas.LeftProperty, 50.0);
            btnFull.SetValue(Canvas.TopProperty, 50.0);
            btnFull.Click += delegate(object sender, RoutedEventArgs e)
            {
                AuditoryGameApp.Current.Host.Content.IsFullScreen = !AuditoryGameApp.Current.Host.Content.IsFullScreen;
            };

           

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnFull);

            ButtonIcon btnOption = new ButtonIcon();
            btnOption.TextContent.Text = "Options";
            btnOption.Icon.Source = ResourceHelper.GetBitmap("Media/fullscreen.png");
            btnOption.Icon.Height = 22;
            btnOption.Icon.Width = 31;
            btnOption.Width = 150;
            btnOption.Height = 150;
            btnOption.SetValue(Canvas.LeftProperty, 50.0);
            btnOption.SetValue(Canvas.TopProperty,250.0);
            btnOption.Click += delegate(object sender, RoutedEventArgs e)
            {
                StateManager.Instance.setState(SubmarineStates.OPTION_STATE);
            };

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnOption);

        }

        private void endMainMenu()
        {
            base.removeAllCanvasChildren();
        }


        private void startGame()
        {
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(255,0,67,171));
            //MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
            //_synthEx = new Frequency2IGenerator(children);
            _synthEx.Stop();

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

 
            double perc = _random.NextDouble();
            _gate = new GateObject();
            _gate.startupGameObject(
                new Point(21, 60),
                "Media/wall.png",
                ZLayers.BACKGROUND_Z + 5);
            _gate.Position = new Point(dim.X - 21, (dim.Y - 60) * perc);

            _wall = new WallObject();
            _wall.startupGameObject(
                new Point(20, dim.Y),
                "Media/wall.png",
                ZLayers.BACKGROUND_Z);
            _wall.Position = new Point(dim.X - 20, 0);


            GameLevelDescriptor.Attach(AuditoryGameApp.Current.RootVisual as GamePage);
            GameLevelDescriptor.Debug();


            ///_synthEx.Arpeggiator.Notes[0].Frequency = 5000;
            ///_synthEx.Arpeggiator.Notes[2].Frequency = (float)(5000 - 50*(_gate.Position.Y - _submarine.Position.Y)/10.0);
            ///_synthEx.Arpeggiator.Start();
            //_synth.TriggerNote(new Note(Notes.F, 5));
            this._synthEx.ResetSequencer();
            this._synthEx.SetTrainingFrequency(5000);
            this._synthEx.SetTargetFrequency((5000 - 50 * (_gate.Position.Y - _submarine.Position.Y) / 10.0),true);
            this._synthEx.Start();


        }
        private void exitGame()
        {
            this._synthEx.Stop(); 
            while (GameObject.gameObjects.Count != 0)
                GameObject.gameObjects[0].shutdown();

            removeAllCanvasChildren();
            UIElementCollection children = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children;
            while (children.Count != 0)
                children.RemoveAt(0);

            //_synthEx.Arpeggiator.Stop();
            //txtbScore = null;
            sp1.Stop();
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background = bg;

        }


        #region Application Manager 

        private void startOptions()
        {
            SubmarineOptions panel = new SubmarineOptions();
           // UserModelEditor panel = new UserModelEditor();
           // panel.AddModel(UserModel.Beginner());
            panel.SetValue(Canvas.LeftProperty, 50.0);
            panel.SetValue(Canvas.TopProperty, 50.0);
          /*  panel._ValidateModelHook += delegate() 
            {
                StateManager.Instance.setState(States.START_STATE);
            };*/

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(panel);

        }

        private void exitOptions()
        {
            removeAllCanvasChildren();
        }

        #endregion

    }

}
