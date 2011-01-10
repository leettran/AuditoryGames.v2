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
using AuditoryGames.GameFramework;

namespace AuditoryGames.TreasureHunter
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

        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            if (inUse)
            {
                if (enemyLogic != null) enemyLogic(dt);
            }
        }

        //public TreasureNugget startupBasicNugget(Point dimensions, AnimationData animationData, int score, int zLayer)
        public TreasureNugget startupBasicNugget(Point dimensions, int index, TreasureType type, int zLayer)
        {
            AnimationData animationData = new AnimationData(
                    new string[] { 
                        type==TreasureType.TREASURE_GOLD ? "Media/gold1.png" : "Media/metal1.png" },
                        .02);

            base.startupAnimatedGameObject(dimensions, animationData, ZLayers.PLAYER_Z, false);

            enemyLogic = new NuggetLogic(this.basicEnemyLogic);
            this.collisionName = CollisionIdentifiers.ENEMY;
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

        public override void collision(GameObject other)
        {
            base.collision(other);

            if (this.Type == TreasureType.TREASURE_NONE) return;
            
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
            this.Type = TreasureType.TREASURE_NONE;
            string newString = GameLevelInfo._curSetup.Substring(0, index) + "0" + GameLevelInfo._curSetup.Substring(index+1);
            GameLevelInfo._curSetup = newString;
            animationData.frames[currentFrame] = "media/hole1.png";
            prepareImage(animationData.frames[currentFrame]);
            (TreasureApplicationManager.Instance as TreasureApplicationManager).UpdateSound();
           // this.shutdown();
        }
    }
}
