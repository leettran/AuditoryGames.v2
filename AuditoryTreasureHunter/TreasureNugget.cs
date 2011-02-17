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
        protected int score = 0;
        protected int index = 0;

        protected const double TIME_EXPOSURE = 2;
        protected double timeSinceExposure = -1;

        static public TreasureNugget UnusedNuggets
        {
            get
            {
                return resourcePool.UnusedObject;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
        }

        public TreasureType Type {get; set;}

        public TreasureNugget()
        {
        }


        //public TreasureNugget startupBasicNugget(Point dimensions, AnimationData animationData, int score, int zLayer)
        public TreasureNugget startupBasicNugget(Point dimensions, int index, TreasureType type, int zLayer)
        {
            AnimationData animationData = new AnimationData(
                    new string[] { 
                        type==TreasureType.TREASURE_GOLD ? "Media/gold1.png" : "Media/metal1.png" },
                        0.0005);

            base.startupAnimatedGameObject(dimensions, animationData, ZLayers.PLAYER_Z, false);

            this.Visibility = System.Windows.Visibility.Collapsed;
            enemyLogic = new NuggetLogic(this.basicEnemyLogic);
            this._collisionName = CollisionIdentifiers.ENEMY;
            this.score = 10;
            this.Type = type;
            this.index = index;
            return this;
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

                TreasureApplicationManager.Instance.Score += score;
                string newString = GameLevelInfo._curSetup.Substring(0, index) + "0" + GameLevelInfo._curSetup.Substring(index + 1);
                GameLevelInfo._curSetup = newString;

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
            (TreasureApplicationManager.Instance as TreasureApplicationManager).UpdateSound();
           // this.shutdown();
        }
    }
}
