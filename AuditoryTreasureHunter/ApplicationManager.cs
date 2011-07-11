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
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using LSRI.AuditoryGames.AudioFramework;
using LSRI.AuditoryGames.GameFramework;
using LSRI.TreasureHunter.UI;
using LSRI.TreasureHunter.Model;
using LSRI.AuditoryGames.GameFramework.Data;
using LSRI.AuditoryGames.GameFramework.UI;
using System.Collections;

namespace LSRI.TreasureHunter
{

    public class TreasureHunterLogger : GameLogger
    {

        /// <summary>
        /// Event name for the Submarine hitting the end of the game scene (whether the gate or the wall)
        /// </summary>
        protected static readonly string LOG_HITNUGGET = "HIT_NUGGET";

        private static readonly string TREASURE_STORAGE_CSVFILENAME = @"logger_treasure.csv";
        private DateTime _startGame = DateTime.Now;
        private DateTime _startLevel = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The filename of the CSV file where the log will be saved</returns>
        public override string getStorageName()
        {
            return TreasureHunterLogger.TREASURE_STORAGE_CSVFILENAME;
        }

        /// <summary>
        /// output 
        /// - $date$,$time$,<b>GAME_STARTED</b>,$duration$,$username$,$training$,$delta$ 
        /// 
        /// where
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: always 0
        /// - <b>$username$</b>: the name of the current user
        /// - <b>$training$</b>: the training frequency (Hz) of the user
        /// - <b>$delta$</b>: the current frequency delta (Hz) of the user
        /// </summary>
        public override void logGameStarted()
        {
            _startGame = DateTime.Now;
            String[] par = {
                    0.ToString(),
                    TreasureOptions.Instance.User.Name.Replace(",","_"),
                    TreasureOptions.Instance.User.FrequencyTraining.ToString(),
                    TreasureOptions.Instance.User.FrequencyDelta.ToString()
                           };

            string strParam = String.Join(",", par);
            WriteLogFile(_startGame, GameLogger.LOG_GAMESTARTED, strParam);
        }

        /// <summary>
        /// output 
        /// - $date$,$time$,<b>GAME_ENDED</b>,$duration$
        /// 
        /// where
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: time elapsed (HH:MM:SS.0000) since the previous GAME_STARTED event
        /// </summary>
        public override void logGameEnded()
        {
            DateTime _now = DateTime.Now;
            TimeSpan elapsed = _now - _startGame;
            String[] par = {
                    elapsed.ToString()
                           };
            WriteLogFile(_now, GameLogger.LOG_GAMEENDED, String.Join(",", par));
        }

        /// <summary>
        /// output 
        /// - $date$,$time$,<b>LEVEL_STARTED</b>,0,$level$,$training$,$delta$
        /// 
        /// where
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: always 0
        /// - <b>$level$</b>: the current level of the game
        /// - <b>$training$</b>: the training frequency (Hz) of the user
        /// - <b>$delta$</b>: the current frequency delta (Hz) of the user
        /// </summary>
        public override void logLevelStarted()
        {
            _startLevel = DateTime.Now;
            DateTime _now = DateTime.Now;
            TimeSpan elapsed = _now - _now;
            String[] par = {
                    elapsed.ToString(),
                    TreasureOptions.Instance.User.CurrentLevel.ToString(),
                    TreasureOptions.Instance.User.FrequencyTraining.ToString(),
                    TreasureOptions.Instance.User.FrequencyDelta.ToString()
                          };
            WriteLogFile(_now, GameLogger.LOG_LEVELSTARTED, String.Join(",", par));
        }

        /// <summary>
        /// output 
        /// - $date$,$time$,<b>LEVEL_ENDED</b>,$duration$,$outcome$,$score$
        /// 
        /// where
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: time elapsed (HH:MM:SS.0000) since the previous LEVEL_STARTED event
        /// - <b>$outcome$</b>: SUCCESS if level completed, FAIL if not, CANCEL if game prematurely stopped
        /// - <b>$score$</b>: the current score of the user (might have a positive value even if level has failed)
        /// </summary>
        public override void logLevelEnded(int win)
        {
            DateTime _now = DateTime.Now;
            TimeSpan elapsed = _now - _startLevel;
            String strWin = "";
            switch (win)
            {
                case 0: strWin = "FAIL";
                    break;
                case 1: strWin = "SUCCESS";
                    break;
                default:
                    strWin = "CANCEL";
                    break;
            }
            String[] par = {
                    elapsed.ToString(),
                    strWin,
                    TreasureOptions.Instance.User.CurrentScore.ToString()
                          };
            WriteLogFile(_now, GameLogger.LOG_LEVELENDED, String.Join(",", par));
        }

