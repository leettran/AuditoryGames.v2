using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LSRI.AuditoryGames.GameFramework
{
    /// <summary>
    /// A simple wrapper to Button, for changing icons.
    /// </summary>
    public partial class ButtonIcon : Button
    {


        public Image Icon
        {
            get
            {
                return _Icon;
         }
        }

        public TextBlock TextContent
        {
            get
            {
                return _TextContent;
         }
        }
        public ButtonIcon()
        {
            InitializeComponent();
        }
    }
}
