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

namespace LSRI.TreasureHunter
{

    public static class GameLevelInfo
    {
        public static int _gameMode = 0;
        public static int _curLevel = 1;
        public static int _nbTreasureZones = 10;
        public static int _sizeZones = 0;
        public static String _curSetup = "";

        public static List<String> setup = new List<String>();

        public static void GenerateStrings(string curr, int gold,int size,List<String> holder)
        {
            if (gold == 0)
            {
                StringBuilder sb1 = new StringBuilder(curr);
                for (int i=0;i<size;i++) sb1.Append('0');
                holder.Add(sb1.ToString());
                return;
            }
            else if (size == 0) return;
            else if (gold > size) return;
            else if (curr.Length <= 1)
            {
                //String ff = curr + '1';
                StringBuilder sb1 = new StringBuilder(curr);
                sb1.Append('1');
                StringBuilder sb2 = new StringBuilder(curr);
                sb2.Append('0');
                GenerateStrings(sb1.ToString(), gold - 1, size - 1, holder);
                GenerateStrings(sb2.ToString(), gold, size - 1, holder);
            }
            else
            {
                if ((curr[curr.Length - 1] == '1' && curr[curr.Length - 2] == '1'))
                {
                    //String ff = curr + '0';
                    StringBuilder sb1 = new StringBuilder(curr);
                    sb1.Append('0');
                    GenerateStrings(sb1.ToString(), gold, size - 1, holder);
                }
                else
                {
                    //tring ff = curr + '1';
                    StringBuilder sb1 = new StringBuilder(curr);
                    sb1.Append('1');
                    StringBuilder sb2 = new StringBuilder(curr);
                    sb2.Append('0');
                    GenerateStrings(sb1.ToString(), gold - 1, size - 1, holder);
                    GenerateStrings(sb2.ToString(), gold, size - 1, holder);
                }
            }

        }
    }

    public static class GameLayout
    {
        public static int SKY_MARGIN = 200;
    }

    public class TreasureApplicationManager : IAppManager
    {
        protected const string SCORE_ISOSTORE_NAME = "score";
        protected const double TIME_BETWEEN_ENEMIES = .2;
        protected const double TIME_BETWEEN_BACKGROUNDS = .3;

        public static Boolean PREVENT_AUDIO_CHANGES = false;

        //protected static TreasureApplicationManager instance = null;
        protected Random rand = new Random((int)DateTime.Now.Ticks);
        protected double timeSinceLastEnemy = 0;
        protected double timeSinceLastBackground = 0;
        protected int score = 0;
        protected TextBlock txtbScore = null;

        public static Boolean PLAY_CUES_ONCE = true;

        public Frequency3IGenerator _synthEx = null;

        /// <summary>
        /// 
        /// </summary>
        protected TreasureHunter _player = null;

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
 
        }


        private void AppMgr_StimuliStarted()
        {
            Debug.WriteLine("---> AUDIO CUE started " + DateTime.Now.ToString("HH:MM:ss.FFFFFF"));
            PREVENT_AUDIO_CHANGES = true;
        }

        private void AppMgr_StimuliStopped()
        {
            Debug.WriteLine("---> AUDIO CUE stopped" + DateTime.Now.ToString("HH:MM:ss.FFFFFF"));
            PREVENT_AUDIO_CHANGES = false;
        }

        public override void startupApplicationManager()
        {
            MediaElement children = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).AudioPlayer;
            _synthEx = new Frequency3IGenerator(children);

            StateManager.Instance.registerStateChange(
                States.START_STATE,
                new StateChangeInfo.StateFunction(startMainMenu),
                new StateChangeInfo.StateFunction(endMainMenu));

            StateManager.Instance.registerStateChange(
                "game",
                new StateChangeInfo.StateFunction(startGame),
                new StateChangeInfo.StateFunction(exitGame));

