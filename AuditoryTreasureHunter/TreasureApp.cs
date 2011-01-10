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
using AuditoryGames.GameFramework;

namespace AuditoryGames.TreasureHunter
{
    public class TreasureApp : AuditoryGameApp
    {
        public TreasureApp()
        {
            // set the running assembly name (for accessing resources in the proper DLL).
            string name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            ResourceHelper.ExecutingAssemblyName = name.Substring(0, name.IndexOf(','));
            TreasureApplicationManager.Instance.GetType();
        }


    }
}
