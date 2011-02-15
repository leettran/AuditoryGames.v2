using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LSRI.AuditoryGames.GameFramework;

namespace LSRI.Submarine
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
        protected double _acceleration = 0;

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
            _acceleration = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            if (KeyHandler.Instance.isKeyPressed(Key.Right))
            {
                _acceleration += 50;
                KeyHandler.Instance.clearKeyPresses();
                Debug.WriteLine("Speed : {0}", (SPEED + _acceleration) / 4);
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Up))
            {
                Position = new Point(Position.X, Position.Y - 15/*5*SPEED * dt*/);
                double deltaf = 2500 * .1;
                double dfpix = deltaf / 4;
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.ChangeTargetFrequency(-dfpix);

                //Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency -= 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                KeyHandler.Instance.clearKeyPresses();
                Debug.WriteLine("position : {0}", Position.Y);
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {
                Position = new Point(Position.X, Position.Y + 15/*5*SPEED * dt*/);
                double deltaf = 2500 * .1;
                double dfpix = deltaf / 4;
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.ChangeTargetFrequency(dfpix);
                // Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency += 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                KeyHandler.Instance.clearKeyPresses();
                Debug.WriteLine("position : {0}", Position.Y);
            }
            Position = new Point(Position.X + (SPEED + _acceleration) / 5 * dt, Position.Y);
            

             // keep the player bound to the screen
            if (Position.X > (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X)
                Position = new Point((AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X, Position.Y);
            else if (Position.X < 0)
                Position = new Point(0, Position.Y);
            if (Position.Y > (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - dimensions.Y)
                Position = new Point(Position.X, (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - dimensions.Y);
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
                    //SubmarineApplicationManager.Instance.Score = 0; ;
                    StateManager.Instance.setState(States.START_STATE);
                    (sender as DispatcherTimer).Stop();

                };

                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Start();

            }
            else if (other is GateObject)
            {
                double tf = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTrainingFrequency();
                double cf = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTargetFrequency();

                Debug.WriteLine("Success : {0} - {1}", tf, cf);
                GameLevelDescriptor.CurrentGate--;
                if (GameLevelDescriptor.CurrentGate == 0)
                {
                    GameLevelDescriptor.CurrentLevel++;
                    GameLevelDescriptor.CurrentGate=5;
                    GameLevelDescriptor.ThresholdFrequency = (int)(GameLevelDescriptor.ThresholdFrequency * 0.90);
                }

                //IApplicationManager.Instance.Score += 50;;
                this.shutdown();
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.Stop();

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
