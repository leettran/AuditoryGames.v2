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

namespace LSRI.Submarine
{
    /// <summary>
    /// Submarine application.
    /// 
    /// Two operations need to be done at that level:
    /// - initialise the ResourceHelper with the name of the current assembly (by default, it will be the GameFramework assembly)
    /// - call the SubmarineApplicationManager to make sure the proper instance is created.
    /// </summary>
    public class SubmarineApp : AuditoryGameApp
    {
        public SubmarineApp()
        {
            // set the running assembly name (for accessing resources in the Submarinre assembly, not the Framework library).
            string name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            ResourceHelper.ExecutingAssemblyName = name.Substring(0, name.IndexOf(','));

            // Force the Submarine Application Manager to be initialised
            SubmarineApplicationManager.Instance.GetType();
        }

    }
}
