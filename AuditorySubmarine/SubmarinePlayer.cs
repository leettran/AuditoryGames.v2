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
using System.Windows.Input;
using System.Windows.Threading;
using LSRI.AuditoryGames.GameFramework;
using LSRI.AuditoryGames.Utils;
using LSRI.AuditoryGames.GameFramework.Data;
using System.Windows.Controls;

namespace LSRI.Submarine
{
    /// <summary>
    /// Game object for the submarine
    /// </summary>
    public class SubmarinePlayer : AnimatedGameObject
    {
        /// <summary>
        /// 
        /// </summary>
        protected double _subSpeed = 50;
        protected double _subAccel = 0;
        protected bool _isBooster = false;

        private double _accTime = 0;

        //StopwatchPlus sp1;

        public int CanvasIndex { set; get; }

        public double Speed
        {
            get { return _subSpeed; }
            set { _subSpeed = value; }
        }

        public double Acceleration
        {
            get { return _subAccel; }
            set { _subAccel = value; }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="animationData"></param>
        /// <param name="zLayer"></param>
        public void startupSubmarine(Point dimensions, int zLayer)
        {
            base.startupAnimatedGameObject(
                dimensions, 
            /*    new AnimationData(new string[] {
                    "Media/asub1.png", 
                    "Media/asub3.png", 
                    "Media/asub4.png", 
                    "Media/asub2.png"
                },50),*/
                new AnimationData(new string[] {
                    "Media/ysubmarine2.png",
                   "Media/ysubmarine3.png",
                   "Media/ysubmarine4.png",
                   "Media/ysubmarine5.png",
                   "Media/ysubmarine6.png"
                }, 10), 
                zLayer, false);

            this.IsCroppable = CropTo.WidthOnly;
            this._collisionName = CollisionIdentifiers.PLAYER;
            this._collisionType = CollisionTypeIdentifiers.TIP;
            this.Speed = SubOptions.Instance.Game.SubmarineSpeed;
            this.Acceleration = 0;
            this._isBooster = false;

            _accTime = 0;

            /*sp1 = new StopwatchPlus(
                            sw => //Debug.WriteLine("Submarine starts at {0}", sw.EllapsedMilliseconds),
                            sw => //Debug.WriteLine("Submarine stops at {0}", sw.EllapsedMilliseconds),
                            (sw,msg) => //Debug.WriteLine("{0} at {1}", msg, sw.EllapsedMilliseconds)
            );
            sp1.Start();*/
            (IAppManager.Instance as SubmarineApplicationManager).Logger.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);
            _accTime += dt;

            bool _hasMoved = false;

            Canvas zone = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot;
            Point dim = new Point(zone.ActualWidth, zone.ActualHeight);
            
            int stepSize = SubOptions.Instance.Game.UnitSize;
            int nbUnitsInScreen = (int)dim.Y / stepSize;
            int screenMargin = ((int)dim.Y % stepSize) / 2;
            int gateExtent = SubOptions.Instance.Game.GateSize;
            int gateSize = 1 + 2 * SubOptions.Instance.Game.GateSize;

            if (KeyHandler.Instance.isKeyPressed(Key.Right))
            {
                /// RIGHT: Accelerate the submarine slightly
                _subAccel += SubOptions.Instance.Game.SubmarineAcceleration;
                KeyHandler.Instance.clearKeyPresses();
                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine accelerated");
                ////Debug.WriteLine("Speed : {0}", (_subSpeed + _subAccel));
                //sp1.Step("Submarine accelerated");
                //(IAppManager.Instance as SubmarineApplicationManager).myLogger.logAcceleration(Key.Right);

            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Space))
            {
                /// SPACE: Booster started (very fast acceleration)
                if (_isBooster == false)
                {
                    _isBooster = true;
                    (IAppManager.Instance as SubmarineApplicationManager).myLogger.logAcceleration(Key.Space);
                    _subAccel = 1000;
                    KeyHandler.Instance.clearKeyPresses();
                    (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine in booster mode");
                }

            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Up))
            {
                /// UP: move the submarine upward
                KeyHandler.Instance.clearKeyPresses();
                _hasMoved = true;
                CanvasIndex--;
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {
                /// DOWN: move the submarine downward
                KeyHandler.Instance.clearKeyPresses();
                _hasMoved = true;
                CanvasIndex++;
            }

            if (_hasMoved)
            {
                /// If the submarine has been moved laterally:
                /// - update its position accordingly 
                /// - regenerate the audio stimuli
                const int MOVE_RESTRICTION = 2;
                if (CanvasIndex <= MOVE_RESTRICTION) CanvasIndex = MOVE_RESTRICTION;
                if (CanvasIndex > nbUnitsInScreen - MOVE_RESTRICTION) CanvasIndex = nbUnitsInScreen - MOVE_RESTRICTION;
                //Debug.WriteLine("XXXXXXXXXXXXXXXX position : {0}", CanvasIndex);
                double SubLoc = screenMargin + CanvasIndex * stepSize;
                SubLoc = SubLoc - Dimensions.Y / 2 + stepSize / 2;
                Position = new Point(Position.X,  SubLoc);


                double deltaf = SubOptions.Instance.User.FrequencyDelta;
                double fqTraining = SubOptions.Instance.User.FrequencyTraining;

                double deltapos = (IAppManager.Instance as SubmarineApplicationManager)._gate.CanvasIndex - CanvasIndex;
                //double deltaf = fqDiff;//  fqTraining * .2;
                double dfpix = deltaf / SubOptions.Instance.Game.GateSize;
                //Debug.WriteLine("*********** deltaloc = {0}", deltapos);

                double fqComp = fqTraining - dfpix * deltapos;
                if (fqComp >= SubOptions.Instance.Auditory.MaxFrequency) fqComp = SubOptions.Instance.Auditory.MaxFrequency;
                if (fqComp <= SubOptions.Instance.Auditory.MinFrequency) fqComp = SubOptions.Instance.Auditory.MinFrequency;
                //SubOptions.Instance.User.FrequencyComparison = fqComp;
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.SetTargetFrequency(fqComp, false);

            }
            Position = new Point(Position.X + (this.Speed + this.Acceleration) * dt, Position.Y);
            

             // keep the player bound to the screen
            if (Position.X > (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X)
                Position = new Point((AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X, Position.Y);
            else if (Position.X < 0)
                Position = new Point(0, Position.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void shutdown()
        {
            base.shutdown();
            (IAppManager.Instance as SubmarineApplicationManager).Logger.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public override void collision(GameObject other)
        {
            base.collision(other);

            // stop the auditory stimuli
            (IAppManager.Instance as SubmarineApplicationManager)._synthEx.Stop();

            // show the gate
            (IAppManager.Instance as SubmarineApplicationManager)._gate.Visibility = Visibility.Visible;


            if (SubOptions.Instance._scoreBuffer.Count == 0)
            {
                SubOptions.Instance._scoreBuffer.Clear();
                for (int i = 0; i < SubOptions.Instance.Game.MaxGates; i++)
                {
                    SubOptions.Instance._scoreBuffer.Add(new SubOptions.ScorePattern());
                }
            }

            if (other is WallObject)
            {
                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine collides with WALL");
                // LOG EVENT
                (IAppManager.Instance as SubmarineApplicationManager).myLogger.logGateReach(false, SubOptions.Instance.User.FrequencyComparison,0,0);


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
                        Position.X + Dimensions.X * 2 / 3 - 55 / 2,
                        Position.Y + Dimensions.Y / 2 - 55 / 2);

                this.shutdown();
                //other.shutdown();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += delegate(object sender, EventArgs e)
                {
                    //SubmarineApplicationManager.Instance.Score = 0; ;
                    double currLife = SubOptions.Instance._scoreBuffer[SubOptions.Instance.User.CurrentGate].LifeLost;
                    SubOptions.Instance._scoreBuffer[SubOptions.Instance.User.CurrentGate].LifeLost++;
                    SubOptions.Instance.User.CurrentLife--;
                    (IAppManager.Instance as SubmarineApplicationManager)._scorePanel.Life = SubOptions.Instance.User.CurrentLife;

                    if (SubOptions.Instance.User.CurrentLife <= 0)
                    {

                        GamePage pg = AuditoryGameApp.Current.RootVisual as GamePage;
                        SubmarineScorePanel pn = new SubmarineScorePanel();

                        pn.SetValue(Canvas.LeftProperty, (pg.LayoutRoot.ActualWidth - pn.Width) / 2);
                        pn.SetValue(Canvas.TopProperty, (pg.LayoutRoot.ActualHeight - pn.Height) / 2);
                        pn.OnCompleteTask += delegate()
                        {
 
                            /// Failure to win level; make it easier and restart
                            SubOptions.Instance.User.CurrentGate = 0;
                            SubOptions.Instance.User.CurrentScore = 0;
                           // SubOptions.Instance._scoreBuffer.Clear();
                            // lives
 
                            SubOptions.Instance.User.CurrentLife = SubOptions.Instance.Game.MaxLives;
                            if (SubOptions.Instance.User.CurrentLevel == 1)
                            {
                            }
                            else if (SubOptions.Instance.User.CurrentLevel <= 8)
                            {
                                // FIRST 8 LEVELS : ACCOMMODATION - NO INCREASE OF DELTA
                            }
                            else
                            {
                                SubOptions.Instance.User.FrequencyDelta *= (1 + SubOptions.Instance.Auditory.Step);
                                if (SubOptions.Instance.User.FrequencyDelta > (SubOptions.Instance.User.FrequencyTraining * SubOptions.Instance.Auditory.Base))
                                {
                                    // Delta capped at base
                                    SubOptions.Instance.User.FrequencyDelta = SubOptions.Instance.User.FrequencyTraining * SubOptions.Instance.Auditory.Base;
                                }

                                for (int i = 0; i < SubOptions.Instance.User.Gates.Data.Length; i++)
                                {
                                    SubOptions.Instance.User.Gates.Data[i] *= (1 + SubOptions.Instance.Auditory.Step);
                                    if (SubOptions.Instance.User.Gates.Data[i] > SubOptions.Instance.Game.Gates.Data[i])
                                        SubOptions.Instance.User.Gates.Data[i] = SubOptions.Instance.Game.Gates.Data[i];
                                   /* SubOptions.Instance.User.Gates.Data[i] = Math.Min(
                                        SubOptions.Instance.Game.Gates.Data[i],
                                        SubOptions.Instance.User.Gates.Data[i]);*/
                                }
                            }
                            SubOptions.Instance._scoreBuffer.Clear();
                            StateManager.Instance.setState(SubmarineStates.MAINMENU_STATE);
                            //StateManager.Instance.setState(States.START_STATE);
                            //StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
                        };
                        // we have to insert any non GameObjects at the end of the children collection
                        pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, pn);
                        //pn.ForceQuit();
                    }
                    else
                    {

                        StateManager.Instance.setState(SubmarineStates.MAINMENU_STATE);
                        StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
                    }
                    (sender as DispatcherTimer).Stop();

                };

                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Start();

            }
            else if (other is GateObject || other is GateAnimatedObject)
            {
                // Submarine hits the gate
                (IAppManager.Instance as SubmarineApplicationManager)._gate.Visibility = Visibility.Visible;


                double tf = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTrainingFrequency();
                double cf = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTargetFrequency();
                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step(
                           String.Format("Submarine goes through gate : {0}", cf)
                           );


                this.shutdown();

 
                // Compute the score
                double baseScore = 100;

                // accuracy
                double deltapos = Math.Abs((IAppManager.Instance as SubmarineApplicationManager)._gate.CanvasIndex - CanvasIndex);
                double maxpos = SubOptions.Instance.Game.GateSize;
                double dartScore = Math.Max(0, 1 - deltapos / (maxpos + 1)) * baseScore;
                
                // time left
                double sTime = (IAppManager.Instance as SubmarineApplicationManager).Logger.EllapsedMilliseconds;
                double mTime = SubOptions.Instance.Game.TimeOnGate * 1000;
                double timeScore = (1 - sTime / mTime) * baseScore;

                // lives
                double currLife = SubOptions.Instance._scoreBuffer[SubOptions.Instance.User.CurrentGate].LifeLost;

                //double maxLife = SubOptions.Instance.Game.MaxLives;
                //double currLife = SubOptions.Instance.User.CurrentLife;
                //double lifeScore = (currLife / maxLife) * baseScore;

                //SubOptions.Instance._scoreBuffer.Add(new SubOptions.ScorePattern
                SubOptions.Instance._scoreBuffer[SubOptions.Instance.User.CurrentGate] = new SubOptions.ScorePattern
                {
                    GateAccuracy = dartScore,
                    TimeLeft = timeScore,
                    GatePosition = deltapos,
                    LifeLost = currLife
                };
                // LOG EVENT
                (IAppManager.Instance as SubmarineApplicationManager).myLogger.logGateReach(true, SubOptions.Instance.User.FrequencyComparison, dartScore, timeScore);

                SubOptions.Instance.User.CurrentGate++;

                // update total score
                SubOptions.Instance.User.CurrentScore += (int)(dartScore + timeScore);
                (IAppManager.Instance as SubmarineApplicationManager)._scorePanel.Gate = SubOptions.Instance.User.CurrentGate;
                (IAppManager.Instance as SubmarineApplicationManager)._scorePanel.Score = SubOptions.Instance.User.CurrentScore;


                if (SubOptions.Instance.User.CurrentGate >= SubOptions.Instance.Game.MaxGates)
                {
                    // if final gate, compute and show final score
                    SubOptions.Instance.User.CurrentGate = 0;
                    SubOptions.Instance.User.CurrentLife = SubOptions.Instance.Game.MaxLives;
                   // SubOptions.Instance.User.Scores.Data.Add(new HighScore()
                    SubOptions.Instance.User.Scores.Data.Add(new HighScore()
                    {
                        Delta = (int)SubOptions.Instance.User.FrequencyDelta,
                        Level = SubOptions.Instance.User.CurrentLevel,
                        Score = SubOptions.Instance.User.CurrentScore
                    });

                    GamePage pg = AuditoryGameApp.Current.RootVisual as GamePage;
                    SubmarineScorePanel pn = new SubmarineScorePanel();

                    pn.SetValue(Canvas.LeftProperty, (pg.LayoutRoot.ActualWidth - pn.Width) / 2);
                    pn.SetValue(Canvas.TopProperty, (pg.LayoutRoot.ActualHeight - pn.Height) / 2);
                    pn.OnCompleteTask += delegate()
                    {
                        SubOptions.Instance._scoreBuffer.Clear();
                        StateManager.Instance.setState(SubmarineStates.MAINMENU_STATE);
                        //StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
                    };
                    // we have to insert any non GameObjects at the end of the children collection
                    pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, pn);

                   // SubOptions.Instance._scoreBuffer.Clear();
                    SubOptions. Instance.User.FrequencyDelta *= (1 - SubOptions.Instance.Auditory.Step);
                    for (int i = 0; i < SubOptions.Instance.User.Gates.Data.Length; i++)
                    {
                        SubOptions.Instance.User.Gates.Data[i] *= (1 - SubOptions.Instance.Auditory.Step);
                        if (SubOptions.Instance.User.Gates.Data[i] > SubOptions.Instance.Game.Gates.Data[i])
                            SubOptions.Instance.User.Gates.Data[i] = SubOptions.Instance.Game.Gates.Data[i];
                        /*SubOptions.Instance.User.Gates.Data[i] = Math.Min(
                            SubOptions.Instance.Game.Gates.Data[i],
                            SubOptions.Instance.User.Gates.Data[i]);*/
                    }
                    SubOptions.Instance.User.CurrentScore = 0;
                    SubOptions.Instance.User.CurrentLevel++;

                }
                else
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Tick += delegate(object sender, EventArgs e)
                    {
                        StateManager.Instance.setState(SubmarineStates.MAINMENU_STATE);
                        if (SubOptions.Instance.User.CurrentGate != 0)
                            StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
                        (sender as DispatcherTimer).Stop();

                    };
                    timer.Interval = TimeSpan.FromSeconds(0.5);
                    timer.Start();
                }


            }
        }
    }
}
