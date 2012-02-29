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
using LSRI.AuditoryGames.GameFramework;

namespace LSRI.TreasureHunter
{
    /// <summary>
    /// Simple implementation of the Hunter's blasting tool
    /// </summary>
    public class HunterWeapon : GameObject
    {
        /// <summary>
        /// Default speed for the movement of the blast object (in pixel per second)
        /// </summary>
        private const double SPEED = 200;

        /// <summary>
        /// Reference to the resource pool containing used and unsued objects.
        /// </summary>
        protected static ResourcePool<HunterWeapon> resourcePool = new ResourcePool<HunterWeapon>();

        /// <summary>
        /// Delegate for the handling of the object's operation
        /// </summary>
        /// <param name="dt">Time passed since the last call of the rendering loop (in ms)</param>
        public delegate void WeaponLogic(double dt);

        /// <summary>
        /// Handler of the object's logic
        /// </summary>
        private WeaponLogic weaponLogic = null;

        /// <summary>
        /// Reference to the parent object generating this blast
        /// </summary>
        public GameObject _player = null;

        /// <summary>
        /// Static access to the pool of resources, for creating (or reusing) a new instance
        /// </summary>
        static public HunterWeapon UnusedWeapon
        {
            get
            {
                return resourcePool.UnusedObject;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public HunterWeapon()
        {

        }

        /// <summary>
        /// Expose the main entry point of the rendering loop of the game.
        /// </summary>
        /// <param name="dt">Time passed since the last call of the rendering loop (in ms)</param>
        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);
            if (weaponLogic != null) weaponLogic(dt);
        }

        /// <summary>
        /// Initialisation of the blasting tool
        /// </summary>
        /// <param name="parent">A reference to the GameObject generating the blast</param>
        /// <returns>A reference to the newly created blasting tool</returns>
        public HunterWeapon startupPlayerBasicWeapon(GameObject parent)
        {
            base.startupGameObject(new Point(17, 15), "Media/twobullets.png", ZLayers.PLAYER_Z);
            _player = parent;
            weaponLogic = new WeaponLogic(this.moveBlastObject);
            this._collisionName = CollisionIdentifiers.PLAYERWEAPON;
            return this;
        }

        /// <summary>
        /// Initialisation of the blasting tool
        /// </summary>
        /// <param name="zLayer">The depth of the graphic object in the page.</param>
        /// <returns>A reference to the newly created blasting tool</returns>
        /// @deprecated Reference to the parent object is needed; use the other constructor
        private HunterWeapon startupEnemyBasicWeapon(int zLayer)
        {
            base.startupGameObject(new Point(17, 15), "Media/twobullets.png", zLayer);
            weaponLogic = new WeaponLogic(this.moveBlastObject);
            this._collisionName = CollisionIdentifiers.ENEMYWEAPON;
            return this;
        }

        /// <summary>
        /// Called by the framework when the object is not needed anymore and needs to be destroyed
        /// </summary>
        public override void shutdown()
        {
            base.shutdown();
            weaponLogic = null;
        }

        /// <summary>
        /// Default behaviour of the blast tool, while in the game scene.
        /// </summary>
        /// <param name="dt">Time passed since the last call of the rendering loop (in ms)</param>
        protected void moveBlastObject(double dt)
        {
            Position = new Point(Position.X, Position.Y + SPEED * dt);
            offscreenCheck();
        }

        /// <summary>
        /// Handling of any collision between this object and others.
        /// 
        /// The blast tool can only collide with nuggets (TreasureNugget).
        /// Outcomes of the collision are handled by the nugget itself.
        /// </summary>
        /// <param name="other">A reference to the other object in the collision</param>
        /// @ see TreasureNugget.collision
        public override void collision(GameObject other)
        {
            base.collision(other);
            this.shutdown();
        }
    }
}
