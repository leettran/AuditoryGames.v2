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

namespace AuditoryGames.GameFramework
{
    public class Enemy : AnimatedGameObject
    {
        protected const double SPEED = 75;
        protected static ResourcePool<Enemy> resourcePool = new ResourcePool<Enemy>();
        public delegate void EnemyLogic(double dt);
        protected EnemyLogic enemyLogic = null;
        protected int score = 0;

        static public Enemy UnusedEnemy
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

        public Enemy()
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

        public Enemy startupBasicEnemy(Point dimensions, AnimationData animationData, int score, int zLayer)
        {
            base.startupAnimatedGameObject(dimensions, animationData, zLayer, false);
            enemyLogic = new EnemyLogic(this.basicEnemyLogic);
            this.collisionName = CollisionIdentifiers.ENEMY;
            this.score = score;
            return this;
        }

        public override void shutdown()
        {
            base.shutdown();
            enemyLogic = null;
        }

        protected void basicEnemyLogic(double dt)
        {
            Position = new Point(Position.X, Position.Y + SPEED * dt);
            offscreenCheck();
        }

        public override void collision(GameObject other)
        {
            base.collision(other);
            
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

            ApplicationManager.Instance.Score += score;

            this.shutdown();
        }
    }
}
