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
using System.Collections.Generic;
using LSRI.AuditoryGames.GameFramework;
using LSRI.TreasureHunter.Model;

namespace LSRI.TreasureHunter
{
    public class HunterPlayer : AnimatedGameObject
    {
        /// <summary>
        /// State of operation of the TreasureHunter player object
        /// </summary>
        private static class HunterActionStates
        {
            public static string ISRESTING = "hunter_resting";      ///< The TreasureHunter player is currenlty not doing anything.
            public static string ISMOVING = "hunter_moving";        ///< The TreasureHunter player is currenlty moving to a different location.
            public static string ISBLASTING = "hunter_blasting";    ///< The TreasureHunter player is currenlty blasting a nugget out.
        }

        /// <summary>
        /// Default speed for the movement of the player object (in pixel per second)
        /// </summary>
        private const double SPEED = 200;

        protected const double TIME_BETWEEN_SHOTS = 0.25;
        protected double timeSinceLastShot = 0;

        /// <summary>
        /// Current state of operation of the TreasureHunter player object
        /// </summary>
        private String _currState = HunterActionStates.ISRESTING;

        /// <summary>
        /// Destination (in screen coordinates) of the player object when in movement.
        /// </summary>
        private Point _moveTo = new Point(0, 0);

        /// <summary>
        /// Contains the index of the ground zone where the player object is currently located.
        /// </summary>
        public int CurrentZone { get; set; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public HunterPlayer()
        {

        }

        /// <summary>
        /// Initialisation of the TreasureHunter player object.
        /// </summary>
        /// <param name="dimensions">Size (pixel) of the image</param>
        /// <param name="animationData"></param>
        /// <param name="zLayer"></param>
        public void startupPlayer(Point dimensions, AnimationData animationData, int zLayer)
        {
            base.startupAnimatedGameObject(dimensions, animationData, zLayer, false);
            this._collisionName = CollisionIdentifiers.PLAYER;
        }

        /// <summary>
        /// Expose the main entry point of the rendering loop of the game.
        /// </summary>
        /// <param name="dt">Time passed since the last call of the rendering loop (in ms)</param>
        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            /// if the player object is in movement, shift its position accordingly until it reaches its destination
            if (_currState == HunterActionStates.ISMOVING)
            {
                if (_moveTo.X > 0)
                {
                    Point pt = new Point(SPEED * dt, Position.Y);
                    pt.X = Math.Min(_moveTo.X,pt.X);
                    Position = new Point(Position.X + pt.X * _moveTo.Y, Position.Y);
                    _moveTo.X = _moveTo.X - pt.X;
                }
                else
                {
                    _currState = HunterActionStates.ISRESTING;
                    _moveTo = new Point(0, 0);
                    // TreasureApplicationManager.Instance;

                    (TreasureApplicationManager.Instance as TreasureApplicationManager).changeExposure();
                    (TreasureApplicationManager.Instance as TreasureApplicationManager).UpdateSound(-1);
                }
            }

            if (!StateManager.Instance.CurrentState.Equals(TreasureStates.LEVEL_STATE)) return;
            if (TreasureApplicationManager.PREVENT_AUDIO_CHANGES)
            {
                KeyHandler.Instance.clearKeyPresses();
                return;
            }

            //timeSinceLastShot -= dt;
            if (KeyHandler.Instance.isKeyPressed(Key.Space) && timeSinceLastShot <= 0 && _currState == HunterActionStates.ISRESTING)
            {
                TreasureApplicationManager.PREVENT_AUDIO_CHANGES = true;
                TreasureOptions.Instance.User.Actions++;
                TreasureOptions.Instance.User.CurrentLife--;
                (TreasureApplicationManager.Instance as TreasureApplicationManager)._scorePanel.Life = TreasureOptions.Instance.User.CurrentLife;
                timeSinceLastShot = TIME_BETWEEN_SHOTS;
                HunterWeapon weapon = HunterWeapon.UnusedWeapon.startupPlayerBasicWeapon(this);
                weapon.Position = new Point(Position.X + dimensions.X / 2 - weapon.Dimensions.X / 2, Position.Y + dimensions.Y - weapon.Dimensions.Y);
            }

            else if (KeyHandler.Instance.isKeyPressed(Key.Left))
            {
                if (_currState == HunterActionStates.ISRESTING)
                {
                    this.CurrentZone--;
                    TreasureOptions.Instance.User.Actions++;
                    if (this.CurrentZone < 0)
                        this.CurrentZone = 0;
                    else
                    {
                        _moveTo = new Point(TreasureOptions.Instance.Game._sizeZones, -1);
                        _currState = HunterActionStates.ISMOVING;
                        (TreasureApplicationManager.Instance as TreasureApplicationManager)._synthEx.Stop();
                    }

                }
                
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Right))
            {
                if (_currState == HunterActionStates.ISRESTING)
                {
                    this.CurrentZone++;
                    TreasureOptions.Instance.User.Actions++;
                    if (this.CurrentZone > (TreasureOptions.Instance.Game.Zones - 1))
                        this.CurrentZone = (TreasureOptions.Instance.Game.Zones - 1);
                    else
                    {
                        _moveTo = new Point(TreasureOptions.Instance.Game._sizeZones, 1);
                        _currState = HunterActionStates.ISMOVING;
                        (TreasureApplicationManager.Instance as TreasureApplicationManager)._synthEx.Stop();
                    }

                }
              
            }

            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {
                TreasureOptions.Instance.User.Actions++;
                (TreasureApplicationManager.Instance as TreasureApplicationManager).UpdateSound(-1);
            }

        }

        public override void shutdown()
        {
            base.shutdown();
        }

        public override void collision(GameObject other)
        {
            //base.collision(other);
            timeSinceLastShot = 0;
        }
    }
}