            Score = SavedScore;
            //(App.Current.RootVisual as Page).AudioPlayer.SetSource(_synthEx);
        }

        public void startMainMenu()
        {
            ButtonIcon btnStart = new ButtonIcon();
            //btnStart.Content = "Start Game (Proximity mode)";
            btnStart.TextContent.Text = "Start Game (Proximity mode)";
            btnStart.Icon.Source = ResourceHelper.GetBitmap("Media/smallisland.png");
            btnStart.Width = 250;
            btnStart.Height = 45;
            btnStart.SetValue(Canvas.LeftProperty, 490.0);
            btnStart.SetValue(Canvas.TopProperty, 300.0);
            btnStart.Click += delegate(object sender, RoutedEventArgs e) {
                GameLevelInfo._gameMode = 0;
                StateManager.Instance.setState("game"); 
            };
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnStart);

            btnStart = new ButtonIcon();
            btnStart.TextContent.Text = "Start Game (Value mode)";
            //btnStart.Icon.Source = ResourceHelper.GetBitmap("Media/smallisland.png");
            btnStart.Width = 250;
            btnStart.Height = 45;
            btnStart.SetValue(Canvas.LeftProperty, 490.0);
            btnStart.SetValue(Canvas.TopProperty, 370.0);
            btnStart.IsEnabled = true;
            btnStart.Click += delegate(object sender, RoutedEventArgs e)
            {
                GameLevelInfo._gameMode = 1;
                StateManager.Instance.setState("game");
            };
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnStart);

            btnStart = new ButtonIcon();
            btnStart.TextContent.Text = "Start Game (distance mode)";
            //btnStart.Icon.Source = ResourceHelper.GetBitmap("Media/smallisland.png");
            btnStart.Width = 250;
            btnStart.Height = 45;
            btnStart.IsEnabled = true;
            btnStart.SetValue(Canvas.LeftProperty, 490.0);
            btnStart.SetValue(Canvas.TopProperty, 440.0);
            btnStart.Click += delegate(object sender, RoutedEventArgs e)
            {
                GameLevelInfo._gameMode = 2;
                StateManager.Instance.setState("game");
            };
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnStart);

            
            Button btnFull = new Button();
            btnFull.Content = "Full Screen Mode";
            btnFull.Width = 150;
            btnFull.Height = 35;
            btnFull.SetValue(Canvas.LeftProperty, 50.0);
            btnFull.SetValue(Canvas.TopProperty, 50.0);
            btnFull.Click += delegate(object sender, RoutedEventArgs e) {
                AuditoryGameApp.Current.Host.Content.IsFullScreen = !AuditoryGameApp.Current.Host.Content.IsFullScreen;
            };
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnFull);


           /* GameParameters param = new GameParameters();
            param.NbZone.Value = GameLevelInfo._nbTreasureZones;

            param.SetValue(Canvas.LeftProperty, 490.0);
            param.SetValue(Canvas.TopProperty, 50.0);
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(param);*/

            MapViewer mp = new MapViewer();
            mp.SetValue(Canvas.LeftProperty, 50.0);
            mp.SetValue(Canvas.TopProperty, 250.0);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(mp);
        }

        public void endMainMenu()
        {
            removeAllCanvasChildren();
        }

        public void startGame()
        {
            // initialise collisions
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMY);
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYER, CollisionIdentifiers.ENEMYWEAPON);
            CollisionManager.Instance.addCollisionMapping(CollisionIdentifiers.PLAYERWEAPON, CollisionIdentifiers.ENEMY);

            // 
            Canvas cv = (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot;
            //GameLevelInfo._nbTreasureZones++;
            GameLevelInfo._sizeZones = ((int)cv.ActualWidth) / GameLevelInfo._nbTreasureZones;

            GameLevelInfo.setup.Clear();
            GameLevelInfo.GenerateStrings("", GameLevelInfo._nbTreasureZones - (GameLevelInfo._nbTreasureZones/3), GameLevelInfo._nbTreasureZones, GameLevelInfo.setup);
            //Debug.WriteLine(String.Join(" - ", GameLevelInfo.setup.ToArray()));
            String settings = GameLevelInfo.setup[rand.Next(0, GameLevelInfo.setup.Count-1)];
            GameLevelInfo._curSetup = settings;
            Debug.WriteLine("game settiungs : " + settings);

            //// initialise game objects
            // background image
            BackgroundTreasureGameObject bgImage = new BackgroundTreasureGameObject()
            {
                ImageStretch = Stretch.Fill
            };
            bgImage.startupBackgroundGameObject(
                new Point(cv.ActualWidth, cv.ActualHeight - GameLayout.SKY_MARGIN),
               "Media/bg.png", -1);
            bgImage.Position = new Point(0, GameLayout.SKY_MARGIN);

            // rail object
            BackgroundTreasureGameObject railImage = new BackgroundTreasureGameObject()
            {
                ImageStretch = Stretch.Fill
            };
            railImage.startupGameObject(
                new Point(cv.ActualWidth, 15),
               "Media/rail.png", -1);
            railImage.Position = new Point(0, GameLayout.SKY_MARGIN - 10);

            // player object
            _player = new TreasureHunter();
            _player.startupPlayer(
                new Point(73,85),
                new AnimationData(
                    new string[] { "Media/bob.png" },
                    1),
                ZLayers.PLAYER_Z);

            _player.CurrentZone =  (GameLevelInfo._nbTreasureZones / 2);
            int currLoc = GameLevelInfo._sizeZones * _player.CurrentZone + (GameLevelInfo._sizeZones - (int)_player.Dimensions.X) / 2;
            _player.Position = new Point(currLoc, GameLayout.SKY_MARGIN - _player.Dimensions.Y - 10);

            
            // treasure nuggets
            double dd = bgImage.Dimensions.Y-56-50;

            for (int i = 0; i < GameLevelInfo._nbTreasureZones; i++)
            {
                Point pt = new Point(GameLevelInfo._sizeZones * i, dd);
                pt.X = pt.X + (GameLevelInfo._sizeZones - 56) / 2;
                pt.Y = GameLayout.SKY_MARGIN + 25 + this.rand.Next(0, (int)dd);
                Boolean isGold = (settings[i] == '1');

                TreasureNugget.UnusedNuggets.startupBasicNugget(
                    new Point(56,40),
                    i,
                    isGold ? TreasureNugget.TreasureType.TREASURE_GOLD : TreasureNugget.TreasureType.TREASURE_METAL,
                    ZLayers.PLAYER_Z)
                .Position = pt;
            }
            

            // score window (TO DO AGAIN)
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
            //    (LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Count, txtbScore);

            ScoreControl ff = new ScoreControl();
            ff.SetValue(Canvas.LeftProperty, 10.0);
            ff.SetValue(Canvas.TopProperty, 10.0);
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Insert(
                (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Count, ff);

            float freqL = 0;
            float freqM = 0;
            float freqR = 0;
            if (GameLevelInfo._gameMode == 0)
            {
                freqL = (GameLevelInfo._curSetup[_player.CurrentZone - 1] == '1') ? 5000 : 3000;
                freqM = (GameLevelInfo._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (GameLevelInfo._curSetup[_player.CurrentZone + 1] == '1') ? 5000 : 3000;
            }
            else if (GameLevelInfo._gameMode == 1)
            {
                String left = GameLevelInfo._curSetup.Substring(0, _player.CurrentZone);
                String right = GameLevelInfo._curSetup.Substring(_player.CurrentZone+1);
                int nbL = left.LastIndexOf('1');
                int nbR = right.IndexOf('1');
                if (nbL != -1) nbL = left.Length - nbL - 1;
                else nbL += 100001;
                if (nbR == -1) nbR += 100001;


                freqL = (nbL < nbR) ? 5000 : 3000;
                freqM = (GameLevelInfo._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (nbL >= nbR ) ? 5000 : 3000;
            }
            else if (GameLevelInfo._gameMode == 2)
            {
                String left = GameLevelInfo._curSetup.Substring(0, _player.CurrentZone);
                String right = GameLevelInfo._curSetup.Substring(_player.CurrentZone + 1);
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
                freqM = (GameLevelInfo._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (nbL < nbR) ? 5000 : 3000;
                

            }
           /* _synthEx.Arpeggiator.Notes[0].Frequency = freqL;
            _synthEx.Arpeggiator.Notes[2].Frequency = freqM;
            _synthEx.Arpeggiator.Notes[4].Frequency = freqR;
            _synthEx.Arpeggiator.Start();*/
            //MediaElement children = (LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).AudioPlayer;
            //this._synthEx.sequencer.StepIndex = this._synthEx.sequencer.StepCount - 1;
            //this._synthEx.ResetSequencer();
           // children.Play();
            //this._synthEx.Start();
            UpdateSound();
        }

        public void exitGame()
        {
            this._synthEx.Stop();
            //MediaElement children = (LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).AudioPlayer;
            //children.Stop();
            //this._synthEx.sequencer.Reset();
            while (GameObject.gameObjects.Count != 0)
                GameObject.gameObjects[0].shutdown();            

            removeAllCanvasChildren();
           // _synthEx.Arpeggiator.Stop();

            txtbScore = null;
        }

        public override void enterFrame(double dt)
        {
            if (KeyHandler.Instance.isKeyPressed(Key.Escape) && StateManager.Instance.CurrentState.Equals("game"))
                StateManager.Instance.setState(States.START_STATE);

            if (KeyHandler.Instance.isKeyPressed(Key.Down) && StateManager.Instance.CurrentState.Equals("game"))
            {
                Visibility? vis = null;
                for (int i = 0; i < GameObject.gameObjects.Count; ++i)
                {
                    TreasureNugget nug = GameObject.gameObjects[i] as TreasureNugget;
                    if (nug == null) continue;
                    if (nug.Type == TreasureNugget.TreasureType.TREASURE_NONE) continue;
                    if (vis == null) vis = (nug.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
                    nug.Visibility = (Visibility)vis;
                }
            }

            
            timeSinceLastEnemy -= dt;
            timeSinceLastBackground -= dt;

        /*    if (timeSinceLastEnemy <= 0)
            {
                timeSinceLastEnemy = TIME_BETWEEN_ENEMIES;
                
                TreasureNugget.UnusedEnemy.startupBasicEnemy(
                    new Point(32, 32),
                    new AnimationData(
                        new string[] { "Media/blueplane1.png", "Media/blueplane2.png", "Media/blueplane3.png" },
                        10),
                    10,
                    ZLayers.PLAYER_Z)
                    .Position =
                        new Point(rand.Next(0, (int)(LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualWidth), 32);
            }*/

        /*    if (timeSinceLastBackground <= 0)
            {
                timeSinceLastBackground = TIME_BETWEEN_BACKGROUNDS;
                
                BackgroundTreasureGameObject.UnusedBackgroundGameObject.startupBackgroundGameObject(
                    new Point(65, 65),
                    "Media/bigisland.png",
                    ZLayers.BACKGROUND_Z)
                    .Position =
                        new Point(rand.Next(0, (int)(LSRI.AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualHeight), -65);
            }*/
        }

        public override void shutdown()
        {
            SavedScore = Score;
        }


        public void UpdateSound()
        {
            double freqL = 0;
            double freqM = 0;
            double freqR = 0;
            if (GameLevelInfo._gameMode == 0)
            {
                freqL = ((_player.CurrentZone - 1) >= 0 && GameLevelInfo._curSetup[_player.CurrentZone - 1] == '1') ? 5000 : 3000;
                freqM = (GameLevelInfo._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (((_player.CurrentZone + 1) < GameLevelInfo._curSetup.Length) &&
                                GameLevelInfo._curSetup[_player.CurrentZone + 1] == '1') ? 5000 : 3000;
            }
            else if (GameLevelInfo._gameMode == 1)
            {
                String left = GameLevelInfo._curSetup.Substring(0, _player.CurrentZone);
                String right = GameLevelInfo._curSetup.Substring(_player.CurrentZone + 1);
                int nbL = left.LastIndexOf('1');
                int nbR = right.IndexOf('1');
                if (nbL != -1) nbL = left.Length - nbL - 1;
                else nbL += 100001;
                if (nbR == -1) nbR += 100001;


                freqL = (nbL < nbR) ? 5000 : 3000;
                freqM = (GameLevelInfo._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (nbL >= nbR) ? 5000 : 3000;
            }
            else if (GameLevelInfo._gameMode == 2)
            {
                String left = GameLevelInfo._curSetup.Substring(0, _player.CurrentZone);
                String right = GameLevelInfo._curSetup.Substring(_player.CurrentZone + 1);
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
                freqM = (GameLevelInfo._curSetup[_player.CurrentZone] == '1') ? 5000 : 3000;
                freqR = (nbL < nbR) ? 5000 : 3000;


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

    }
}
