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

namespace AuditoryGames.GameFramework
{
    public class TreasureHunter : AnimatedGameObject
    {
        /// <summary>
        /// State description of the miner
        /// </summary>
        private static class MinerActionStates
        {
            public static string MINER_IDLE = "miner_idle"; ///< No action
            public static string MINER_MOVE = "miner_move"; ///< Move action started
            public static string MINER_GRAB = "miner_grab"; ///< Grab action started (i.e. )
        }

        protected const double SPEED = 200;
        protected const double TIME_BETWEEN_SHOTS = 0.25;
        protected double timeSinceLastShot = 0;

        private String _currState = MinerActionStates.MINER_IDLE;
        private Point _moveTo = new Point(0, 0);

        public int CurrentZone { get; set; }
        
        public TreasureHunter()
        {

        }

        public void startupPlayer(Point dimensions, AnimationData animationData, int zLayer)
        {
            base.startupAnimatedGameObject(dimensions, animationData, zLayer, false);
            this.collisionName = CollisionIdentifiers.PLAYER;
        }

        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            if (_currState == MinerActionStates.MINER_MOVE)
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
                    _currState = MinerActionStates.MINER_IDLE;
                    _moveTo = new Point(0, 0);
                    // ApplicationManager.Instance;

                    ApplicationManager.Instance.UpdateSound();
                }
            }

            if (ApplicationManager.PREVENT_AUDIO_CHANGES) return;

            timeSinceLastShot -= dt;
            if (KeyHandler.Instance.isKeyPressed(Key.Space) && timeSinceLastShot <= 0 && _currState == MinerActionStates.MINER_IDLE)
            {
                timeSinceLastShot = TIME_BETWEEN_SHOTS;
                Weapon weapon = Weapon.UnusedWeapon.startupPlayerBasicWeapon(ZLayers.PLAYER_Z);
                weapon.Position = new Point(Position.X + dimensions.X / 2 - weapon.Dimensions.X / 2, Position.Y + dimensions.Y - weapon.Dimensions.Y);
            }

            if (KeyHandler.Instance.isKeyPressed(Key.Left))
            {
                if (_currState == MinerActionStates.MINER_IDLE)
                {
                    this.CurrentZone--;
                    if (this.CurrentZone < 0)
                        this.CurrentZone = 0;
                    else
                    {
                        _moveTo = new Point(GameLevelInfo._sizeZones, -1);
                        _currState = MinerActionStates.MINER_MOVE;
                    }

                }
                
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Right))
            {
                if (_currState == MinerActionStates.MINER_IDLE)
                {
                    this.CurrentZone++;
                    if (this.CurrentZone > (GameLevelInfo._nbTreasureZones - 1))
                        this.CurrentZone = (GameLevelInfo._nbTreasureZones - 1);
                    else
                    {
                        _moveTo = new Point(GameLevelInfo._sizeZones, 1);
                        _currState = MinerActionStates.MINER_MOVE;
                    }

                }
              
            }

        }

        public override void shutdown()
        {
            base.shutdown();
        }

        public override void collision(GameObject other)
        {
            base.collision(other);
        }
    }
}
