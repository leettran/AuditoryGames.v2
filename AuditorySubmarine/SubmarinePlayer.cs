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

            /*sp1 = new StopwatchPlus(
                            sw => Debug.WriteLine("Submarine starts at {0}", sw.EllapsedMilliseconds),
                            sw => Debug.WriteLine("Submarine stops at {0}", sw.EllapsedMilliseconds),
                            (sw,msg) => Debug.WriteLine("{0} at {1}", msg, sw.EllapsedMilliseconds)
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

            bool hasMoved = false;

            Canvas zone = (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot;
            Point dim = new Point(zone.ActualWidth, zone.ActualHeight);
            
            int stepSize = SubOptions.Instance.Game.UnitSize;
            int nbUnitsInScreen = (int)dim.Y / stepSize;
            int screenMargin = ((int)dim.Y % stepSize) / 2;
            int gateExtent = SubOptions.Instance.Game.GateSize;
            int gateSize = 1 + 2 * SubOptions.Instance.Game.GateSize;

            int bias = 2;
      
            if (KeyHandler.Instance.isKeyPressed(Key.Right))
            {
                _subAccel += SubOptions.Instance.Game.SubmarineAcceleration;
                KeyHandler.Instance.clearKeyPresses();
                //Debug.WriteLine("Speed : {0}", (_subSpeed + _subAccel));
                //sp1.Step("Submarine accelerated");
                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine accelerated");

            }
            else if (KeyHandler.Instance.isKeyPressed(Key.S))
            {
                KeyHandler.Instance.clearKeyPresses();
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.SetSignalDelay(4);
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Space))
            {
                _subAccel = 500;
                KeyHandler.Instance.clearKeyPresses();
               (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine in booster mode");
                

            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Up))
            {
                KeyHandler.Instance.clearKeyPresses();
                hasMoved = true;
                CanvasIndex--;
               // Position = new Point(Position.X, Position.Y - SubOptions.Instance.Game.UnitSize);
              //  this.CanvasIndex++;

          /*      double fqTraining = SubOptions.Instance.User.FrequencyTraining;
                double fqDiff = SubOptions.Instance.User.FrequencyDelta;
                double dfpix = fqDiff / SubOptions.Instance.Game.GateSize;

                double tg = fqTraining - dfpix * CanvasIndex;//(IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTargetFrequency();
                //tg = tg - dfpix;

                if (tg >= SubOptions.Instance.Auditory.MaxFrequency) tg = SubOptions.Instance.Auditory.MaxFrequency;
                if (tg <= SubOptions.Instance.Auditory.MinFrequency) tg = SubOptions.Instance.Auditory.MinFrequency;

                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.SetTargetFrequency(tg,false);

                //SubOptions.Instance.User.FrequencyComparison -= dfpix;
                //SubOptions.Instance.UpdateDebug(); 
                //(IAppManager.Instance as SubmarineApplicationManager)._synthEx.ChangeTargetFrequency(-dfpix);

                //Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency -= 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                KeyHandler.Instance.clearKeyPresses();
                //Debug.WriteLine("position : {0}", Position.Y);
                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine moved up");
               
           * */
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {
                KeyHandler.Instance.clearKeyPresses();
                hasMoved = true;
                CanvasIndex++;
         /*      // Position = new Point(Position.X, Position.Y + SubOptions.Instance.Game.UnitSize);
             //   //this.CanvasIndex--;
                
                double fqTraining = SubOptions.Instance.User.FrequencyTraining;
                double fqDiff = SubOptions.Instance.User.FrequencyDelta;
                double dfpix = fqDiff / SubOptions.Instance.Game.GateSize;

                double tg = fqTraining - dfpix * CanvasIndex;//(IAppManager.Instance as SubmarineApplicationManager)._synthEx.GetTargetFrequency();
                //tg = tg + dfpix;

                if (tg >= SubOptions.Instance.Auditory.MaxFrequency) tg = SubOptions.Instance.Auditory.MaxFrequency;
                if (tg <= SubOptions.Instance.Auditory.MinFrequency) tg = SubOptions.Instance.Auditory.MinFrequency;

                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.SetTargetFrequency(tg, false);

                Debug.WriteLine("XXXXXXXXXXXXXXXX position : {0}", CanvasIndex);

                
                ///SubOptions.Instance.UpdateDebug();
                //(IAppManager.Instance as SubmarineApplicationManager)._synthEx.ChangeTargetFrequency(dfpix);
                // Note ss = (IApplicationManager.Instance as SubApplicationManager)._synthEx.Arpeggiator.Notes[2];
               // ss.Frequency += 50;
                //Note ss2 = _synthEx.Arpeggiator.Notes[1];
                //float gg = ss2.Frequency;
                //Debug.WriteLine("Frequency : {0}", ss.Frequency);
                
                //Debug.WriteLine("position : {0}", Position.Y);
                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine moved down");*/
            }

            if (hasMoved)
            {

                if (CanvasIndex <= bias) CanvasIndex = bias;
                if (CanvasIndex > nbUnitsInScreen - bias) CanvasIndex = nbUnitsInScreen - bias;
                Debug.WriteLine("XXXXXXXXXXXXXXXX position : {0}", CanvasIndex);
                double SubLoc = screenMargin + CanvasIndex * stepSize;
                SubLoc = SubLoc - Dimensions.Y / 2 + stepSize / 2;
                Position = new Point(Position.X,  SubLoc);


                double deltaf = SubOptions.Instance.User.FrequencyDelta;
                double fqTraining = SubOptions.Instance.User.FrequencyTraining;

                double deltapos = (IAppManager.Instance as SubmarineApplicationManager)._gate.CanvasIndex - CanvasIndex;
                //double deltaf = fqDiff;//  fqTraining * .2;
                double dfpix = deltaf / SubOptions.Instance.Game.GateSize;
                Debug.WriteLine("*********** deltaloc = {0}", deltapos);

                double fqComp = fqTraining - dfpix * deltapos;
                if (fqComp >= SubOptions.Instance.Auditory.MaxFrequency) fqComp = SubOptions.Instance.Auditory.MaxFrequency;
                if (fqComp <= SubOptions.Instance.Auditory.MinFrequency) fqComp = SubOptions.Instance.Auditory.MinFrequency;
                //SubOptions.Instance.User.FrequencyComparison = fqComp;
                (IAppManager.Instance as SubmarineApplicationManager)._synthEx.SetTargetFrequency(fqComp, false);

            }
            Position = new Point(Position.X + (this.Speed + this.Acceleration) * dt, Position.Y);
            

             // keep the player bound to the screen
       /*     if (Position.X > (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X)
                Position = new Point((AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X, Position.Y);
            else if (Position.X < 0)
                Position = new Point(0, Position.Y);
            if (Position.Y > (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - dimensions.Y)
                Position = new Point(Position.X, (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - dimensions.Y);
            else if (Position.Y < 0)
                Position = new Point(Position.X, 0);*/
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
            
            if (other is WallObject)
            {
                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step("Submarine collides with WALL");
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
                    SubOptions.Instance.User.CurrentLife--;
                    if (SubOptions.Instance.User.CurrentLife <= 0)
                    {
                        /// Failure to win level; make it easier and restart
                        SubOptions.Instance.User.CurrentGate = 0;
                        SubOptions.Instance.User.CurrentScore = 0;
                        SubOptions.Instance.User.CurrentLife = SubOptions.Instance.Game.MaxLives;
                        SubOptions.Instance.User.FrequencyDelta *= (1+SubOptions.Instance.Auditory.Step);
                        for (int i = 0; i < SubOptions.Instance.User.Gates.Data.Length; i++)
                        {
                            SubOptions.Instance.User.Gates.Data[i] *= (1 + SubOptions.Instance.Auditory.Step);
                            SubOptions.Instance.User.Gates.Data[i] = Math.Max(
                                SubOptions.Instance.Game.Gates.Data[i],
                                SubOptions.Instance.User.Gates.Data[i]);
                        }
                    }
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

                (IAppManager.Instance as SubmarineApplicationManager).Logger.Step(
                           String.Format("Submarine goes through gate : {0}", cf)
                           );

                /*Debug.WriteLine("Success : {0} - {1}", tf, cf);
                GameLevelDescriptor.CurrentGate--;
                if (GameLevelDescriptor.CurrentGate == 0)
                {
                    GameLevelDescriptor.CurrentLevel++;
                    GameLevelDescriptor.CurrentGate=5;
                    GameLevelDescriptor.ThresholdFrequency = (int)(GameLevelDescriptor.ThresholdFrequency * 0.90);
                }*/
                this.shutdown();

                SubOptions.Instance.User.CurrentGate++;

                double baseScore = 100;
                double deltapos = Math.Abs((IAppManager.Instance as SubmarineApplicationManager)._gate.CanvasIndex - CanvasIndex);
                double maxpos = SubOptions.Instance.Game.GateSize;

                double dartScore = baseScore * Math.Max(0, 1 - deltapos / (maxpos + 1));

                double sTime = (IAppManager.Instance as SubmarineApplicationManager).Logger.EllapsedMilliseconds;
                double mTime = SubOptions.Instance.Game.TimeOnGate * 1000;

                double timeScore = (sTime / mTime) * baseScore;


                SubOptions.Instance.User.CurrentScore += (int)(dartScore + timeScore);


                if (SubOptions.Instance.User.CurrentGate == SubOptions.Instance.Game.MaxGates)
                {
                    SubOptions.Instance.User.CurrentGate = 0;
                    SubOptions.Instance.User.CurrentLife = SubOptions.Instance.Game.MaxLives;
                    SubOptions.Instance.User.Scores.Data.Add(new HighScore()
                    {
                        Delta = SubOptions.Instance.User.FrequencyDelta,
                        Level = SubOptions.Instance.User.CurrentLevel,
                        Score = SubOptions.Instance.User.CurrentScore
                    });
                    SubOptions.Instance.User.FrequencyDelta *= (1 - SubOptions.Instance.Auditory.Step);
                    for (int i = 0; i < SubOptions.Instance.User.Gates.Data.Length; i++)
                    {
                        SubOptions.Instance.User.Gates.Data[i] *= (1 - SubOptions.Instance.Auditory.Step);
                        SubOptions.Instance.User.Gates.Data[i] = Math.Min(
                            SubOptions.Instance.Game.Gates.Data[i],
                            SubOptions.Instance.User.Gates.Data[i]);
                    }
                    SubOptions.Instance.User.CurrentScore = 0;
                    SubOptions.Instance.User.CurrentLevel++;

                }

                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += delegate(object sender, EventArgs e)
                {
                    StateManager.Instance.setState(States.START_STATE);
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
