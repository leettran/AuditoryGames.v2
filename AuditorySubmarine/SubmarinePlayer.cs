﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AuditoryGames.GameFramework;

namespace AuditoryGames.Submarine
{
    /// <summary>
    /// 
    /// </summary>
    public class SubmarinePlayer : AnimatedGameObject
    {
        /// <summary>
        /// 
        /// </summary>
        protected const double SPEED = 200;

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="animationData"></param>
        /// <param name="zLayer"></param>
        public void startupSubmarine(Point dimensions, AnimationData animationData, int zLayer)
        {
            base.startupAnimatedGameObject(dimensions, animationData, zLayer, false);
            this.collisionName = CollisionIdentifiers.PLAYER;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            if (KeyHandler.Instance.isKeyPressed(Key.Up))
            {
                Position = new Point(Position.X, Position.Y - 10/*5*SPEED * dt*/);
                //Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency -= 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                KeyHandler.Instance.clearKeyPresses();
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {
                Position = new Point(Position.X, Position.Y + 10/*5*SPEED * dt*/);
               // Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency += 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                KeyHandler.Instance.clearKeyPresses();
            }
            Position = new Point(Position.X + SPEED/4 * dt, Position.Y);
            

             // keep the player bound to the screen
            if (Position.X > (App.Current.RootVisual as Page).LayoutRoot.ActualWidth - dimensions.X)
                Position = new Point((App.Current.RootVisual as Page).LayoutRoot.ActualWidth - dimensions.X, Position.Y);
            else if (Position.X < 0)
                Position = new Point(0, Position.Y);
            if (Position.Y > (App.Current.RootVisual as Page).LayoutRoot.ActualHeight - dimensions.Y)
                Position = new Point(Position.X, (App.Current.RootVisual as Page).LayoutRoot.ActualHeight - dimensions.Y);
            else if (Position.Y < 0)
                Position = new Point(Position.X, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void shutdown()
        {
            base.shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public override void collision(GameObject other)
        {
            base.collision(other);
            //(IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Stop();

            if (other is WallObject)
            {
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

                this.shutdown();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += delegate(object sender, EventArgs e)
                {
                    //ApplicationManager.Instance.Score = 0; ;
                    StateManager.Instance.setState(States.START_STATE);
                    (sender as DispatcherTimer).Stop();

                };

                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Start();

            }
            else if (other is GateObject)
            {
                //IApplicationManager.Instance.Score += 50;;
                this.shutdown();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += delegate(object sender, EventArgs e)
                {
                    StateManager.Instance.setState(States.START_STATE);
                    StateManager.Instance.setState(SubmarineStates.LEVEL_STATE);
                    (sender as DispatcherTimer).Stop();

                };
                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Start();



            }
        }
    }
}