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
using GameFramework.UI;
using System.Collections.ObjectModel;
using System.Collections.Generic;



namespace LSRI.Submarine
{

    /// <summary>
    /// 
    /// </summary>
    /// @see States
    public class SubmarineStates : States
    {
        public const string LEVEL_STATE = "start_Level";    ///< Starting a new level @deprecated  Not in use, see 
        public const string OPTION_STATE = "start_option";  ///< Starting the option page
        public const string LOG_STATE = "start_log";        ///< Starting the log page
    }

  /*  public static class GameLevelDescriptor
    {
        private static TextBlock _debugUI = null;
        public static int CurrentGate { get; set; }
        public static int TrainingFrequency { get; set; }
        public static double ComparisonFrequency { get; set; }
        public static int ThresholdFrequency { get; set; }

        private static UserModelContainer _container = new UserModelContainer();
        private static AuditoryModel _auditory = new AuditoryModel();
        private static GameOptions _gOption = new GameOptions();

        public static GameOptions Game
        {
            get
            {
                return _gOption;
            }
            set
            {
                _gOption = value;
            }
        }

        public static AuditoryModel Auditory
        {
            get
            {
                return _auditory;
            }
            set
            {
                _auditory = value;
            }
        }

        public static ObservableCollection<UserModel> UserLists
        {
            get
            {
                return _container.UserModels;
            }
        }

        public static UserModel CurrentModel
        {
            get
            {
                return _container.CurrentModel;
            }
        }

        public static int CurrentLevel {
            get
            {
                return _container.CurrentModel.CurrentLevel;

            }
            set
            {
                _container.CurrentModel.CurrentLevel = value;
            }
        }


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
                _debugUI.SetValue(Canvas.TopProperty, 0.0);
            }
            // we have to insert any non GameObjects at the end of the children collection
            pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, _debugUI);
        }

        public static void Debug()
        {
            if (_debugUI == null) return;
            _debugUI.Text = String.Format(
                "Training Fq : {0} Hz\n"+
                "Delta       : {1} Hz\n-----\n" +
                "Comparison  : {4} Hz\n-----\n" +
                "Level       : {2}\n" +
                "Gates       : {3}",
                TrainingFrequency, 
                ThresholdFrequency,
                CurrentLevel,
                CurrentGate,
                ComparisonFrequency);
        }
    }*/


    /// <summary>
    /// General wrapper for the submarine game configuration
    /// </summary>
    public sealed class SubOptions : IConfigurationManager
    {
        /// <summary>
        /// private subclass to handle the 
        /// </summary>
        private class Nested
        {
            /// <summary>
            /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            /// </summary>
            static Nested() {}
            internal static readonly SubOptions instance = new SubOptions();
        }
        
        private UserModelContainer _container = new UserModelContainer();
        private AuditoryModel _auditory = new AuditoryModel();
        private GameOptions _gOption = new GameOptions();


        public string Beat { set; get; }

        /// <summary>
        /// Access to the game configuration
        /// </summary>
        public GameOptions Game
        {
            get
            {
                return _gOption;
            }
            set
            {
                _gOption = value;
            }
        }

        /// <summary>
        /// Access to the auditory system configuration
        /// </summary>
        public AuditoryModel Auditory
        {
            get
            {
                return _auditory;
            }
            set
            {
                _auditory = value;
            }
        }

        /// <summary>
        /// Access to the list of users
        /// </summary>
        public ObservableCollection<UserModel> UserLists
        {
            get
            {
                return _container.UserModels;
            }
        }

        /// <summary>
        /// Access to the current user
        /// </summary>
        public UserModel User
        {
            get
            {
                return _container.CurrentModel;
            }
        }

        public static SubOptions Instance
        {
            get
            {
                return Nested.instance;
            }
        }


        /// <summary>
        /// Save the configuration into isolated storage
        /// </summary>
        public void SaveConfiguration()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Load configuration from the isolated storage
        /// </summary>
        public void RetrieveConfiguration()
        {
            throw new NotImplementedException();
        }

        #region Debug Display Mode

        private static TextBlock _debugUI = null;   ///< placeholder for the debug panel

        /// <summary>
        /// Add the debug information panel on the game page
        /// </summary>
        /// <param name="pg">A reference to the game's main page</param>
        public void AttachDebug(GamePage pg)
        {
            if (pg == null) return;
            if (_debugUI == null)
            {
                _debugUI = new TextBlock();
                _debugUI.Text = "### Debug information ###";
                _debugUI.Name = "txtbScore";
                _debugUI.Width = 100;
                _debugUI.Height = 35;
                _debugUI.FontSize = 9;
                _debugUI.FontFamily = new FontFamily("Courier New");
                _debugUI.Foreground = new SolidColorBrush(Colors.White);
                _debugUI.SetValue(Canvas.LeftProperty, 10.0);
                _debugUI.SetValue(Canvas.TopProperty, 10.0);
            }

            // we have to insert any non GameObjects at the end of the children collection
            pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, _debugUI);
        }

