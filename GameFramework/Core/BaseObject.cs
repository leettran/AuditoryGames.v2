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
    /// <summary>
    /// The base for all the game objects.
    /// 
    /// Defines the three basic entry points for all objects: startup, shutdown and update.
    /// </summary>
    abstract public class BaseObject
    {

        /// <summary>
        /// Indicates whether the object is in use (ie deployed in the game scene) or not.
        /// </summary>
        protected Boolean _inUse = false;

        /// <summary>
        /// TRUE if the object is already in use (ie deployed in the game scene), FALSE otherwise
        /// </summary>
        public Boolean InUse
        {
            get
            {
                return _inUse;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void startupBaseObject()
        {
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).enterFrame += new GamePage.EnterFrame(enterFrame);
            _inUse = true;
        }

        /// <summary>
        /// 
        /// </summary>
        virtual public void shutdown()
        {
            (LSRI.AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).enterFrame -= new GamePage.EnterFrame(enterFrame);
            _inUse = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">Time passed since the last call of the rendering loop (in ms)</param>
        virtual public void enterFrame(double dt)
        {
            
        }


    }
}
