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
using LSRI.AuditoryGames.GameFramework.Data;

namespace LSRI.TreasureHunter.Model
{
    public class TreasureOptionsWrapper : IConfigurationManager
    {
        private static readonly string STORAGE_FILENAME = @"TreasureSettings.xml";

        public TreasureOptionsWrapper()
        {
        }


        public void SaveConfiguration()
        {
            throw new NotImplementedException();
        }

        public void RetrieveConfiguration()
        {
            throw new NotImplementedException();
        }
    }


    public sealed class TreasureOptions : TreasureOptionsWrapper
    {
        /// <summary>
        /// private subclass to handle the 
        /// </summary>
        private class Nested
        {
            /// <summary>
            /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            /// </summary>
            static Nested() { }
            internal static readonly TreasureOptions instance = new TreasureOptions()
            {
            };
        }

        public static TreasureOptions Instance
        {
            get
            {
                return Nested.instance;
            }
        }
    }
}
