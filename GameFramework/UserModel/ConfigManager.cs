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
using System.Collections.ObjectModel;

namespace LSRI.AuditoryGames.GameFramework.Data
{
    /// <summary>
    /// TEST INTERFACE
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// 
        /// </summary>
        void SaveConfiguration();

        /// <summary>
        /// 
        /// </summary>
        void RetrieveConfiguration();
    }
}
