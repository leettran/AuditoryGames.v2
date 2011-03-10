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
using LSRI.AuditoryGames.GameFramework;
using System.Windows.Threading;
using LSRI.TreasureHunter.Model;
using LSRI.TreasureHunter.UI;

namespace LSRI.TreasureHunter
{
    public class TreasureNugget : AnimatedGameObject
    {
        public enum TreasureType
        {
            TREASURE_NONE = 0,  /// dfdfdfdf
            TREASURE_GOLD = 1,   /// dfs dsdfasadf
            TREASURE_METAL = 2,   /// dfs dsdfasadf
        }

        protected const double SPEED = 75;
        protected static ResourcePool<TreasureNugget> resourcePool = new ResourcePool<TreasureNugget>();

        public delegate void NuggetLogic(double dt);
        protected NuggetLogic enemyLogic = null;

        protected const double TIME_EXPOSURE = 2;
        protected double timeSinceExposure = -1;

        static public TreasureNugget UnusedNuggets
        {
            get
            {
                return resourcePool.UnusedObject;
            }
        }

        public TreasureType Type {get; set;}
        public int Depth { get; set; }
        public int Score { get; set; }
        public int Index { get; set; }
        public bool IsExposed { get; set; }

        public TreasureNugget()
        {
            Depth = 0;
            Score = 0;
            Index = 0;
            IsExposed = true;
        }


        //public TreasureNugget startupBasicNugget(Point dimensions, AnimationData animationData, int score, int zLayer)
        public TreasureNugget startupBasicNugget(Point dimensions, int index, TreasureType type, int zLayer)
        {


            AnimationData animationData = new AnimationData(
                    new string[] { 
                        type==TreasureType.TREASURE_GOLD ? "Media/unknown.png" : "Media/unknown.png" },
                        0.0005);

            base.startupAnimatedGameObject(dimensions, animationData, ZLayers.PLAYER_Z, false);

            this.Visibility = System.Windows.Visibility.Collapsed;
            enemyLogic = new NuggetLogic(this.basicEnemyLogic);
            this._collisionName = CollisionIdentifiers.ENEMY;
            this.Score = 10;
            this.Type = type;
            this.Index = index;
            return this;
        }

        private void ChangeNuggetDisplay()
        {
            String strCnt="";
            if (TreasureOptions.Instance.Game.Display == TreasureGame.DisplayMode.Content)
            {
                if (Type == TreasureType.TREASURE_NONE)
                {
                    strCnt = "Media/hole1.png";
                }
                else
                {
                    if (!IsExposed)
                        strCnt = "Media/unknown.png";
                    else if (Type == TreasureType.TREASURE_GOLD) strCnt = "Media/gold1.png";
                    else if (Type == TreasureType.TREASURE_METAL) strCnt = "Media/metal1.png";
                    else strCnt = "Media/hole1.png";
                }
                this.Visibility = Visibility.Visible;
            }
            if (TreasureOptions.Instance.Game.Display == TreasureGame.DisplayMode.Position)
            {
                if (Type == TreasureType.TREASURE_NONE)
                {
                    strCnt = "Media/hole1.png";
                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    strCnt = "Media/unknown.png";
                    if (!IsExposed)
                    {
                        //strCnt = "Media/unknown.png";
                        this.Visibility = Visibility.Collapsed;
                    }
                    else this.Visibility = Visibility.Visible;
                }
            }
            if (TreasureOptions.Instance.Game.Display == TreasureGame.DisplayMode.None)
            {
                if (Type == TreasureType.TREASURE_NONE)
                {
                    strCnt = "Media/hole1.png";
                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    strCnt = "Media/unknown.png";
                    if (!IsExposed)
                    {
                        //strCnt = "Media/unknown.png";
                        this.Visibility = Visibility.Collapsed;
                    }
                    else this.Visibility = Visibility.Visible;
                }
            }

            animationData.frames = new string[] { strCnt };
            currentFrame = 0;
            prepareImage(animationData.frames[currentFrame]);
        }

        public void ChangeExposure(bool exposure)
        {
            IsExposed = exposure;
           ChangeNuggetDisplay();
        }

