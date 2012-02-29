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
    public class Player : AnimatedGameObject
    {
        protected const double SPEED = 200;
        protected const double TIME_BETWEEN_SHOTS = 0.25;
        protected double timeSinceLastShot = 0;
        
        public Player()
        {

        }

        public void startupPlayer(Point dimensions, AnimationData animationData, int zLayer)
        {
            base.startupAnimatedGameObject(dimensions, animationData, zLayer, false);
            this._collisionName = CollisionIdentifiers.PLAYER;
        }

        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            timeSinceLastShot -= dt;
            if (KeyHandler.Instance.isKeyPressed(Key.Space) && timeSinceLastShot <= 0)
            {
                timeSinceLastShot = TIME_BETWEEN_SHOTS;
                Weapon weapon = Weapon.UnusedWeapon.startupPlayerBasicWeapon(ZLayers.PLAYER_Z);
                weapon.Position = new Point(Position.X + dimensions.X / 2 - weapon.Dimensions.X / 2, Position.Y - weapon.Dimensions.Y);
            }

            if (KeyHandler.Instance.isKeyPressed(Key.Up))
            {
                Position = new Point(Position.X, Position.Y - SPEED * dt);
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Down))
            {
                Position = new Point(Position.X, Position.Y + SPEED * dt);
            }

            if (KeyHandler.Instance.isKeyPressed(Key.Left))
            {
                Position = new Point(Position.X - SPEED * dt, Position.Y);
            }
            else if (KeyHandler.Instance.isKeyPressed(Key.Right))
            {
                Position = new Point(Position.X + SPEED * dt, Position.Y);
            }

            // keep the player bound to the screen
            if (Position.X > (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X)
                Position = new Point((LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualWidth - dimensions.X, Position.Y);
            else if (Position.X < 0)
                Position = new Point(0, Position.Y);
            if (Position.Y > (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - dimensions.Y)
                Position = new Point(Position.X, (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.ActualHeight - dimensions.Y);
            else if (Position.Y < 0)
                Position = new Point(Position.X, 0);
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
