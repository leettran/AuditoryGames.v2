﻿/*  Auditory Training Games in Silverlight
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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using LSRI.AuditoryGames.GameFramework.UI;
using System.ComponentModel;
using System.Reflection;



namespace LSRI.Submarine
{

    /// <summary>
    /// 
    /// </summary>
    /// @see States
    public class SubmarineStates : States
    {
        public const string MAINMENU_STATE = "start_mainmenu";  ///< starting the main menu
        public const string LEVEL_STATE = "start_Level";        ///< Starting a new level 
        public const string OPTION_STATE = "start_option";      ///< Starting the option page
        public const string LOG_STATE = "start_log";            ///< Starting the log page
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubOptionsWrapper : IConfigurationManager
    {
        private static readonly string STORAGE_FILENAME = @"SubmarineSettings.xml";

        /// <summary>
        /// 
        /// </summary>
        protected AuditoryModel _auditory = null;

        /// <summary>
        /// 
        /// </summary>
        protected GameOptions _gOption = null;

        /// <summary>
        /// 
        /// </summary>
        protected UserModel _user = null;

        /// <summary>
        /// 
        /// </summary>
        public SubOptionsWrapper()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A deep copy of the Submarine Game options</returns>
        public SubOptionsWrapper Clone()
        {
            SubOptionsWrapper tt = new SubOptionsWrapper();
            tt._auditory = this._auditory.Clone();
            tt._gOption = this._gOption.Clone();
            tt._user = this._user.Clone();
            return tt;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temp"></param>
        public void Copy(SubOptionsWrapper temp)
        {
            if (temp == null) return;
            this._auditory = temp._auditory.Clone();
            this._gOption = temp._gOption.Clone();
            this._user = temp._user.Clone();
        }

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
        /// Access to the current user configuration
        /// </summary>
        public UserModel User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        /// <summary>
        /// Save the configuration into isolated storage
        /// </summary>
        public void SaveConfiguration()
        {
            try
            {
                //var settings = IsolatedStorageSettings.ApplicationSettings;
                //IsolatedStorageSettings.ApplicationSettings["configuration"] = this.Clone();
                //IsolatedStorageSettings.ApplicationSettings["username"] = User.Name;
                //settings.Save();

                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    using (IsolatedStorageFileStream isoStream = store.OpenFile(SubOptionsWrapper.STORAGE_FILENAME, FileMode.Create))
                    {
                        XmlSerializer s = new XmlSerializer(typeof(SubOptionsWrapper));
                        TextWriter writer = new StreamWriter(isoStream);
                        s.Serialize(writer, this.Clone());
                        writer.Close();
                    }
                }

            }
            catch (Exception e)
            {
                //Debug.WriteLine("SERIALIZATION ERROR : " + e.Message);
            }
        }


        /// <summary>
        /// Load configuration from the isolated storage
        /// </summary>
        public void RetrieveConfiguration()
        {
            try
            {
                //var settings = IsolatedStorageSettings.ApplicationSettings;
                //SubOptionsWrapper um2 = null;
                //String name;
                //IsolatedStorageSettings.ApplicationSettings.TryGetValue("configuration", out um2);
                //IsolatedStorageSettings.ApplicationSettings.TryGetValue("username", out name);

                SubOptionsWrapper umXML = null;
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists(SubOptionsWrapper.STORAGE_FILENAME))
                    {
                        using (IsolatedStorageFileStream isoStream = store.OpenFile(SubOptionsWrapper.STORAGE_FILENAME, FileMode.Open))
                        {
                            XmlSerializer s = new XmlSerializer(typeof(SubOptionsWrapper));
                            TextReader writer = new StreamReader(isoStream);
                            umXML = s.Deserialize(writer) as SubOptionsWrapper;
                            writer.Close();
                        }
                    }
                }
                if (umXML != null)
                {
                    this.Copy(umXML);
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine("DE-SERIALIZATION ERROR : " + e.Message);
            }
        }
    }

    /// <summary>
    /// General wrapper for the submarine game configuration
    /// </summary>
    public sealed class SubOptions : SubOptionsWrapper
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
            internal static readonly SubOptions instance = new SubOptions()
                {
                    //_container = UserModelContainer.Default(),
                    _auditory = new AuditoryModel(),
                    _gOption = new GameOptions(),
                    _user = new UserModel
                    {
                        Type = UserModel.UserType.User,
                        Name = "Current User",
                        Scores = HighScoreContainer.Default()
                    }
                };
        }

        public static SubOptions Instance
        {
            get
            {
                return Nested.instance;
            }
        }


 
        #region Debug Display Mode

        public class ScorePattern
        {
            /// <summary>
            /// The score associated with the accuracy of targetting
            /// </summary>
            public double GateAccuracy { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public double GatePosition { get; set; }

            /// <summary>
            /// Percentage of time left in getting through the gate
            /// </summary>
            public double TimeLeft { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public double LifeLost { get; set; }

            public Boolean GateCrossed { get; set; }
        }

        /// <summary>
        /// The GUI panel to display the debugging information
        /// </summary>
        private static TextBlock _debugUI = null;
        public Collection<ScorePattern> _scoreBuffer = new Collection<ScorePattern>();
                      
        /// <summary>
        /// 
        /// </summary>
        public string Beat { set; get; }    

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

            if (SubOptions.Instance.User.ShowDebug)
                _debugUI.Visibility = Visibility.Visible;
            else
                _debugUI.Visibility = Visibility.Collapsed;
            // we have to insert any non GameObjects at the end of the children collection
            pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, _debugUI);
        }

        /// <summary>
        /// Update the content of the debug panel
        /// </summary>
        public void UpdateDebug()
        {
            if (_debugUI == null) return;
            if (!SubOptions.Instance.User.ShowDebug) return;
            _debugUI.Text = String.Format(
                "Training Fq : {0} Hz\n" +
                "Delta       : {1} Hz\n" + 
                "-----\n" +
                "Comparison  : {2} Hz\n" +
                "-----\n" +
                "Level       : {3}\n" +
                "Gate        : {4}\n\n" +
                "-----\n" +
                "Gates       : {5}\n" +
                 "-----\n" + 
                 "{6}",
                this.User.FrequencyTraining,
                 this.User.FrequencyDelta,
                this.User.FrequencyComparison,
                this.User.CurrentLevel,
                this.User.CurrentGate,
                String.Join(",",this.User.Gates.Data),
                this.Beat
                );

        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public class SubmarineLogger : GameLogger
    {

        /// <summary>
        /// Event triggered when the user starts a scene (i.e. a run to the next gate) in the game
        /// </summary>
        protected static readonly string LOG_STARTSCENE = "START_SCENE";
        /// <summary>
        /// Event triggered when the Submarine hits the end of the game scene (whether the gate or the wall)
        /// </summary>
        protected static readonly string LOG_HITWALLORGATE = "HIT_WALLGATE";
        /// <summary>
        /// Event triggered when the user uses the acceleration key
        /// </summary>
        protected static readonly string LOG_USERACTION_ACCEL = "USER_ACCELERATION";
        /// <summary>
        /// Event triggered when the user uses the booster key
        /// </summary>
        protected static readonly string LOG_USERACTION_BOOSTER = "USER_BOOSTER";

        private static readonly string SUB_STORAGE_CSVFILENAME = @"logger_sub.csv";


        private DateTime _startGame = DateTime.Now;
        private DateTime _startLevel = DateTime.Now;
        private DateTime _startScene = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The filename of the CSV file where the log will be saved</returns>
        public override string getStorageName()
        {
            return SubmarineLogger.SUB_STORAGE_CSVFILENAME;
        }

        public override string getJSONFilename()
        {
            return @"logger_sub.json"; ;
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
                    SubOptions.Instance.User.Name.Replace(",","_"),
                    SubOptions.Instance.User.FrequencyTraining.ToString(),
                    SubOptions.Instance.User.FrequencyDelta.ToString()
                           };

            string strParam = String.Join(",", par);
            WriteLogFile(_startGame,GameLogger.LOG_GAMESTARTED, strParam);
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
                    SubOptions.Instance.User.CurrentLevel.ToString(),
                    SubOptions.Instance.User.FrequencyTraining.ToString(),
                    SubOptions.Instance.User.FrequencyDelta.ToString()
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
        /// - <b>$level$</b>: the current level of the game
        /// - <b>$outcome$</b>: SUCCESS if level completed, FAIL if not, CANCEL if game prematurely stopped
        /// - <b>$gate$</b>: index (0-based) of the gate reached (4 if SUCCESS)
        /// - <b>$score$</b>: the current score of the user (might have a positive value even if level has failed)
        /// - <b>$train$</b>: the number of training frequencies presented
        /// - <b>$comp$</b>: the number of comparison frequencies presented
        /// - <b>$acc$</b>: the average accuracy (percentage)
        /// - <b>$time$</b>: the average time left (percentage)
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

            int train = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.CountTraining;
            int comp = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.CountComparison;

            double avAcc = 0;
            double avTime = 0;
            if (win == 1 || win == 0)
            {
                for (int i = 0; i < SubOptions.Instance._scoreBuffer.Count; i++)
                {
                    avAcc += SubOptions.Instance._scoreBuffer[i].GateAccuracy;
                    avTime += SubOptions.Instance._scoreBuffer[i].TimeLeft;
                }
                avAcc = avAcc / SubOptions.Instance._scoreBuffer.Count;
                avTime = avTime / SubOptions.Instance._scoreBuffer.Count;
            }

            String[] par = {
                    elapsed.ToString(),
                    SubOptions.Instance.User.CurrentLevel.ToString(),
                    strWin,
                    SubOptions.Instance.User.CurrentGate.ToString(),
                    SubOptions.Instance.User.CurrentScore.ToString(),
                    train.ToString(),
                    comp.ToString(),
                    ((int)avAcc).ToString(),
                    ((int)avTime).ToString(),
                          };
            WriteLogFile(_now, GameLogger.LOG_LEVELENDED, String.Join(",", par));
        }

        /// <summary>
        /// output 
        /// - $date$,$time$,<b>LEVEL_STARTED</b>,0,$level$,$training$,$delta$
        /// 
        /// where.
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: always 0
        /// - <b>$level$</b>: the current level of the game
        /// - <b>$training$</b>: the training frequency (Hz) of the user
        /// - <b>$delta$</b>: the current frequency delta (Hz) of the user
        /// </summary>
        public virtual void logGateStart()
        {
            _startScene = DateTime.Now;
            DateTime _now = DateTime.Now;
            TimeSpan elapsed = _now - _now;
            String[] par = {
                    elapsed.ToString(),
                    SubOptions.Instance.User.CurrentLevel.ToString(),
                    SubOptions.Instance.User.CurrentGate.ToString()
                          };
            WriteLogFile(_now, SubmarineLogger.LOG_STARTSCENE, String.Join(",", par));
        }


        /// <summary>
        /// output 
        /// - $date$,$time$,<b>HIT_WALLGATE</b>,$duration$,$outcome$,$gate$,$diff$,$comparison$
        /// 
        /// where
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: time elapsed (HH:MM:SS.0000) since the previous LEVEL_STARTED event
        /// - <b>$outcome$</b>: GATE if hit the gate, WALL if hit the wall, CANCEL if game prematurely stopped
        /// - <b>$gate$</b>: index (0-based) of the current gate
        /// - <b>$diff$</b>: position difference (in game steps) between submarine and gate (negative means submarine below, positive means above)
        /// - <b>$comparison$</b>: the current comparison frequency (Hz) of the user
        /// - <b>$acc$</b>: the accuracy (percentage)
        /// - <b>$time$</b>: the time left (percentage)
        /// </summary>
        public virtual void logGateReach(bool win, double Fq,double acc,double eff)
        {
            DateTime _now = DateTime.Now;
            TimeSpan elapsed = _now - _startLevel;
            String strWin = "";
            switch (win)
            {
                case false: strWin = "WALL";
                    break;
                case true: strWin = "GATE";
                    break;
                default:
                    strWin = "CANCEL";
                    break;
            }
            int nb = (IAppManager.Instance as SubmarineApplicationManager)._gate.CanvasIndex - (IAppManager.Instance as SubmarineApplicationManager)._submarine.CanvasIndex;
            String[] par = {
                    elapsed.ToString(),
                    strWin,
                    SubOptions.Instance.User.CurrentGate.ToString(),
                    nb.ToString(),
                    Fq.ToString("F2"),
                    acc.ToString("F2"),
                    eff.ToString("F2")
                          };
            WriteLogFile(_now, SubmarineLogger.LOG_HITWALLORGATE, String.Join(",", par));
        }

        /// <summary>
        /// output 
        /// - $date$,$time$,<b>USER_ACCELERATION</b>,$duration$
        /// - $date$,$time$,<b>USER_BOOSTER</b>,$duration$
        /// 
        /// where
        /// - <b>$date$</b>: the date (DD/MM/YYYY) of the event
        /// - <b>$time$</b>: the time (HH:MM:SS.0000) of the event
        /// - <b>$duration$</b>: time elapsed (HH:MM:SS.0000) since the previous LEVEL_STARTED event
        /// </summary>
        public virtual void logAcceleration(Key key)
        {
            DateTime _now = DateTime.Now;
            TimeSpan elapsed = _now - _startLevel;
            String strWin = "";
            switch (key)
            {
                case Key.Space:
                    strWin = SubmarineLogger.LOG_USERACTION_BOOSTER;
                    break;
                default:
                    strWin = SubmarineLogger.LOG_USERACTION_ACCEL;
                    break;
            }
            String[] par = {
                    elapsed.ToString()
            };
            WriteLogFile(_now, strWin, String.Join(",", par));
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class SubmarineApplicationManager : IAppManager
    {
        public SubmarinePlayer _submarine = null;   ///< Reference to the submarine object
        public GateAnimatedObject _gate = null;     ///< Reference to the gate object
        public WallObject _wall = null;             ///< Reference to the wall object

        public SubmarineToolbox _scorePanel = null;

        public Frequency2IGenerator _synthEx = null;    ///< Reference to the the auditory stimuli generator

        private Random _random; ///< Random number generation
        private StopwatchPlus _watch;
        private Brush bg = null;

        private ButtonIcon btnOption = null;
        public SubmarineLogger myLogger = new SubmarineLogger();

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
            this.Pause = false;
            KeyHandler.Instance.IskeyUpOnly = false;
            //_synthEx = new Frequency2IGenerator();
            _random = new Random();


            SubOptions.Instance.RetrieveConfiguration();
            myLogger.logGameStarted();
        }

        private void GamePage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           e.Handled = (allowConfiguration == false);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void startupApplicationManager()
        {
            // Prevent right-click by user
            (AuditoryGameApp.Current.RootVisual as GamePage).MouseRightButtonDown += new MouseButtonEventHandler(GamePage_MouseRightButtonDown);
            (AuditoryGameApp.Current.RootVisual as GamePage).MouseRightButtonUp += new MouseButtonEventHandler(GamePage_MouseRightButtonDown);

            // Register the different states of the game
            StateManager.Instance.registerStateChange(
                States.START_STATE,
                new StateChangeInfo.StateFunction(startGame),
                new StateChangeInfo.StateFunction(endGame));

            StateManager.Instance.registerStateChange(
                SubmarineStates.MAINMENU_STATE,
                new StateChangeInfo.StateFunction(startMainMenu),
                new StateChangeInfo.StateFunction(endMainMenu));

            StateManager.Instance.registerStateChange(
                SubmarineStates.LEVEL_STATE,
                new StateChangeInfo.StateFunction(startLevel),
                new StateChangeInfo.StateFunction(exitLevel));


            StateManager.Instance.registerStateChange(
                SubmarineStates.OPTION_STATE,
                new StateChangeInfo.StateFunction(startOptions),
                new StateChangeInfo.StateFunction(exitOptions));


            // Associate the stimuli generator with the media element of the game page
            MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
            _synthEx = new Frequency2IGenerator(children,SubOptions.Instance.Auditory.BufferLength)
            {
                AttenuationSequencer = SubOptions.Instance.Auditory.Attenuation + SubOptions.Instance.Auditory.AttenuationRandom,
                AttenuationRandom = SubOptions.Instance.Auditory.AttenuationRandom
            };

            // Add the listeners to the stimuli generator
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
            if (StateManager.Instance.CurrentState.Equals(SubmarineStates.LEVEL_STATE))
            {
                if (KeyHandler.Instance.isKeyPressed(Key.P))
                {
                    this.Pause = !this.Pause;
                }
            }

            if (StateManager.Instance.CurrentState.Equals(SubmarineStates.MAINMENU_STATE))
            {
                if (KeyHandler.Instance.isKeyPressed(Key.Enter))
                {
                    StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
                }
            }

            if (StateManager.Instance.CurrentState.Equals(SubmarineStates.LEVEL_STATE))
            {
                if (KeyHandler.Instance.isKeyPressed(Key.Q))
                {
                    (IAppManager.Instance as SubmarineApplicationManager).myLogger.logLevelEnded(-1);
                    // Quit and restart level
                    SubOptions.Instance.User.CurrentLife = SubOptions.Instance.Game.MaxLives;
                    SubOptions.Instance.User.CurrentGate = 0;
                    SubOptions.Instance.User.CurrentScore = 0;
                    SubOptions.Instance._scoreBuffer.Clear();
                    StateManager.Instance.setState(SubmarineStates.MAINMENU_STATE);
                    return;
                }

                // cheat code for showing the gate
                if (KeyHandler.Instance.isKeyPressed(Key.G))
                {
                    if (_gate == null) return;
                    Visibility isVis = _gate.Visibility;
                    if (isVis == Visibility.Visible)
                        _gate.Visibility = Visibility.Collapsed;
                    else
                        _gate.Visibility = Visibility.Visible;
                    KeyHandler.Instance.clearKeyPresses();
                }


                /// Check for visibility of gate
                double r = (_gate.Position.X - _submarine.Position.X) / _gate.Position.X;
                if (SubOptions.Instance.User.IsGateVisible(r))
                {
                    if (_gate.Visibility != Visibility.Visible)
                        _gate.Visibility = Visibility.Visible;
                }

                SubOptions.Instance.UpdateDebug();
            }
            else if (StateManager.Instance.CurrentState.Equals(SubmarineStates.MAINMENU_STATE))
            {
                ModifierKeys keys = Keyboard.Modifiers;
                bool controlKey = (keys & ModifierKeys.Control) != 0;
                bool altKey = (keys & ModifierKeys.Alt) != 0;
                bool shiftkey = (keys & ModifierKeys.Shift) != 0;
                if (controlKey && altKey && shiftkey && KeyHandler.Instance.isKeyPressed(Key.Z))
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

        public override void shutdown()
        {
            myLogger.logGameEnded();
            SubOptions.Instance.SaveConfiguration();
        }

        #region Application State: Init game

        private void startGame()
        {
            // initialise toolbox
            this._scorePanel = new SubmarineToolbox()
            {
                FullMode = false
            };
            _scorePanel.SetValue(Canvas.LeftProperty, 0.0);
            _scorePanel.SetValue(Canvas.TopProperty, 0.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children.Add(_scorePanel); 
            
            Button btnFull = new Button();
            btnFull.Content = "Click here to start";
            btnFull.FontSize = 36;
                btnFull.Width = 350;
            btnFull.Height = 150;

            GamePage pg = AuditoryGameApp.Current.RootVisual as GamePage;
            btnFull.SetValue(Canvas.LeftProperty, (800 - btnFull.Width) / 2);
            btnFull.SetValue(Canvas.TopProperty, (600 - btnFull.Height) / 2);

            btnFull.Click += delegate(object sender, RoutedEventArgs e)
            {
                AuditoryGameApp.Current.Host.Content.IsFullScreen = !AuditoryGameApp.Current.Host.Content.IsFullScreen;
                StateManager.Instance.setState(SubmarineStates.MAINMENU_STATE);

            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnFull);
        }

        private void endGame()
        {
            base.removeAllCanvasChildren();
        }
        #endregion

        #region Application State: Main Menu

        private void startMainMenu()
        {
            // initialise toolbox
            this._scorePanel = new SubmarineToolbox()
            {
                FullMode = false
            };
            _scorePanel.SetValue(Canvas.LeftProperty, 0.0);
            _scorePanel.SetValue(Canvas.TopProperty, 0.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children.Add(_scorePanel);

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


            /*
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
             */

            // Add OPTION button
            btnOption = new ButtonIcon();
            btnOption.TextContent.Text = "Options";
            btnOption.Icon.Source = ResourceHelper.GetBitmap("/GameFramework;component/Media/btn_options.png");
            btnOption.Icon.Height = 32;
            btnOption.Icon.Width = 32;
            btnOption.Width = 120;
            btnOption.Height = 40;
            btnOption.Visibility = Visibility.Collapsed;
            btnOption.SetValue(Canvas.LeftProperty, 40.0);
            btnOption.SetValue(Canvas.TopProperty, 400.0);
            btnOption.Click += delegate(object sender, RoutedEventArgs e)
            {
                StateManager.Instance.setState(SubmarineStates.OPTION_STATE);
            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnOption);
            
            ButtonIcon btnExit = new ButtonIcon();
            btnExit.TextContent.Text = "Exit";
            btnExit.Icon.Source = ResourceHelper.GetBitmap("/GameFramework;component/Media/btn_exit.png");
            btnExit.Icon.Height = 32;
            btnExit.Icon.Width = 32;
            btnExit.Width = 120;
            btnExit.Height = 40;
            btnExit.SetValue(Canvas.LeftProperty, 40.0);
            btnExit.SetValue(Canvas.TopProperty, 540.0 - 40.0 - 25.0);
            btnExit.Click += delegate(object sender, RoutedEventArgs e)
            {
                AssemblyName assemblyName = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                MessageBoxResult res = MessageBox.Show("Do you really want to quit?", assemblyName.Name, MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                    Application.Current.MainWindow.Close();
            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnExit);

           /* ButtonIcon pAboutBtn = new ButtonIcon();
            pAboutBtn.TextContent.Text = @"";
            pAboutBtn.TextContent.Margin = new Thickness() {Left=0};
            pAboutBtn.Icon.Source = ResourceHelper.GetBitmap("/GameFramework;component/Media/btn_information.png");
            pAboutBtn.Icon.Height = 32;
            pAboutBtn.Icon.Width = 32; 
            pAboutBtn.Width = 36;
            pAboutBtn.Height = 36;
            pAboutBtn.SetValue(Canvas.LeftProperty, 800.0-36.0-25.0);
            pAboutBtn.SetValue(Canvas.TopProperty, 540.0-36.0-25.0);
            pAboutBtn.Click += delegate(object sender, RoutedEventArgs e)
            {
                
            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(pAboutBtn);
            */
            bg = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background;

            //StatusPanelControl ctr = new StatusPanelControl();
            //(GameApplication.Current.RootVisual as GamePage).GetLayoutElt().Children.Add(ctr);

          /*  MediaElement media = new MediaElement();
            media.Name = "AudioPlayer";
            media.Loaded += delegate(object sender, RoutedEventArgs e) { //Debug.WriteLine("SOUND LOADED"); };
            media.CurrentStateChanged += delegate(object sender, RoutedEventArgs e) { //Debug.WriteLine("CurrentStateChanged"); };
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
            //StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);

            // Force configuration saving
            SubOptions.Instance.SaveConfiguration();
        }

        private void endMainMenu()
        {
            base.removeAllCanvasChildren();
            _synthEx.Stop();
        }

        #endregion

        #region Application State: Level

        private void startLevel()
        {
            SubOptions.Instance.AttachDebug(AuditoryGameApp.Current.RootVisual as GamePage);

            /*sp1 = new StopwatchPlus(
                    sw => //Debug.WriteLine("Game Started"),
                    sw => //Debug.WriteLine("TimeLeft! {0}", sw.EllapsedMilliseconds),
                    sw => //Debug.WriteLine("totot {0}", sw.EllapsedMilliseconds)

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
            this._scorePanel = new SubmarineToolbox()
            {
                FullMode = true,
                Life = SubOptions.Instance.User.CurrentLife,
                Gate = SubOptions.Instance.User.CurrentGate,
                Gates = SubOptions.Instance.Game.MaxGates,
                Score = SubOptions.Instance.User.CurrentScore,
                Level = SubOptions.Instance.User.CurrentLevel
            };
            _scorePanel.SetValue(Canvas.LeftProperty, 0.0);
            _scorePanel.SetValue(Canvas.TopProperty, 0.0);

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutTitle.Children.Add(_scorePanel);

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

            //Gatepos = nbUnitsInScreen - MOVE_RESTRICTION - gateExtent;
            //Subpos = nbUnitsInScreen - MOVE_RESTRICTION - gateExtent;

            double GateLoc = screenMargin + (Gatepos - gateExtent) * stepSize;
            double SubLoc = screenMargin + Subpos * stepSize;

            // create submarine object
            _submarine = new SubmarinePlayer() { CanvasIndex = Subpos };
            _submarine.startupSubmarine(new Point(90, 61), ZLayers.PLAYER_Z);

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
            ////Debug.WriteLine("Theoretical timing : {0}", theodur*1000);

            // initialise auditory stimuli
            double fqTraining = SubOptions.Instance.User.FrequencyTraining;
            if (SubOptions.Instance.User.FrequencyDelta <= 0)
                SubOptions.Instance.User.FrequencyDelta = SubOptions.Instance.User.FrequencyTraining * SubOptions.Instance.Auditory.Base;
            double deltaf = SubOptions.Instance.User.FrequencyDelta;

            double deltapos = Gatepos - Subpos;
            //double deltaf = fqDiff;//  fqTraining * .2;
            double dfpix = deltaf / SubOptions.Instance.Game.GateSize;
            //Debug.WriteLine("*********** Gatepos = {0}", Gatepos);

            double fqComp = fqTraining - dfpix * deltapos;
            if (fqComp >= SubOptions.Instance.Auditory.MaxFrequency) fqComp = SubOptions.Instance.Auditory.MaxFrequency;
            if (fqComp <= SubOptions.Instance.Auditory.MinFrequency) fqComp = SubOptions.Instance.Auditory.MinFrequency;
            SubOptions.Instance.User.FrequencyComparison = fqComp;

            this._synthEx.ResetSequencer();
            this._synthEx.SetTrainingFrequency(fqTraining);
            this._synthEx.SetTargetFrequency(SubOptions.Instance.User.FrequencyComparison, true);

           
            SubOptions.Instance.UpdateDebug();

            if ((SubOptions.Instance.User.CurrentGate == 0) &&
                (SubOptions.Instance.User.CurrentScore == 0) &&
                (SubOptions.Instance.User.CurrentLife == SubOptions.Instance.Game.MaxLives))
            {
                this._synthEx.CountComparison = 0;
                this._synthEx.CountTraining = 0;
                (IAppManager.Instance as SubmarineApplicationManager).myLogger.logLevelStarted();
            }
            (IAppManager.Instance as SubmarineApplicationManager).myLogger.logGateStart();

            this._synthEx.Start();
        }
        private void exitLevel()
        {
            this._synthEx.Stop();
            //Debug.WriteLine("COUNT SEQUENCER : TRAINING (" + this._synthEx.CountTraining + ") COMPARISON (" + this._synthEx.CountComparison + ")");
            while (GameObject.gameObjects.Count != 0)
                GameObject.gameObjects[0].shutdown();

            removeAllCanvasChildren();

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Background = bg;

        }

        #endregion

        #region Application State: Options

        private void startOptions()
        {
            this.allowConfiguration = true;

            GameParameters panel = new GameParameters(
               SubOptions.Instance.User,
               SubOptions.Instance.Auditory,
               SubOptions.Instance.Game);
            panel.SetValue(Canvas.LeftProperty, 10.0);
            panel.SetValue(Canvas.TopProperty, 10.0);

            /*this._synthEx.Sequencer._freqChangedHook -= new SequencerExt.FrequencyChanged(Sequencer__freqChangedHook);
            this._synthEx.Sequencer._freqPlayedHook -= new SequencerExt.FrequencyPlayed(Sequencer__freqStartHook);
            this._synthEx.Sequencer._freqStoppedHook -= new SequencerExt.FrequencyStopped(Sequencer__freqStopHook);
            this._synthEx.Sequencer._stepEndedHook -= new SequencerExt.StepEnded(Sequencer__stepEndedHook);*/

            // Start the calibration task
            panel.OnCalibrateTask += delegate()
            {
                /*MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
                children = new MediaElement();
                int bug = SubOptions.Instance.Auditory.BufferLength;
                _synthEx = new Frequency2IGenerator(children, bug)
                {
                    AttenuationSequencer = SubOptions.Instance.Auditory.Attenuation + SubOptions.Instance.Auditory.AttenuationRandom,
                    AttenuationRandom = SubOptions.Instance.Auditory.AttenuationRandom
                };*/
                _synthEx.Stop();
                _synthEx.CalibrateSequencer(panel._calibration.Frequency, panel._calibration.Duration);
                _synthEx.Start();
            };

            // Triggered when changes are made to parameters (Auditory model)
            panel.OnCommitParamsTask += delegate()
            {
                _synthEx.AttenuationSequencer = SubOptions.Instance.Auditory.Attenuation + SubOptions.Instance.Auditory.AttenuationRandom;
                _synthEx.AttenuationRandom = SubOptions.Instance.Auditory.AttenuationRandom;
            };

            // Triggered when quitting the options
            panel.OnCompleteTask += delegate() 
            {
                /*MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
                children = new MediaElement();
                int bug = SubOptions.Instance.Auditory.BufferLength;
                _synthEx = new Frequency2IGenerator(children, bug)
                {
                    AttenuationSequencer = SubOptions.Instance.Auditory.Attenuation + SubOptions.Instance.Auditory.AttenuationRandom,
                    AttenuationRandom = SubOptions.Instance.Auditory.AttenuationRandom
                };
                this._synthEx.Sequencer._freqChangedHook += new SequencerExt.FrequencyChanged(Sequencer__freqChangedHook);
                this._synthEx.Sequencer._freqPlayedHook += new SequencerExt.FrequencyPlayed(Sequencer__freqStartHook);
                this._synthEx.Sequencer._freqStoppedHook += new SequencerExt.FrequencyStopped(Sequencer__freqStopHook);
                this._synthEx.Sequencer._stepEndedHook += new SequencerExt.StepEnded(Sequencer__stepEndedHook);*/
                _synthEx.Stop();
                StateManager.Instance.setState(SubmarineStates.MAINMENU_STATE);
            };

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(panel);

           /* ButtonIcon btnFull = new ButtonIcon();
            btnFull.TextContent.Text = "Full Screen Mode";
            btnFull.Icon.Source = ResourceHelper.GetBitmap("/GameFramework;component/Media/fullscreen.png");
            //btnFull.Icon.Height = 22;
            //btnFull.Icon.Width = 31;
            btnFull.Width = 150;
            btnFull.Height = 40;
            btnFull.SetValue(Canvas.LeftProperty,40.0);
            btnFull.SetValue(Canvas.TopProperty, 540.0 - 40.0 - 25.0); 
            btnFull.Click += delegate(object sender, RoutedEventArgs e)
            {
                AuditoryGameApp.Current.Host.Content.IsFullScreen = !AuditoryGameApp.Current.Host.Content.IsFullScreen;
                
            };
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnFull);*/

        }

        private void exitOptions()
        {
            removeAllCanvasChildren();
            //_synthEx.Stop();
            this.allowConfiguration = false;

        }

        #endregion

    }

}

