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