        /// <summary>
        /// output 
        /// - $date$,$time$,<b>HIT_NUGGET</b>,$duration$,$outcome$,$left$,$center$,$right$
        /// 
        /// where
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: time elapsed (HH:MM:SS.0000) since the previous LEVEL_STARTED event
        /// - <b>$outcome$</b>: nature of the nugget blasted; can be GOLD, COAL or EMPTY (i.e. already blasted out)
        /// - <b>$left$</b>: the frequency played on the left BEFORE the nugget was blasted out
        /// - <b>$center$</b>: the frequency played on the middle BEFORE the nugget was blasted out
        /// - <b>$right$</b>: the frequency played on the right BEFORE the nugget was blasted out
        /// </summary>
        public virtual void logHitNugget(int win, double Fq)
        {
            DateTime _now = DateTime.Now;
            TimeSpan elapsed = _now - _startLevel;

            //WriteLogFile(_now, SubmarineLogger.LOG_HITNUGGET, String.Join(",", par));
        }


    }

    /// <summary>
    /// Definition of the logical states specific to the TresureHunter
    /// </summary>
    /// @see States
    public class TreasureStates : States
    {
        public const string LEVEL_STATE = "start_Level";    ///< Starting a new level @deprecated  Not in use, see 
        public const string OPTION_STATE = "start_option";  ///< Starting the option page
        public const string LOG_STATE = "start_log";        ///< Starting the log page
        public const string SCORE_STATE = "start_score";    ///< Starting the end-of-level score dialog
    }

     /// <summary>
     /// Main entry point of the TreasureHunter game.
     /// 
     /// The TreasureHunter Application Manager is the functional core of the game and is responsible for piecing 
     /// together the various elements of the game: logical states, game objects, etc.
     /// </summary>
    public class TreasureApplicationManager : IAppManager
    {
        /// <summary>
        /// Various constants used in defining the layout of the main game scene 
        /// </summary>
        private static class GameLayout
        {
            /// <summary>
            ///  Height (in px) to keep for the sky
            /// </summary>
            public static int MARGIN_SKY = 200;      

            /// <summary>
            /// Margin (in px) for the nuggets
            /// </summary>
            public static int MARGIN_NUGGETS = 40;   
        }
        
         /// <summary>
        /// Indicates whether changes of the audio signals should be made or not.
        /// 
        /// @todo change the static flag into a proper state (TreasureHunterPlayer)
        /// </summary>
        public static Boolean PREVENT_AUDIO_CHANGES = false;

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random _rand = new Random((int)DateTime.Now.Ticks);
        public TreasureHunterLogger myLogger = new TreasureHunterLogger();

        public Frequency3IGenerator _synthEx = null;

        public TreasureToolbox _scorePanel = null;
        private ButtonIcon btnOption = null;

        /// <summary>
        /// 
        /// </summary>
        protected HunterPlayer _player = null;
        public List<TreasureNugget> _nuggets = new List<TreasureNugget>();


        private int[] depthArray = null;//new int[TreasureOptions.Instance.Game._curGold];
        private int[] scoreArray = null;//new int[TreasureOptions.Instance.Game._curGold];


