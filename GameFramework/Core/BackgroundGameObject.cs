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
    public class BackgroundGameObject : GameObject
    {
        protected const double SPEED = 35;
        protected static ResourcePool<BackgroundGameObject> resourcePool = new ResourcePool<BackgroundGameObject>();

        static public BackgroundGameObject UnusedBackgroundGameObject
        {
            get
            {
                return resourcePool.UnusedObject;
            }
        }

        public BackgroundGameObject()
        {

        }

        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);
            Position = new Point(Position.X, Position.Y + SPEED * dt);
            offscreenCheck();
        }

        public BackgroundGameObject startupBackgroundGameObject(Point dimensions, string image, int zLayer)
        {
            base.startupGameObject(dimensions, image, zLayer);
            return this;
        }

        public override void shutdown()
        {
            base.shutdown();
        }
    }
}
