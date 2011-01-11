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
    /// Submarine application
    /// </summary>
    public class SubmarineApp : AuditoryGameApp
    {
        public SubmarineApp()
        {
            // set the running assembly name (for accessing resources in the proper DLL).
            string name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            ResourceHelper.ExecutingAssemblyName = name.Substring(0, name.IndexOf(','));
            SubmarineApplicationManager.Instance.GetType();
        }

    }
}
