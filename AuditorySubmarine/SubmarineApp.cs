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
