/*  Auditory Training Games in Silverlight
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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LSRI.AuditoryGames.GameFramework
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

            if (_inUse)
            {
                if (enemyLogic != null) enemyLogic(dt);
            }
        }

        public Enemy startupBasicEnemy(Point dimensions, AnimationData animationData, int score, int zLayer)
        {
            base.startupAnimatedGameObject(dimensions, animationData, zLayer, false);
            enemyLogic = new EnemyLogic(this.basicEnemyLogic);
            this._collisionName = CollisionIdentifiers.ENEMY;
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

            IAppManager.Instance.Score += score;

            this.shutdown();
        }
    }
}