        /// <summary>
        /// Update the content of the debug panel
        /// </summary>
        public void UpdateDebug()
        {
            if (_debugUI == null) return;
            _debugUI.Text = String.Format(
                "Training Fq : {0} Hz\n" +
                "Delta       : {1} Hz\n" + 
                "-----\n" +
                "Comparison  : {2} Hz\n" +
                "-----\n" +
                "Level       : {3}\n" +
                "Gates       : {4}\n\n" + 
                "{5}",
                this.User.FrequencyTraining,
                this.User.FrequencyDelta,
                this.User.FrequencyComparison,
                this.User.CurrentLevel,
                this.User.CurrentGate,
                this.Beat
                );

        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public class SubmarineApplicationManager : IAppManager
    {
        public SubmarinePlayer _submarine = null;   ///< Reference to the submarine object
        public GateAnimatedObject _gate = null;     ///< Reference to the gate object
        public WallObject _wall = null;             ///< Reference to the wall object
        public SubmarineToolbox tbox = null;

        public Frequency2IGenerator _synthEx = null;    ///< Reference to the the auditory stimuli generator

        private Random _random; ///< Random number generation
        private StopwatchPlus _watch;
        private Brush bg = null;

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

        public StopwatchPlus Logger
        {
            get
            {
                return _watch;
            }
            set
            {
                _watch = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected SubmarineApplicationManager()
        {
            KeyHandler.Instance.IskeyUpOnly = false;
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


            /// Associate the stimuli generator with the media element of the game page
            MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
            _synthEx = new Frequency2IGenerator(children,SubOptions.Instance.Auditory.BufferLength);
            this._synthEx.Sequencer._freqChangedHook += new SequencerExt.FrequencyChanged(Sequencer__freqChangedHook);
            this._synthEx.Sequencer._freqPlayedHook += new SequencerExt.FrequencyPlayed(Sequencer__freqStartHook);
            this._synthEx.Sequencer._freqStoppedHook += new SequencerExt.FrequencyStopped(Sequencer__freqStopHook);
            this._synthEx.Sequencer._stepEndedHook +=new SequencerExt.StepEnded(Sequencer__stepEndedHook);
            _synthEx.Stop();

        }

        void Sequencer__stepEndedHook()
        {

            this._synthEx.SetSignalDelay(2);
        }
        void Sequencer__freqChangedHook(int idx,double fq)
        {
            if (this.Logger!=null) 
                this.Logger.Step(String.Format("\t[AUD] \t Changed stimuli {0} : {1}", idx + 1, fq));
            SubOptions.Instance.User.FrequencyComparison = fq;

        }

        void Sequencer__freqStartHook(string msg)
        {
            if (this.Logger != null) this.Logger.Step(msg);
            SubOptions.Instance.Beat = "●●●●●●●●●●";
        }
        void Sequencer__freqStopHook(string msg)
        {
            if (this.Logger != null) this.Logger.Step(msg);
            SubOptions.Instance.Beat = "";
        }


        public override void enterFrame(double dt)
        {
            if (KeyHandler.Instance.isKeyPressed(Key.Q) && StateManager.Instance.CurrentState.Equals(SubmarineStates.LEVEL_STATE))
                StateManager.Instance.setState(States.START_STATE);

            // cheat code for showing the gate
            if (KeyHandler.Instance.isKeyPressed(Key.G))
            {
                if (_gate==null) return;
                Visibility isVis = _gate.Visibility;
                if (isVis == Visibility.Visible)
                    _gate.Visibility = Visibility.Collapsed;
                else
                    _gate.Visibility = Visibility.Visible;
                KeyHandler.Instance.clearKeyPresses();
            }


            /// Check for visibility of gate
            double r = (_gate.Position.X-_submarine.Position.X) / _gate.Position.X;
            if (SubOptions.Instance.User.IsGateVisible(r))
            {
                if (_gate.Visibility != Visibility.Visible)
                    _gate.Visibility = Visibility.Visible;
            }

            SubOptions.Instance.UpdateDebug(); 

                            

        }

        public override void shutdown()
        {
            //SavedScore = Score;
        }


        #region Application State: Main Menu

        private void startMainMenu()
        {
            // initialise toolbox
            this.tbox = new SubmarineToolbox()
            {
                FullMode = false
            };
            tbox.SetValue(Canvas.LeftProperty, 0.0);
            tbox.SetValue(Canvas.TopProperty, 0.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children.Add(tbox);

            HighScoreControl ct = new HighScoreControl();
            ct.SetValue(Canvas.LeftProperty, 350.0);
            ct.SetValue(Canvas.TopProperty, 50.0);
            ct.Source.ItemsSource = SubOptions.Instance.User.Scores.Data;

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(ct);

            StartLevelPanel pp = new StartLevelPanel();
            pp.CurrentLevel = SubOptions.Instance.User.CurrentLevel;
            pp.CurrentGate = SubOptions.Instance.User.CurrentGate;

            pp.SetValue(Canvas.LeftProperty, 10.0);
            pp.SetValue(Canvas.TopProperty, 50.0);
            pp.StartBtn.Click += delegate(object sender, RoutedEventArgs e)
            {
                StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(pp);


            ButtonIcon btnFull = new ButtonIcon();
            btnFull.TextContent.Text = "Full Screen Mode";
            btnFull.Icon.Source = ResourceHelper.GetBitmap("Media/fullscreen.png");
            //btnFull.Icon.Height = 22;
            //btnFull.Icon.Width = 31;
            btnFull.Width = 150;
            btnFull.Height = 40;
            btnFull.SetValue(Canvas.LeftProperty, 50.0);
            btnFull.SetValue(Canvas.TopProperty, 350.0);
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
            btnOption.Height = 40;
            btnOption.SetValue(Canvas.LeftProperty, 50.0);
            btnOption.SetValue(Canvas.TopProperty, 400.0);
            btnOption.Click += delegate(object sender, RoutedEventArgs e)
            {
                StateManager.Instance.setState(SubmarineStates.OPTION_STATE);
            };

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnOption);
            
            /*  Button btnStart = new Button();
              btnStart.Content = "Start Game";
              btnStart.Width = 100;
              btnStart.Height = 35;
              btnStart.SetValue(Canvas.LeftProperty, 490.0);
              btnStart.SetValue(Canvas.TopProperty, 355.0);
              btnStart.Click += delegate(object sender, RoutedEventArgs e) { 
                  StateManager.Instance.setState(SubmarineStates.LEVEL_STATE); 
              };
              (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnStart);
             */


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


            _synthEx.Stop();
 

        }

        private void endMainMenu()
        {
            base.removeAllCanvasChildren();
            _synthEx.Stop();
        }

        #endregion

        #region Application State: Game

        private void startGame()
        {
            SubOptions.Instance.AttachDebug(AuditoryGameApp.Current.RootVisual as GamePage);

            /*sp1 = new StopwatchPlus(
                    sw => Debug.WriteLine("Game Started"),
                    sw => Debug.WriteLine("Time! {0}", sw.EllapsedMilliseconds),
                    sw => Debug.WriteLine("totot {0}", sw.EllapsedMilliseconds)

                );*/

            this.Logger = new StopwatchPlus(
                            sw => Debug.WriteLine("Submarine starts \t TIMESTAMP = {0}", sw.EllapsedMilliseconds),
                            sw => Debug.WriteLine("Submarine stops \t TIMESTAMP = {0}", sw.EllapsedMilliseconds),
                            (sw, msg) => Debug.WriteLine("{0} \t TIMESTAMP = {1} ms", msg, sw.EllapsedMilliseconds)
            );

            // stop audio (just in case)
            //_synthEx.Stop();
            
            // initialise collisions
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMY);
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMYWEAPON);

            // initialise toolbox
            this.tbox = new SubmarineToolbox()
            {
                FullMode = true,
                Life = SubOptions.Instance.User.CurrentLife,
                Gate = SubOptions.Instance.User.CurrentGate,
                Gates = SubOptions.Instance.Game.MaxGates,
                Score = SubOptions.Instance.User.CurrentScore,
                Level = SubOptions.Instance.User.CurrentLevel
            };
            tbox.SetValue(Canvas.LeftProperty, 0.0);
            tbox.SetValue(Canvas.TopProperty, 0.0);

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children.Add(tbox);

            // initialise game layout and position grid
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(255, 0, 67, 171));

            Canvas zone = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot;
            Point dim = new Point(zone.ActualWidth, zone.ActualHeight);

            int stepSize = SubOptions.Instance.Game.UnitSize;
            int nbUnitsInScreen = (int)dim.Y / stepSize;
            int screenMargin = ((int)dim.Y % stepSize)/2;
            int gateExtent = SubOptions.Instance.Game.GateSize;
            int gateSize = 1+2*SubOptions.Instance.Game.GateSize;

            int bias = 2;
            int Gatepos = _random.Next(bias+gateExtent, nbUnitsInScreen - bias - gateExtent);
            int Subpos = _random.Next(bias+gateExtent, nbUnitsInScreen - bias - gateExtent);

            //Gatepos = nbUnitsInScreen - bias - gateExtent;
          //  Subpos = nbUnitsInScreen - bias - gateExtent;

            double GateLoc = screenMargin + (Gatepos - gateExtent) * stepSize;
            double SubLoc = screenMargin + Subpos * stepSize;

            // create submarine object
            _submarine = new SubmarinePlayer() { CanvasIndex = Subpos };
            _submarine.startupSubmarine(new Point(48, 32), ZLayers.PLAYER_Z);

            // create gate object
            _gate = new GateAnimatedObject() { CanvasIndex = Gatepos };
            _gate.startupGameObject(
                new Point(25, gateSize * stepSize),
                ZLayers.BACKGROUND_Z + 5);

            // create wall object
            _wall = new WallObject();
            _wall.startupGameObject(
                new Point(20, dim.Y),
                "Media/wall.png",
                ZLayers.BACKGROUND_Z);
            _wall.Position = new Point(dim.X - 20, 0);

            _gate.Position = new Point(dim.X - 25, GateLoc);

            _submarine.Position = new Point(0, SubLoc - _submarine.Dimensions.Y / 2 + stepSize / 2);

           // _submarine.CanvasIndex = _gate.Dimensions.Y / 2 - _submarine.Dimensions.Y / 2;

            double theodur = (_wall.Position.X - _submarine.Dimensions.X) / _submarine.Speed;
            //Debug.WriteLine("Theoretical timing : {0}", theodur*1000);

            // initialise auditory stimuli
            double fqTraining = SubOptions.Instance.User.FrequencyTraining;
            double deltaf = SubOptions.Instance.User.FrequencyDelta;

            double deltapos = Gatepos - Subpos;
            //double deltaf = fqDiff;//  fqTraining * .2;
            double dfpix = deltaf / SubOptions.Instance.Game.GateSize;
            Debug.WriteLine("*********** Gatepos = {0}", Gatepos);

            double fqComp = fqTraining - dfpix * deltapos;
            if (fqComp >= SubOptions.Instance.Auditory.MaxFrequency) fqComp = SubOptions.Instance.Auditory.MaxFrequency;
            if (fqComp <= SubOptions.Instance.Auditory.MinFrequency) fqComp = SubOptions.Instance.Auditory.MinFrequency;
            SubOptions.Instance.User.FrequencyComparison = fqComp;

            this._synthEx.ResetSequencer();
            this._synthEx.SetTrainingFrequency(fqTraining);
            this._synthEx.SetTargetFrequency(SubOptions.Instance.User.FrequencyComparison, true);

           
            SubOptions.Instance.UpdateDebug();

            this._synthEx.Start();
        }
        private void exitGame()
        {
            this._synthEx.Stop(); 
            while (GameObject.gameObjects.Count != 0)
                GameObject.gameObjects[0].shutdown();

            removeAllCanvasChildren();

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background = bg;

        }

        #endregion

        #region Application State: Options

        private void startOptions()
        {
            SubmarineOptionPanel panel = new SubmarineOptionPanel();
            panel.SetValue(Canvas.LeftProperty, 10.0);
            panel.SetValue(Canvas.TopProperty, 10.0);

            panel.OnCompleteTask += delegate(SubmarineOptionPanel.CompleteTaskArgs arg) 
            {
                this._synthEx.Sequencer._freqChangedHook -= new SequencerExt.FrequencyChanged(Sequencer__freqChangedHook);
                this._synthEx.Sequencer._freqPlayedHook -= new SequencerExt.FrequencyPlayed(Sequencer__freqStartHook);
                this._synthEx.Sequencer._freqStoppedHook -= new SequencerExt.FrequencyStopped(Sequencer__freqStopHook);
                this._synthEx.Sequencer._stepEndedHook -= new SequencerExt.StepEnded(Sequencer__stepEndedHook);

                
                MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
                //children = new MediaElement();
                int bug = SubOptions.Instance.Auditory.BufferLength;
                _synthEx = new Frequency2IGenerator(children,bug);
                this._synthEx.Sequencer._freqChangedHook += new SequencerExt.FrequencyChanged(Sequencer__freqChangedHook);
                this._synthEx.Sequencer._freqPlayedHook += new SequencerExt.FrequencyPlayed(Sequencer__freqStartHook);
                this._synthEx.Sequencer._freqStoppedHook += new SequencerExt.FrequencyStopped(Sequencer__freqStopHook);
                this._synthEx.Sequencer._stepEndedHook += new SequencerExt.StepEnded(Sequencer__stepEndedHook);
                _synthEx.Start();
                _synthEx.Stop();
                StateManager.Instance.setState(States.START_STATE);
            };

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(panel);

        }

        private void exitOptions()
        {
            removeAllCanvasChildren();
            _synthEx.Stop();
        }

        #endregion

    }

}

