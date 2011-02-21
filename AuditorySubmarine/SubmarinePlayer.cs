using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LSRI.AuditoryGames.GameFramework;
using LSRI.AuditoryGames.Utils;

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
        protected double _subSpeed = 50;
        protected double _subAccel = 0;

        private double _accTime = 0;

        StopwatchPlus sp1;


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
                new AnimationData(new string[] {
                    "Media/asub1.png", 
                    "Media/asub3.png", 
                    "Media/asub4.png", 
                    "Media/asub2.png"
                },50), 
                zLayer, false);

            this._collisionName = CollisionIdentifiers.PLAYER;
            this._collisionType = CollisionTypeIdentifiers.TIP;
            this.Speed = SubOptions.Instance.Game.SubmarineSpeed;
            this.Acceleration = 0;

            _accTime = 0;

            sp1 = new StopwatchPlus(
                            sw => Debug.WriteLine("Submarine starts at {0}", sw.EllapsedMilliseconds),
                            sw => Debug.WriteLine("Submarine stops at {0}", sw.EllapsedMilliseconds),
                            (sw,msg) => Debug.WriteLine("{0} at {1}", msg, sw.EllapsedMilliseconds)
            );
            sp1.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);
            _accTime += dt;

            if (KeyHandler.Instance.isKeyPressed(Key.Right))
            {
                _subAccel += SubOptions.Instance.Game.SubmarineAcceleration;
                KeyHandler.Instance.clearKeyPresses();
                Debug.WriteLine("Speed : {0}", (_subSpeed + _subAccel));
                sp1.Step("Submarine accelerated");
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Space))
            {
                _subAccel = 500;
                KeyHandler.Instance.clearKeyPresses();
                sp1.Step("Submarine in booster mode");

            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Up))
            {
                Position = new Point(Position.X, Position.Y - SubOptions.Instance.Game.UnitSize);
                double fqTraining = SubOptions.Instance.User.FrequencyTraining;
                double fqDiff = SubOptions.Instance.User.FrequencyDelta;
                double dfpix = fqDiff / SubOptions.Instance.Game.GateSize;

                SubOptions.Instance.User.FrequencyComparison -= dfpix;
                SubOptions.Instance.UpdateDebug(); 
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.ChangeTargetFrequency(-dfpix);

                //Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency -= 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                KeyHandler.Instance.clearKeyPresses();
                Debug.WriteLine("position : {0}", Position.Y);
                sp1.Step("Submarine moved up");
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {
                Position = new Point(Position.X, Position.Y + SubOptions.Instance.Game.UnitSize);
                double fqTraining = SubOptions.Instance.User.FrequencyTraining;
                double fqDiff = SubOptions.Instance.User.FrequencyDelta;
                double dfpix = fqDiff / SubOptions.Instance.Game.GateSize;

                SubOptions.Instance.User.FrequencyComparison += dfpix;
                SubOptions.Instance.UpdateDebug();
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.ChangeTargetFrequency(dfpix);
                // Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency += 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                KeyHandler.Instance.clearKeyPresses();
                Debug.WriteLine("position : {0}", Position.Y);
                sp1.Step("Submarine moved down");
            }
            Position = new Point(Position.X + (this.Speed + this.Acceleration) * dt, Position.Y);
            

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
            sp1.Stop();

            // stop the auditory stimuli
            (IAppManager.Instance as SubmarineApplicationManager)._synthEx.Stop();

            // show the gate
            (IAppManager.Instance as SubmarineApplicationManager)._gate.Visibility = Visibility.Visible;
            
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
                    SubOptions.Instance.User.CurrentLife -= 1;

                    StateManager.Instance.setState(States.START_STATE);
                    (sender as DispatcherTimer).Stop();

                };

                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Start();

            }
            else if (other is GateObject || other is GateAnimatedObject)
            {
                (IAppManager.Instance as SubmarineApplicationManager)._gate.Visibility = Visibility.Visible;
                double tf = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTrainingFrequency();
                double cf = (IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTargetFrequency();

                /*Debug.WriteLine("Success : {0} - {1}", tf, cf);
                GameLevelDescriptor.CurrentGate--;
                if (GameLevelDescriptor.CurrentGate == 0)
                {
                    GameLevelDescriptor.CurrentLevel++;
                    GameLevelDescriptor.CurrentGate=5;
                    GameLevelDescriptor.ThresholdFrequency = (int)(GameLevelDescriptor.ThresholdFrequency * 0.90);
                }*/

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
