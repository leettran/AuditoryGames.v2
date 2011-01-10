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

namespace AuditoryGames.GameFramework
{

    public partial class IApplicationManager
    {
        public int dev1 = 0;
    }

    public class ApplicationManager
    {
        protected const string SCORE_ISOSTORE_NAME = "score";
        protected const double TIME_BETWEEN_ENEMIES = .2;
        protected const double TIME_BETWEEN_BACKGROUNDS = .3;

        protected static ApplicationManager instance = null;
        protected Player plane = null;    
        protected Random rand = new Random((int)DateTime.Now.Ticks);
        protected double timeSinceLastEnemy = 0;
        protected double timeSinceLastBackground = 0;
        protected int score = 0;
        protected TextBlock txtbScore = null;

        public static ApplicationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ApplicationManager();
                return instance;
            }
        }

        public int SavedScore
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(SCORE_ISOSTORE_NAME))
                {
                    int? value = IsolatedStorageSettings.ApplicationSettings[SCORE_ISOSTORE_NAME] as int?;
                    if (value != null) return value.Value;
                }

                return 0;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[SCORE_ISOSTORE_NAME] = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
                if (txtbScore != null) txtbScore.Text = score.ToString();
            }
        }

        protected ApplicationManager()
        {

        }

        protected void removeAllCanvasChildren()
        {
            UIElementCollection children = (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children;
            while (children.Count != 0)
                children.RemoveAt(0);
        }

        public void startupApplicationManager()
        {
            StateManager.Instance.registerStateChange(
                States.START_STATE,
                new StateChangeInfo.StateFunction(startMainMenu),
                new StateChangeInfo.StateFunction(endMainMenu));

            StateManager.Instance.registerStateChange(
                "game",
                new StateChangeInfo.StateFunction(startGame),
                new StateChangeInfo.StateFunction(exitGame));

            Score = SavedScore;
        }

        public void startMainMenu()
        {
            Button btnStart = new Button();
            btnStart.Content = "Start Game";
            btnStart.Width = 100;
            btnStart.Height = 35;
            btnStart.SetValue(Canvas.LeftProperty, 490.0);
            btnStart.SetValue(Canvas.TopProperty, 355.0);
            btnStart.Click += delegate(object sender, RoutedEventArgs e) { StateManager.Instance.setState("game"); };
            (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnStart);

            Button btnFull = new Button();
            btnFull.Content = "Start Game";
            btnFull.Width = 100;
            btnFull.Height = 35;
            btnFull.SetValue(Canvas.LeftProperty, 50.0);
            btnFull.SetValue(Canvas.TopProperty, 50.0);
            btnFull.Click += delegate(object sender, RoutedEventArgs e) {
                AuditoryGameApp.Current.Host.Content.IsFullScreen = !AuditoryGameApp.Current.Host.Content.IsFullScreen;
            };
            (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(btnFull);

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

            plane = new Player();
            plane.startupPlayer(
                new Point(59, 43),
                new AnimationData(
                    new string[] { "Media/brownplane1.png", "Media/brownplane2.png", "Media/brownplane3.png" },
                    10),
                ZLayers.PLAYER_Z);
            plane.Position = new Point(150, 75);

            txtbScore = new TextBlock();
            txtbScore.Text = ApplicationManager.Instance.Score.ToString();
            txtbScore.Width = 100;
            txtbScore.Height = 35;
            txtbScore.FontSize = 20;
            txtbScore.Foreground = new SolidColorBrush(Colors.White);
            txtbScore.SetValue(Canvas.LeftProperty, 10.0);
            txtbScore.SetValue(Canvas.TopProperty, 10.0);
            // we have to insert any non GameObjects at the end of the children collection
            (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Insert(
                (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Count, txtbScore);
        }

        public void exitGame()
        {
            while (GameObject.gameObjects.Count != 0)
                GameObject.gameObjects[0].shutdown();            

            removeAllCanvasChildren();

            txtbScore = null;
        }

        public void enterFrame(double dt)
        {
            if (KeyHandler.Instance.isKeyPressed(Key.Escape) && StateManager.Instance.CurrentState.Equals("game"))
                StateManager.Instance.setState(States.START_STATE);

            
            timeSinceLastEnemy -= dt;
            timeSinceLastBackground -= dt;

            if (timeSinceLastEnemy <= 0)
            {
                timeSinceLastEnemy = TIME_BETWEEN_ENEMIES;
                
                Enemy.UnusedEnemy.startupBasicEnemy(
                    new Point(32, 32),
                    new AnimationData(
                        new string[] { "Media/blueplane1.png", "Media/blueplane2.png", "Media/blueplane3.png" },
                        10),
                    10,
                    ZLayers.PLAYER_Z)
                    .Position =
                        new Point(rand.Next(0, (int)(AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth), -32);
            }

            if (timeSinceLastBackground <= 0)
            {
                timeSinceLastBackground = TIME_BETWEEN_BACKGROUNDS;
                
                BackgroundGameObject.UnusedBackgroundGameObject.startupBackgroundGameObject(
                    new Point(65, 65),
                    "Media/bigisland.png",
                    ZLayers.BACKGROUND_Z)
                    .Position =
                        new Point(rand.Next(0, (int)(AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight), -65);
            }
        }

        public void shutdown()
        {
            SavedScore = Score;
        }
    }
}
