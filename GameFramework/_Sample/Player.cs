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
            this.collisionName = CollisionIdentifiers.PLAYER;
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
            if (Position.X > (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualWidth - dimensions.X)
                Position = new Point((AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualWidth - dimensions.X, Position.Y);
            else if (Position.X < 0)
                Position = new Point(0, Position.Y);
            if (Position.Y > (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualHeight - dimensions.Y)
                Position = new Point(Position.X, (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.ActualHeight - dimensions.Y);
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