        /// <summary>
        /// 
        /// </summary>
        public new static IAppManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TreasureApplicationManager();
                return _instance;
            }
        }


        protected TreasureApplicationManager()
        {
            KeyHandler.Instance.IskeyUpOnly = true;

        /*    _synthEx = new Frequency3IGenerator();

            if (_synthEx.FrequencyTimer != null)
            {
                _synthEx.FrequencyTimer.stimuliStarted += new FrequencyTimer.StimuliStarted(AppMgr_StimuliStarted);
                _synthEx.FrequencyTimer.stimuliStopped += new FrequencyTimer.StimuliStopped(AppMgr_StimuliStopped);
            }*/

            TreasureOptions.Instance.RetrieveConfiguration();
        }

        public override void shutdown()
        {
            SavedScore = Score;
            TreasureOptions.Instance.SaveConfiguration();
        }

      /*  private void AppMgr_StimuliStarted()
        {
            //Debug.WriteLine("---> AUDIO CUE started " + DateTime.Now.ToString("HH:MM:ss.FFFFFF"));
            PREVENT_AUDIO_CHANGES = true;
        }

        private void AppMgr_StimuliStopped()
        {
            Debug.WriteLine("---> AUDIO CUE stopped" + DateTime.Now.ToString("HH:MM:ss.FFFFFF"));
            PREVENT_AUDIO_CHANGES = false;
        }*/

        public override void startupApplicationManager()
        {
            MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
            _synthEx = new Frequency3IGenerator(children);

            StateManager.Instance.registerStateChange(
                TreasureStates.START_STATE,
                new StateChangeInfo.StateFunction(startMainMenu),
                new StateChangeInfo.StateFunction(endMainMenu));

            StateManager.Instance.registerStateChange(
                TreasureStates.LEVEL_STATE,
                new StateChangeInfo.StateFunction(startGame),
                new StateChangeInfo.StateFunction(exitGame));

            StateManager.Instance.registerStateChange(
                TreasureStates.OPTION_STATE,
                new StateChangeInfo.StateFunction(startOptions),
                new StateChangeInfo.StateFunction(exitOptions));


            StateManager.Instance.registerStateChange(
                TreasureStates.SCORE_STATE,
                new StateChangeInfo.StateFunction(startScore),
                new StateChangeInfo.StateFunction(exitScore));


            //Score = SavedScore;
            //(App.Current.RootVisual as Page).AudioPlayer.SetSource(_synthEx);
        }

        public override void enterFrame(double dt)
        {
            //if (KeyHandler.Instance.isKeyPressed(Key.Escape) && StateManager.Instance.CurrentState.Equals(TreasureStates.LEVEL_STATE))
            //    StateManager.Instance.setState(States.START_STATE);
            if (StateManager.Instance.CurrentState.Equals(TreasureStates.LEVEL_STATE))
            {
                if (KeyHandler.Instance.isKeyPressed(Key.Q))
                {
                    StateManager.Instance.setState(TreasureStates.START_STATE);
                    return;
                }

                if (KeyHandler.Instance.isKeyPressed(Key.Add) && StateManager.Instance.CurrentState.Equals(TreasureStates.LEVEL_STATE))
                {
                    TreasureOptions.Instance.User._currExposure++;
                    if (TreasureOptions.Instance.User._currExposure >= 4) TreasureOptions.Instance.User._currExposure = 4;
                    changeExposure();
                }

                if (KeyHandler.Instance.isKeyPressed(Key.Subtract) && StateManager.Instance.CurrentState.Equals(TreasureStates.LEVEL_STATE))
                {
                    TreasureOptions.Instance.User._currExposure--;
                    if (TreasureOptions.Instance.User._currExposure <= 0) TreasureOptions.Instance.User._currExposure = 0;
                    changeExposure();
                }

                if (KeyHandler.Instance.isKeyPressed(Key.Z) && StateManager.Instance.CurrentState.Equals(TreasureStates.LEVEL_STATE))
                {
                    TreasureOptions.Instance.Game.Display = TreasureGame.DisplayMode.Content;
                    changeExposure();
                }
                if (KeyHandler.Instance.isKeyPressed(Key.X) && StateManager.Instance.CurrentState.Equals(TreasureStates.LEVEL_STATE))
                {
                    TreasureOptions.Instance.Game.Display = TreasureGame.DisplayMode.Position;
                    changeExposure();
                }
                TreasureOptions.Instance.UpdateDebug();
            }
            else if (StateManager.Instance.CurrentState.Equals(TreasureStates.START_STATE))
            {
                ModifierKeys keys = Keyboard.Modifiers;
                bool controlKey = (keys & ModifierKeys.Control) != 0;
                bool altKey = (keys & ModifierKeys.Alt) != 0;
                if (controlKey && altKey)
                {
                    btnOption.Visibility = Visibility.Visible;
                }
                else
                {
                    if (btnOption.Visibility == Visibility.Visible)
                        btnOption.Visibility = Visibility.Collapsed;
                }
            }
      
  
        }


        public void changeExposure()
        {
            double delta = TreasureOptions.Instance.User.VisualTiming.Data[TreasureOptions.Instance.User._currExposure];
            TreasureOptions.Instance.nExposedX = (int)(delta * TreasureOptions.Instance.Game.Zones);
            TreasureOptions.Instance.nExposedY = (int)(delta * TreasureOptions.Instance.Game.Depth);

            double nb = TreasureOptions.Instance.Game.Zones * (1.5);
            int idx = Math.Min(4,TreasureOptions.Instance.User.CurrentGold);
            double mod = TreasureOptions.Instance.User.VisualTiming.Data[idx];
            double fog = Math.Max(1,(nb - TreasureOptions.Instance.User.Actions)*mod);
            for (int i = 0; i < _nuggets.Count; i++)
            {
                TreasureNugget tt = _nuggets[i];
                tt.ChangeExposure(true);
              //  if (tt.Depth > TreasureOptions.Instance.nExposedY)
              //      tt.ChangeExposure(false);
                if  (Math.Abs(_player.CurrentZone - tt.Index) > fog/*TreasureOptions.Instance.nExposedX*/)
                    tt.ChangeExposure(false);
            }
        }

        public void UpdateSound(int clearIdx)
        {
            double freqL = 0;
            double freqM = 0;
            double freqR = 0;

            if (TreasureOptions.Instance.User.FrequencyDelta <= 0)
                TreasureOptions.Instance.User.FrequencyDelta = TreasureOptions.Instance.User.FrequencyTraining * TreasureOptions.Instance.Auditory.Base;

            double fqTrain = TreasureOptions.Instance.User.FrequencyTraining;
            double fqComp = TreasureOptions.Instance.User.FrequencyTraining - TreasureOptions.Instance.User.FrequencyDelta;
            TreasureOptions.Instance.User.FrequencyComparison = fqComp;
            if (clearIdx != -1)
            {
                scoreArray[clearIdx] = 0;
            }
            if (TreasureOptions.Instance.Game.Detection == TreasureGame.DetectionMode.Proximity)
            {
                freqL = ((_player.CurrentZone - 1) >= 0 && TreasureOptions.Instance.Game._curSetup[_player.CurrentZone - 1] == '1') ? fqTrain : fqComp;
                freqM = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone] == '1') ? fqTrain : fqComp;
                freqR = (((_player.CurrentZone + 1) < TreasureOptions.Instance.Game._curSetup.Length) &&
                                TreasureOptions.Instance.Game._curSetup[_player.CurrentZone + 1] == '1') ? fqTrain : fqComp;
            }
            else if (TreasureOptions.Instance.Game.Detection == TreasureGame.DetectionMode.Distance)
            {
                String left = TreasureOptions.Instance.Game._curSetup.Substring(0, _player.CurrentZone);
                String right = TreasureOptions.Instance.Game._curSetup.Substring(_player.CurrentZone + 1);
                int nbL = left.LastIndexOf('1');
                int nbR = right.IndexOf('1');
                if (nbL != -1) nbL = left.Length - nbL - 1;
                else nbL += 100001;
                if (nbR == -1) nbR += 100001;


                freqL = (nbL <= nbR) ? fqTrain : fqComp;
                freqM = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone] == '1') ? fqTrain : fqComp;
                freqR = (nbL >= nbR) ? fqTrain : fqComp;
            }
            else if (TreasureOptions.Instance.Game.Detection == TreasureGame.DetectionMode.Value)
            {
                String left = TreasureOptions.Instance.Game._curSetup.Substring(0, _player.CurrentZone);
                String right = TreasureOptions.Instance.Game._curSetup.Substring(_player.CurrentZone + 1);
                int nbL = 0;
                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i] == '1') nbL++;
                }
                int nbR = 0;
                for (int i = 0; i < right.Length; i++)
                {
                    if (right[i] == '1') nbR++;
                }

                nbL = 0;
                nbR = 0;
                for (int i = 0; i < _player.CurrentZone; i++)
                {
                    nbL += scoreArray[i];
                }
                for (int i = _player.CurrentZone+1; i < scoreArray.Length; i++)
                {
                    nbR += scoreArray[i];
                }

                if (nbL==0) 
                    freqL = fqComp;
                else
                    freqL = (nbL >= nbR ) ? fqTrain : fqComp;
                freqM = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone] == '1') ? fqTrain : fqComp;
                if (nbR == 0)
                    freqR = fqComp;
                else
                    freqR = (nbL <= nbR) ? fqTrain : fqComp;


            }


            /*List<Note> newnote = new List<Note>(_synthEx.Arpeggiator.Notes);
            FrequencyGenerator a = (_synthEx as FrequencyGenerator);
            newnote[0].Frequency = freqL;
            newnote[2].Frequency = freqM;
            newnote[4].Frequency = freqR;

            a.updateNotes(newnote);
            _synthEx.Arpeggiator.Start();*/
            this._synthEx.ResetSequencer(freqL, freqM, freqR);
            this._synthEx.Start();
        }

        #region Application State: Main Menu

        private void GenerateGameSettings()
        {
            TreasureOptions.Instance.User.CurrentLife = TreasureOptions.Instance.Game.Charges;
            TreasureOptions.Instance.User.CurrentGold = 0;
            TreasureOptions.Instance.User.CurrentScore = 0;
            TreasureOptions.Instance.User.Actions = 0;
            TreasureOptions.Instance.Game.Detection = TreasureGame.DetectionMode.Value;

            // Get a random game descriptor
            List<String> setup = TreasureOptions.Instance.Game.GetLevelDescriptors();
            String settings = setup[_rand.Next(0, setup.Count - 1)];
            TreasureOptions.Instance.Game._curSetup = settings;
            Debug.WriteLine("game settings : " + settings);

            depthArray = new int[settings.Length];
            scoreArray = new int[settings.Length];

            int total = 0;
            for (int i = 0; i < TreasureOptions.Instance.Game.Zones; i++)
            {
                Boolean isGold = (TreasureOptions.Instance.Game._curSetup[i] == '1');
                //if (!isGold) continue;

                int loc = this._rand.Next(0, TreasureOptions.Instance.Game.Depth);
                double scoreRatio = (loc + 1.0) / (double)TreasureOptions.Instance.Game.Depth;

                if (isGold)
                {
                    depthArray[i] = loc;
                    scoreArray[i] = (int)(200 * scoreRatio);
                    total += scoreArray[i];
                }
                else
                {
                    depthArray[i] = loc;
                    scoreArray[i] = 0;
 
                }

            }
            //Array.Sort(scoreArray, delegate(int x, int y) { return y.CompareTo(x); });
            int[] TT = Array.CreateInstance(typeof(int), settings.Length) as int[];
            scoreArray.CopyTo(TT, 0);
            Array.Sort(TT, delegate(int x, int y) { return y.CompareTo(x); });

            int median = TreasureOptions.Instance.Game._curGold / TreasureOptions.Instance.Game.Charges;
            int acc = 0;
            for (int i = median; i < median + TreasureOptions.Instance.Game.Charges && i < TT.Length; i++)
                acc += TT[i];
            int max = 0;
            for (int i = 0; i < TreasureOptions.Instance.Game.Charges && i < TT.Length; i++)
                max += TT[i];

                //Array.Sort(TT, (x, y) => y.CompareTo(x));

                // Array.Sort(TT, int.);

            Debug.WriteLine("game scores : " + String.Join(",", TT));
            Debug.WriteLine("median : " + acc);
            //int acc = 0;
            //foreach (int i in scoreArray)
            //    acc += i;
            TreasureOptions.Instance.User.CurrentTarget = acc;
            TreasureOptions.Instance.User.MaxTarget = max;
            Debug.WriteLine("target scores : " + TreasureOptions.Instance.User.CurrentTarget + " / " + TreasureOptions.Instance.User.MaxTarget);


        }

        public void startMainMenu()
        {
            while (GameObject.gameObjects.Count != 0)
                GameObject.gameObjects[0].shutdown();

            GamePage pg = AuditoryGameApp.Current.RootVisual as GamePage;

            removeAllCanvasChildren();

            TreasureOptions.Instance.Game.Detection = TreasureGame.DetectionMode.Value;
            GenerateGameSettings();

            this._scorePanel = new TreasureToolbox()
            {
                FullMode = false
            };
            _scorePanel.SetValue(Canvas.LeftProperty, 0.0);
            _scorePanel.SetValue(Canvas.TopProperty, 0.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children.Add(_scorePanel);

            HighScoreControl ct = new HighScoreControl();
            ct.SetValue(Canvas.LeftProperty, 400.0);
            ct.SetValue(Canvas.TopProperty, 50.0);
            ct.Source.ItemsSource = TreasureOptions.Instance.User.Scores.Data;

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(ct);

            StartLevelPanel pp = new StartLevelPanel()
                {
                    Gold =  TreasureOptions.Instance.Game._curGold,
                    Metal = TreasureOptions.Instance.Game.Zones - TreasureOptions.Instance.Game._curGold,
                    Live = TreasureOptions.Instance.Game.Charges,
                    Target = new Point(TreasureOptions.Instance.User.CurrentTarget, TreasureOptions.Instance.User.MaxTarget)
                };
            pp.CurrentLevel = TreasureOptions.Instance.User.CurrentLevel;
            //pp.CurrentGate = TreasureOptions.Instance.User.CurrentGate;

            pp.SetValue(Canvas.LeftProperty, 10.0);
            pp.SetValue(Canvas.TopProperty, 50.0);
            pp.StartBtn.Click += delegate(object sender, RoutedEventArgs e)
            {
                StateManager.Instance.setState(TreasureStates.LEVEL_STATE);
            };
            pp.RefreshBtn.Click += delegate(object sender, RoutedEventArgs e)
            {
                GenerateGameSettings();
                pp.Gold =  TreasureOptions.Instance.Game._curGold;
                  pp.Metal = TreasureOptions.Instance.Game.Zones - TreasureOptions.Instance.Game._curGold;
                  pp.Live = TreasureOptions.Instance.Game.Charges;
                  pp.Target = new Point(TreasureOptions.Instance.User.CurrentTarget, TreasureOptions.Instance.User.MaxTarget);
            };


            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(pp);


            ButtonIcon btnFull = new ButtonIcon();
            btnFull.TextContent.Text = "Full Screen Mode";
            btnFull.Icon.Source = ResourceHelper.GetBitmap("/GameFramework;component/Media/fullscreen.png");
            btnFull.Icon.Height = 22;
            btnFull.Icon.Width = 31;
            btnFull.Width = 150;
            btnFull.Height = 40;
            btnFull.SetValue(Canvas.LeftProperty, 50.0);
            btnFull.SetValue(Canvas.TopProperty, 450.0);
            btnFull.Click += delegate(object sender, RoutedEventArgs e)
            {
                AuditoryGameApp.Current.Host.Content.IsFullScreen = !AuditoryGameApp.Current.Host.Content.IsFullScreen;
            };
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnFull);


            btnOption = new ButtonIcon();
            btnOption.TextContent.Text = "Options";
            btnOption.Icon.Source = ResourceHelper.GetBitmap("/GameFramework;component/Media/fullscreen.png");
            btnOption.Icon.Height = 22;
            btnOption.Icon.Width = 31;
            btnOption.Width = 150;
            btnOption.Height = 40;
            btnOption.SetValue(Canvas.LeftProperty, 50.0);
            btnOption.SetValue(Canvas.TopProperty, 500.0);
            btnOption.Click += delegate(object sender, RoutedEventArgs e)
            {
                StateManager.Instance.setState(TreasureStates.OPTION_STATE);
            };
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnOption);

            /* GameParameters param = new GameParameters();
             param.NbZone.Value = GameLevelInfo._nbTreasureZones;

             param.SetValue(Canvas.LeftProperty, 490.0);
             param.SetValue(Canvas.TopProperty, 50.0);
             (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(param);*/

            //MapViewer mp = new MapViewer();
            //mp.SetValue(Canvas.LeftProperty, 50.0);
            //mp.SetValue(Canvas.TopProperty, 250.0);
            //(AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(mp);

            /*QuickPlayPanel qp = new QuickPlayPanel();
            qp.LayoutRoot.DataContext = TreasureOptions.Instance;
            qp.SetValue(Canvas.LeftProperty, 50.0);
            qp.SetValue(Canvas.TopProperty, 250.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(qp);*/

            Button pAboutBtn = new Button();
            pAboutBtn.Content = @"About ...";

            pAboutBtn.Width = 75;
            pAboutBtn.Height = 40;
            pAboutBtn.SetValue(Canvas.LeftProperty, 800.0-75.0-25.0);
            pAboutBtn.SetValue(Canvas.TopProperty, 540.0-40.0-25.0);
            pAboutBtn.Click += delegate(object sender, RoutedEventArgs e)
            {
                /*AboutWindow pAbout = new AboutWindow();
                pAbout.SetValue(Canvas.LeftProperty, (pg.LayoutRoot.ActualWidth - pAbout.Width) / 2);
                pAbout.SetValue(Canvas.TopProperty, (pg.LayoutRoot.ActualHeight - pAbout.Height) / 2);
                pAbout.Closed += delegate(object s1, EventArgs e1)
                    {
                    };
                pAbout.Show();*/

                AboutPanel pAbout2 = new AboutPanel();
                Rectangle pMask = new Rectangle();
                pMask.Height = pg.LayoutRoot.ActualHeight;
                pMask.Width = pg.LayoutRoot.ActualWidth;
                pMask.SetValue(Canvas.LeftProperty, 0.0);
                pMask.SetValue(Canvas.TopProperty, 0.0);
                pMask.Fill = new SolidColorBrush(Color.FromArgb((byte)(0.75*255.0),0,0,0));
                pAbout2.SetValue(Canvas.LeftProperty, (pg.LayoutRoot.ActualWidth - pAbout2.Width) / 2);
                pAbout2.SetValue(Canvas.TopProperty, (pg.LayoutRoot.ActualHeight - pAbout2.Height) / 2);
                (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(pMask);
                (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(pAbout2);
                pAbout2.OnCompleteTask += delegate()
                {
                    (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(pMask);
                    (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(pAbout2);
                };
            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(pAboutBtn);
        }

        public void endMainMenu()
        {
            removeAllCanvasChildren();
        }

        #endregion

        #region Application State: Game

        public void startGame()
        {
            TreasureOptions.Instance.AttachDebug(AuditoryGameApp.Current.RootVisual as GamePage);
            TreasureApplicationManager.PREVENT_AUDIO_CHANGES = false;

            // Set the toolbox
            this._scorePanel = new TreasureToolbox()
            {
                FullMode = true,
                Life = TreasureOptions.Instance.User.CurrentLife,
                Gold = TreasureOptions.Instance.Game._curGold,
                Score = TreasureOptions.Instance.User.CurrentScore,
                Level = TreasureOptions.Instance.User.CurrentLevel,
                Target = TreasureOptions.Instance.User.CurrentTarget,
                MaxScore = TreasureOptions.Instance.User.MaxTarget
            };
            _scorePanel.SetValue(Canvas.LeftProperty, 0.0);
            _scorePanel.SetValue(Canvas.TopProperty, 0.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children.Add(_scorePanel);

            // initialise collisions
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMY);
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMYWEAPON);
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYERWEAPON, CollisionIdentifiers.ENEMY);

            // Get layout
            Canvas cv = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot;
            TreasureOptions.Instance.Game._sizeZones = ((int)cv.ActualWidth) / TreasureOptions.Instance.Game.Zones;

            //// initialise game objects
            // background image
            BackgroundTreasureGameObject bgImage = new BackgroundTreasureGameObject()
            {
                ImageStretch = Stretch.Fill
            };
            bgImage.startupBackgroundGameObject(
                new Point(cv.ActualWidth, cv.ActualHeight - GameLayout.MARGIN_SKY),
               "Media/bg.png", -1);
            bgImage.Position = new Point(0, GameLayout.MARGIN_SKY);

            // rail object
            BackgroundTreasureGameObject railImage = new BackgroundTreasureGameObject()
            {
                ImageStretch = Stretch.Fill
            };
            railImage.startupGameObject(
                new Point(cv.ActualWidth, 15),
               "Media/rail.png", -1);
            railImage.Position = new Point(0, GameLayout.MARGIN_SKY - 10);

            // player object
            _player = new HunterPlayer();
            _player.startupPlayer(
                new Point(73, 85),
                new AnimationData(
                    new string[] { "Media/bob.png" },
                    1),
                ZLayers.PLAYER_Z);

            _player.CurrentZone = (TreasureOptions.Instance.Game.Zones / 2);
            int currLoc = TreasureOptions.Instance.Game._sizeZones * _player.CurrentZone + (TreasureOptions.Instance.Game._sizeZones - (int)_player.Dimensions.X) / 2;
            _player.Position = new Point(currLoc, GameLayout.MARGIN_SKY - _player.Dimensions.Y - 10);


            // treasure nuggets
            double dd = bgImage.Dimensions.Y - GameLayout.MARGIN_NUGGETS * 2;
            double depthsize = dd / TreasureOptions.Instance.Game.Depth;

            _nuggets.Clear();

            for (int i = 0; i < TreasureOptions.Instance.Game.Zones; i++)
            {
                Boolean isGold = (TreasureOptions.Instance.Game._curSetup[i] == '1');

                TreasureNugget ng = TreasureNugget.UnusedNuggets.startupBasicNugget(
                    new Point(56, 40),
                    i,
                    isGold ? TreasureNugget.TreasureType.TREASURE_GOLD : TreasureNugget.TreasureType.TREASURE_METAL,
                    ZLayers.PLAYER_Z);

                Point pt = new Point(TreasureOptions.Instance.Game._sizeZones * i, dd);
                pt.X = pt.X + (TreasureOptions.Instance.Game._sizeZones - ng.Dimensions.X) / 2;
                //pt.Y = GameLayout.MARGIN_SKY + 25 + this._rand.Next(0, (int)depthsize) * depthsize;
                //int loc = this._rand.Next(0, TreasureOptions.Instance.Game.Depth);

                //double scoreRatio = (loc + 1.0) / (double)TreasureOptions.Instance.Game.Depth;
                int loc = depthArray[i];
                //loc = (i % TreasureOptions.Instance.Game.Depth);
                pt.Y = GameLayout.MARGIN_SKY + GameLayout.MARGIN_NUGGETS + loc * depthsize;
                pt.Y += (depthsize - ng.Dimensions.Y) / 2;
                ng.Depth = loc;
                ng.Position = pt;
                //ng.Score = (isGold) ? (int)(200 * scoreRatio) : 0;
                ng.Score = (isGold) ? (int)(scoreArray[i]) : 0;
               /* if (isGold)
                {
                    depthArray[i] = loc;
                    scoreArray[i] = ng.Score;
                    j++;
                }
                */
                _nuggets.Add(ng);
            }
            //Array.Sort(depthArray, delegate(int x, int y) { return y.CompareTo(x); });
            //Array.Sort(scoreArray, delegate(int x, int y) { return y.CompareTo(x); });


            changeExposure();

            /*// score window (TO DO AGAIN)
            txtbScore = new TextBlock();
            txtbScore.Text = TreasureApplicationManager.Instance.Score.ToString();
            txtbScore.Width = 100;
            txtbScore.Height = 35;
            txtbScore.FontSize = 20;
            txtbScore.Foreground = new SolidColorBrush(Colors.White);
            txtbScore.SetValue(Canvas.LeftProperty, 10.0);
            txtbScore.SetValue(Canvas.TopProperty, 10.0);

            // we have to insert any non GameObjects at the end of the children collection
           // (LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Insert(
            //    (LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Count, txtbScore);*/

            /* ScoreControl ff = new ScoreControl();
             ff.SetValue(Canvas.LeftProperty, 10.0);
             ff.SetValue(Canvas.TopProperty, 10.0);
             (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Insert(
                 (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Count, ff);
             */

            // UpdateSound();
            /*
            float freqL = 0;
            float freqM = 0;
            float freqR = 0;
            if (TreasureOptions.Instance.Game.Detection == TreasureGame.DetectionMode.Proximity)
            {
                freqL = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone - 1] == '1') ? 5000 : 3000;
                freqM = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone + 1] == '1') ? 5000 : 3000;
            }
            else if (TreasureOptions.Instance.Game.Detection == TreasureGame.DetectionMode.Value)
            {
                String left = TreasureOptions.Instance.Game._curSetup.Substring(0, _player.CurrentZone);
                String right = TreasureOptions.Instance.Game._curSetup.Substring(_player.CurrentZone + 1);
                int nbL = left.LastIndexOf('1');
                int nbR = right.IndexOf('1');
                if (nbL != -1) nbL = left.Length - nbL - 1;
                else nbL += 100001;
                if (nbR == -1) nbR += 100001;


                freqL = (nbL < nbR) ? 5000 : 3000;
                freqM = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (nbL >= nbR ) ? 5000 : 3000;
            }
            else if (TreasureOptions.Instance.Game.Detection == TreasureGame.DetectionMode.Distance)
            {
                String left = TreasureOptions.Instance.Game._curSetup.Substring(0, _player.CurrentZone);
                String right = TreasureOptions.Instance.Game._curSetup.Substring(_player.CurrentZone + 1);
                int nbL = 0;
                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i] == '1') nbL++;
                }
                int nbR = 0;
                for (int i = 0; i < right.Length; i++)
                {
                    if (right[i] == '1') nbR++;
                }
                freqL = (nbL >= nbR) ? 5000 : 3000;
                freqM = (TreasureOptions.Instance.Game._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (nbL < nbR) ? 5000 : 3000;
                

            }*/
            /* _synthEx.Arpeggiator.Notes[0].Frequency = freqL;
             _synthEx.Arpeggiator.Notes[2].Frequency = freqM;
             _synthEx.Arpeggiator.Notes[4].Frequency = freqR;
             _synthEx.Arpeggiator.Start();*/
            //MediaElement children = (LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).AudioPlayer;
            //this._synthEx.sequencer.StepIndex = this._synthEx.sequencer.StepCount - 1;
            //this._synthEx.ResetSequencer();
            // children.Play();
            //this._synthEx.Start();
            UpdateSound(-1);
            TreasureOptions.Instance.UpdateDebug();
        }

        public void exitGame()
        {
            this._synthEx.Stop();
            //MediaElement children = (LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).AudioPlayer;
            //children.Stop();
            //this._synthEx.sequencer.Reset();
           // while (GameObject.gameObjects.Count != 0)
           //     GameObject.gameObjects[0].shutdown();

            //removeAllCanvasChildren();
            // _synthEx.Arpeggiator.Stop();

            //txtbScore = null;
        }

        #endregion

        #region Application State: Options

        private void startOptions()
        {
            GameParameters panel = new GameParameters(
                TreasureOptions.Instance.User,
                TreasureOptions.Instance.Auditory,
                TreasureOptions.Instance.Game);
            panel.OnCompleteTask += delegate()
            {
                StateManager.Instance.setState(TreasureStates.START_STATE);
            };
            panel.SetValue(Canvas.LeftProperty, 10.0);
            panel.SetValue(Canvas.TopProperty, 10.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(panel);

        }

        private void exitOptions()
        {
            removeAllCanvasChildren();
        }

        #endregion

        #region Application State: Scores

        private void startScore()
        {
            bool success = TreasureOptions.Instance.User.CurrentScore >= TreasureOptions.Instance.User.CurrentTarget;

            GamePage pg = AuditoryGameApp.Current.RootVisual as GamePage;
            ScorePanel pn = new ScorePanel();
            pn.SetValue(Canvas.LeftProperty, (pg.LayoutRoot.ActualWidth - pn.Width) / 2);
            pn.SetValue(Canvas.TopProperty, (pg.LayoutRoot.ActualHeight - pn.Height) / 2);
            pn.OnCompleteTask += delegate()
            {
                // adapt the auditory staircase based on the result
                if (success)
                {
                    // Add the score to the HighScore list
                    TreasureOptions.Instance.User.Scores.Data.Add(new HighScore()
                    {
                        Delta = (int)TreasureOptions.Instance.User.FrequencyDelta,
                        Level = TreasureOptions.Instance.User.CurrentLevel,
                        Score = pn.FinalScore
                    });

                    // change Frequency delta
                    TreasureOptions.Instance.User.FrequencyDelta *= (1 - TreasureOptions.Instance.Auditory.Step);
                    if (TreasureOptions.Instance.User.FrequencyDelta > (TreasureOptions.Instance.User.FrequencyTraining * TreasureOptions.Instance.Auditory.Base))
                    {
                        // Delta capped at base
                        TreasureOptions.Instance.User.FrequencyDelta = TreasureOptions.Instance.User.FrequencyTraining * TreasureOptions.Instance.Auditory.Base;
                    }

                    // go to next level
                    TreasureOptions.Instance.User.CurrentLevel++;
                }
                else
                {
                    if (TreasureOptions.Instance.User.CurrentLevel == 1)
                    {
                        // FIRST LEVEL : TUTORIAL - NO CHANGE IN AUDITORY MODEL
                    }
                    else if (TreasureOptions.Instance.User.CurrentLevel <= 8)
                    {
                        // FIRST 8 LEVELS : ACCOMMODATION - NO INCREASE OF DELTA
                    }
                    else
                    {
                        TreasureOptions.Instance.User.FrequencyDelta *= (1 + TreasureOptions.Instance.Auditory.Step);
                        if (TreasureOptions.Instance.User.FrequencyDelta > (TreasureOptions.Instance.User.FrequencyTraining * TreasureOptions.Instance.Auditory.Base))
                        {
                            // Delta capped at base
                            TreasureOptions.Instance.User.FrequencyDelta = TreasureOptions.Instance.User.FrequencyTraining * TreasureOptions.Instance.Auditory.Base;
                        }

                    }
                }

                StateManager.Instance.setState(TreasureStates.START_STATE);
                //StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
            };
            pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, pn);
        }

        private void exitScore()
        {
        }

        #endregion

    }
}
