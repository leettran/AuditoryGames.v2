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
    /*
        The base for all game objects 
    */
    abstract public class BaseObject
    {
        protected bool inUse = false;

        public bool InUse
        {
            get
            {
                return inUse;
            }
        }
        
        public void startupBaseObject()
        {
            (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).enterFrame += new GamePage.EnterFrame(enterFrame);
            inUse = true;
        }

        virtual public void shutdown()
        {
            (AuditoryGames.GameFramework.AuditoryGameApp.Current.RootVisual as GamePage).enterFrame -= new GamePage.EnterFrame(enterFrame);
            inUse = false;
        }

        virtual public void enterFrame(double dt)
        {
            
        }

        virtual public Visibility Visibility { get; set; }

    }
}
