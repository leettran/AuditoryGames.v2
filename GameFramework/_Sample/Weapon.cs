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
    public class Weapon : GameObject
    {
        protected const double SPEED = 200;
        protected static ResourcePool<Weapon> resourcePool = new ResourcePool<Weapon>();
        public delegate void WeaponLogic(double dt);
        protected WeaponLogic weaponLogic = null;

        static public Weapon UnusedWeapon
        {
            get
            {
                return resourcePool.UnusedObject;
            }
        }

        public Weapon()
        {

        }

        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);
            if (weaponLogic != null) weaponLogic(dt);
        }

        public Weapon startupPlayerBasicWeapon(int zLayer)
        {
            base.startupGameObject(new Point(17, 15), "Media/twobullets.png", zLayer);
            weaponLogic = new WeaponLogic(this.basicPlayerWeaponLogic);
            this._collisionName = CollisionIdentifiers.PLAYERWEAPON;
            return this;
        }

        public Weapon startupEnemyBasicWeapon(int zLayer)
        {
            base.startupGameObject(new Point(17, 15), "Media/twobullets.png", zLayer);
            weaponLogic = new WeaponLogic(this.basicEnemyWeaponLogic);
            this._collisionName = CollisionIdentifiers.ENEMYWEAPON;
            return this;
        }

        public override void shutdown()
        {
            base.shutdown();
            weaponLogic = null;
        }

        protected void basicPlayerWeaponLogic(double dt)
        {
            Position = new Point(Position.X, Position.Y - SPEED * dt);
            offscreenCheck();
        }

        protected void basicEnemyWeaponLogic(double dt)
        {
            Position = new Point(Position.X, Position.Y + SPEED * dt);
            offscreenCheck();
        }

        public override void collision(GameObject other)
        {
            base.collision(other);
            this.shutdown();
        }
    }
}
