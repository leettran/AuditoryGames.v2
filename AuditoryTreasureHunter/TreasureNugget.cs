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
using LSRI.AuditoryGames.GameFramework;
using System.Windows.Threading;
using LSRI.TreasureHunter.Model;
using LSRI.TreasureHunter.UI;
using LSRI.AuditoryGames.GameFramework.Data;

namespace LSRI.TreasureHunter
{
    /// <summary>
    /// Implementation of the nuggets in the TreasureHunter game.
    /// </summary>
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
            this.timeSinceExposure = -1;
            enemyLogic = new NuggetLogic(this.basicEnemyLogic);
            this._collisionName = CollisionIdentifiers.ENEMY;
            this.Score = 10;
            this.Type = type;
            this.Index = index;
            return this;
        }

        private void FogOfWar()
        {
            if (timeSinceExposure > 0) return;
            //int nbStep = TreasureOptions.Instance.User.Actions;
            double nb = TreasureOptions.Instance.Game.Zones * (1.5);
            double delta = TreasureOptions.Instance.User.VisualTiming.Data[TreasureOptions.Instance.User._currExposure];
            TreasureOptions.Instance.nExposedX = (int)(delta * TreasureOptions.Instance.Game.Zones);
            TreasureOptions.Instance.nExposedY = (int)(delta * TreasureOptions.Instance.Game.Depth);
            String strCnt = "";
            if (Type == TreasureType.TREASURE_NONE)
            {
                strCnt = "Media/hole1.png";
            }
            else
            {
                strCnt = "Media/unknown.png";
                if (this.Depth < TreasureOptions.Instance.nExposedY)
                {
                   // if (Type == TreasureType.TREASURE_GOLD) strCnt = "Media/gold1.png";
                   // else if (Type == TreasureType.TREASURE_METAL) strCnt = "Media/metal1.png";
                }
     
            }
            if (!IsExposed && Type != TreasureType.TREASURE_NONE)
            {
                //strCnt = "Media/unknown.png";
                this.Visibility = Visibility.Collapsed;
            }
            else this.Visibility = Visibility.Visible;
            animationData.frames = new string[] { strCnt };
            currentFrame = 0;
            prepareImage(animationData.frames[currentFrame]);
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
           //ChangeNuggetDisplay();
            FogOfWar();
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

      /*  private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this != null && this.Rect != null)
                this.Rect.Dispatcher.BeginInvoke(() => this.ExposeNugget());
            (sender as DispatcherTimer).Stop();
        }*/


        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            if (_inUse)
            {
                if (enemyLogic != null) enemyLogic(dt);
            }

            if (this.timeSinceExposure >= 0)
            {
                timeSinceExposure -= dt;
                if (timeSinceExposure <= 0)
                {
                    animationData.fps = 0.0005;
                   // if (this.Type == TreasureType.TREASURE_METAL)
                    //    animationData.frames = new string[] { "Media/metal1.png" };
                    //else
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

            int oldScore = 0;

            double l = (TreasureApplicationManager.Instance as TreasureApplicationManager)._synthEx.Left;
            double m = (TreasureApplicationManager.Instance as TreasureApplicationManager)._synthEx.Middle;
            double r = (TreasureApplicationManager.Instance as TreasureApplicationManager)._synthEx.Right;
            (TreasureApplicationManager.Instance as TreasureApplicationManager).myLogger.logHitNugget((int)this.Type, this.Depth,this.Score,l,m,r);

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
                animationData.fps = 5;
                animationData.frames = new string[] { 
                            "Media/hole1.png", 
                            this.Type==TreasureType.TREASURE_GOLD ? "Media/gold1.png" : "Media/metal1.png"
                };
                currentFrame = 0;
                oldScore = this.Score;
                this.Score = 0;
                prepareImage(animationData.frames[currentFrame]);
                // (TreasureApplicationManager.Instance as TreasureApplicationManager).changeExposure();
            }
            HunterWeapon tt = other as HunterWeapon;
            ScoreObject tttt = ScoreObject.UnusedScore.startupGameObject((tt != null) ? tt._player : null, oldScore);

            tttt.Position = new Point(
                        Position.X + Dimensions.X / 2 - 55 / 2,
                        Position.Y + Dimensions.Y / 2 - 55 / 2);


            if (TreasureOptions.Instance.Game._curGold == 0 || TreasureOptions.Instance.User.CurrentLife == 0)
            {
                StateManager.Instance.setState(TreasureStates.SCORE_STATE);

            }
            else
            {
                (TreasureApplicationManager.Instance as TreasureApplicationManager).UpdateSound(this.Index);
                TreasureApplicationManager.PREVENT_AUDIO_CHANGES = false;
            }
           // this.shutdown();
        }
    }
}