        public override void shutdown()
        {
            base.shutdown();
            enemyLogic = null;
        }

        protected void basicEnemyLogic(double dt)
        {
            //Position = new Point(Position.X, Position.Y + SPEED * dt);
            offscreenCheck();
        }

        private void ExposeNugget()
        {
         }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this != null && this.Rect != null)
                this.Rect.Dispatcher.BeginInvoke(() => this.ExposeNugget());
            (sender as DispatcherTimer).Stop();
        }


        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            if (inUse)
            {
                if (enemyLogic != null) enemyLogic(dt);
            }

            if (this.timeSinceExposure >= 0)
            {
                timeSinceExposure -= dt;
                if (timeSinceExposure <= 0)
                {
                    animationData.fps = 0.0005;
                    if (this.Type == TreasureType.TREASURE_METAL)
                        animationData.frames = new string[] { "Media/metal1.png" };
                    else
                        animationData.frames = new string[] { "Media/hole1.png" };
                    currentFrame = 0;
                    //animationData.frames[currentFrame] = "media/hole1.png";
                    prepareImage(animationData.frames[currentFrame]);
                    this.Type = TreasureType.TREASURE_NONE;
                }
            }
        }


        public override void collision(GameObject other)
        {
            base.collision(other);

            if (this.Type != TreasureType.TREASURE_NONE)
            {
                // Show explosion animation
                AnimatedGameObject.UnusedAnimatedGameObject.startupAnimatedGameObject(
                    new Point(55, 55),
                    new AnimationData(
                        new string[] { 
                            "Media/Explosion1.png", 
                            "Media/Explosion2.png", 
                            "Media/Explosion3.png", 
                            "Media/Explosion4.png", 
                            "Media/Explosion5.png", 
                            "Media/Explosion6.png", 
                            "Media/Explosion7.png" },
                        20),
                        ZLayers.PLAYER_Z,
                        true).Position = new Point(
                            Position.X + Dimensions.X / 2 - 55 / 2,
                            Position.Y + Dimensions.Y / 2 - 55 / 2);

                TreasureApplicationManager.Instance.Score += Score;
                string newString = TreasureOptions.Instance.Game._curSetup.Substring(0, this.Index) + "0" + TreasureOptions.Instance.Game._curSetup.Substring(this.Index + 1);
                TreasureOptions.Instance.Game._curSetup = newString;
                if (this.Type == TreasureType.TREASURE_GOLD)
                {
                    TreasureOptions.Instance.Game._curGold--;
                    TreasureOptions.Instance.User.CurrentGold++;
                    TreasureOptions.Instance.User.CurrentScore += this.Score;
                    (TreasureApplicationManager.Instance as TreasureApplicationManager)._scorePanel.Gold = TreasureOptions.Instance.Game._curGold;
                    (TreasureApplicationManager.Instance as TreasureApplicationManager)._scorePanel.Score = TreasureOptions.Instance.User.CurrentScore;
                }

                // Change visibility and initiate exposure animation
                this.Visibility = System.Windows.Visibility.Visible;
                this.timeSinceExposure = TreasureNugget.TIME_EXPOSURE;
                animationData.fps = 10;
                animationData.frames = new string[] { 
                            "Media/hole1.png", 
                            this.Type==TreasureType.TREASURE_GOLD ? "Media/gold1.png" : "Media/metal1.png"
                };
                currentFrame = 0;
                prepareImage(animationData.frames[currentFrame]);

             }

            if (true)
            {
                GamePage pg = AuditoryGameApp.Current.RootVisual as GamePage;
                ScorePanel pn = new ScorePanel();
                pn.SetValue(Canvas.LeftProperty, (pg.LayoutRoot.ActualWidth - pn.Width) / 2);
                pn.SetValue(Canvas.TopProperty, (pg.LayoutRoot.ActualHeight - pn.Height) / 2);
                pn.OnCompleteTask += delegate()
                {
                    StateManager.Instance.setState(States.START_STATE);
                    //StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
                };
                pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, pn);
            }
            else
                (TreasureApplicationManager.Instance as TreasureApplicationManager).UpdateSound();
           // this.shutdown();
        }
    }
}
