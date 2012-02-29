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
