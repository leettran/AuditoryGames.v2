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

namespace AuditoryGames.GameFramework
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
            this.collisionName = CollisionIdentifiers.PLAYERWEAPON;
            return this;
        }

        public Weapon startupEnemyBasicWeapon(int zLayer)
        {
            base.startupGameObject(new Point(17, 15), "Media/twobullets.png", zLayer);
            weaponLogic = new WeaponLogic(this.basicEnemyWeaponLogic);
            this.collisionName = CollisionIdentifiers.ENEMYWEAPON;
            return this;
        }

        public override void shutdown()
        {
            base.shutdown();
            weaponLogic = null;
        }

        protected void basicPlayerWeaponLogic(double dt)
        {
            Position = new Point(Position.X, Position.Y + SPEED * dt);
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
