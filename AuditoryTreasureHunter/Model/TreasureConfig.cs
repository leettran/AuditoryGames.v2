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
using LSRI.AuditoryGames.GameFramework.Data;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using LSRI.AuditoryGames.GameFramework;

namespace LSRI.TreasureHunter.Model
{
    public class TreasureOptionsWrapper : IConfigurationManager
    {
        private static readonly string STORAGE_FILENAME = @"TreasureSettings.xml";

        /// <summary>
        /// 
        /// </summary>
        protected AuditoryModel _auditory = null;

        /// <summary>
        /// 
        /// </summary>
        protected TreasureGame _gOption = null;

        /// <summary>
        /// 
        /// </summary>
        protected TreasureUser _user = null;


        public TreasureOptionsWrapper()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A deep copy of the Submarine Game options</returns>
        public TreasureOptionsWrapper Clone()
        {
            TreasureOptionsWrapper tt = new TreasureOptionsWrapper();
            tt._auditory = this._auditory.Clone();
            tt._gOption = this._gOption.Clone();
            tt._user = this._user.Clone();
            return tt;

        }


        /// <summary>
        /// Access to the game configuration
        /// </summary>
        public TreasureGame Game
        {
            get
            {
                return _gOption;
            }
            set
            {
                _gOption = value;
            }
        }

        /// <summary>
        /// Access to the auditory system configuration
        /// </summary>
        public AuditoryModel Auditory
        {
            get
            {
                return _auditory;
            }
            set
            {
                _auditory = value;
            }
        }

        /// <summary>
        /// Access to the current user configuration
        /// </summary>
        public TreasureUser User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="temp"></param>
        public void Copy(TreasureOptionsWrapper temp)
        {
            if (temp == null) return;
            this._auditory = temp._auditory.Clone();
            this._gOption = temp._gOption.Clone();
            this._user = temp._user.Clone();
        }


        /// <summary>
        /// Save the configuration into isolated storage
        /// </summary>
        public void SaveConfiguration()
        {
            try
            {
                //var settings = IsolatedStorageSettings.ApplicationSettings;
                //IsolatedStorageSettings.ApplicationSettings["configuration"] = this.Clone();
                //IsolatedStorageSettings.ApplicationSettings["username"] = User.Name;
                //settings.Save();

                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    using (IsolatedStorageFileStream isoStream = store.OpenFile(TreasureOptionsWrapper.STORAGE_FILENAME, FileMode.Create))
                    {
                        XmlSerializer s = new XmlSerializer(typeof(TreasureOptionsWrapper));
                        TextWriter writer = new StreamWriter(isoStream);
                        s.Serialize(writer, this.Clone());
                        writer.Close();
                    }
                   /* store.CreateDirectory("submarine");

                    using (IsolatedStorageFileStream isoStream = store.OpenFile("submarine/test1.txt", FileMode.Create))
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(isoStream);
                        sw.WriteLine("####### Writing some text in the file.");
                        sw.Flush();
                        sw.Close();
                    }
                    using (IsolatedStorageFileStream isoStream = store.OpenFile("submarine/test2.txt", FileMode.Create))
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(isoStream);
                        sw.WriteLine("####### Writing some other text in the file.");
                        sw.Flush();
                        sw.Close();
                    }*/
                }

            }
            catch (Exception e)
            {
                //Debug.WriteLine("SERIALIZATION ERROR : " + e.Message);
            }
        }


        /// <summary>
        /// Load configuration from the isolated storage
        /// </summary>
        public void RetrieveConfiguration()
        {
            try
            {
                //var settings = IsolatedStorageSettings.ApplicationSettings;
                //SubOptionsWrapper um2 = null;
                //String name;
                //IsolatedStorageSettings.ApplicationSettings.TryGetValue("configuration", out um2);
                //IsolatedStorageSettings.ApplicationSettings.TryGetValue("username", out name);

                TreasureOptionsWrapper umXML = null;
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists(TreasureOptionsWrapper.STORAGE_FILENAME))
                    {
                        using (IsolatedStorageFileStream isoStream = store.OpenFile(TreasureOptionsWrapper.STORAGE_FILENAME, FileMode.Open))
                        {
                            XmlSerializer s = new XmlSerializer(typeof(TreasureOptionsWrapper));
                            TextReader writer = new StreamReader(isoStream);
                            umXML = s.Deserialize(writer) as TreasureOptionsWrapper;
                            writer.Close();
                        }
                    }
                }
                if (umXML != null)
                {
                    this.Copy(umXML);
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine("DE-SERIALIZATION ERROR : " + e.Message);
            }
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
                //_container = UserModelContainer.Default(),
                _auditory = new AuditoryModel(),
                _gOption = new TreasureGame(),
                _user = new TreasureUser
                {
                    Type = TreasureUser.UserType.User,
                    Name = "Current User",
                    Scores = HighScoreContainer.Default()
                }
            };
        }

        public static TreasureOptions Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        #region Debug Display Mode

         /// <summary>
        /// The GUI panel to display the debugging information
        /// </summary>
        private static TextBlock _debugUI = null;

        /// <summary>
        /// 
        /// </summary>
        public string Beat { set; get; }

        /// <summary>
        /// Add the debug information panel on the game page
        /// </summary>
        /// <param name="pg">A reference to the game's main page</param>
        public void AttachDebug(GamePage pg)
        {
            if (pg == null) return;
            if (_debugUI == null)
            {
                _debugUI = new TextBlock();
                _debugUI.Text = "### Debug information ###";
                _debugUI.Name = "txtbScore";
                _debugUI.Width = 100;
                _debugUI.Height = 35;
                _debugUI.FontSize = 9;
                _debugUI.FontFamily = new FontFamily("Courier New");
                _debugUI.Foreground = new SolidColorBrush(Colors.White);
                _debugUI.SetValue(Canvas.LeftProperty, 10.0);
                _debugUI.SetValue(Canvas.TopProperty, 10.0);
            }

            if (TreasureOptions.Instance.User.ShowDebug)
                _debugUI.Visibility = Visibility.Visible;
            else
                _debugUI.Visibility = Visibility.Collapsed;
 
            // we have to insert any non GameObjects at the end of the children collection
            pg.LayoutRoot.Children.Insert(pg.LayoutRoot.Children.Count, _debugUI);
        }

        public int nExposedX=0;
        public int nExposedY=0;

        /// <summary>
        /// Update the content of the debug panel
        /// </summary>
        public void UpdateDebug()
        {
            if (_debugUI == null) return;
            if (!TreasureOptions.Instance.User.ShowDebug) return;
            _debugUI.Text = String.Format(
                "Training Fq : {0} Hz\n" +
                "Delta       : {1} Hz\n" +
                "-----\n" +
                "Comparison  : {2} Hz\n" +
                "-----\n" +
                "Level       : {3}\n" +
                "Gold        : {4}/{5}\n" +
                "-----\n" +
                "Actions     : {10}\n" +
                "Exposure    : {6} - {7}/{8}\n" +
                "{9}",
                this.User.FrequencyTraining,
                this.User.FrequencyDelta,
                this.User.FrequencyComparison,
                this.User.CurrentLevel,
                this.User.CurrentGold,
                this.Game._curGold,
                this.User._currExposure,
                nExposedX,
                nExposedY,
                this.Beat,
                this.User.Actions
                );

        }
        #endregion

    }
}
