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


namespace TestGame
{
    public class TestApp : AuditoryGameApp
    {

        public TestApp()
        {
            // set the running assembly name (for accessing resources in the proper DLL).
            string name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            ResourceHelper.ExecutingAssemblyName = name.Substring(0, name.IndexOf(','));
            TestApplicationManager.Instance.GetType();
        }


      /*  override protected void Application_Startup(object sender, StartupEventArgs e)
        {
            TestApplicationManager.Instance.GetType();

            this.RootVisual = new GamePage();
            KeyHandler.Instance.startupKeyHandler(this.RootVisual as GamePage);
            IAppManager.Instance.startupApplicationManager();
            StateManager.Instance.startupStateManager();

        }*/

    }

}