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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.IO;
using System.Windows.Media.Imaging;

namespace LSRI.AuditoryGames.GameFramework
{
    public class ResourceHelper
    {
        protected static string strExecutingAssemblyName = null;

        public static string ExecutingAssemblyName
        {
            get
            {
                if (strExecutingAssemblyName == null)
                {
                    string name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                    return name.Substring(0, name.IndexOf(','));
                }
                else
                {
                    return strExecutingAssemblyName;
                }
            }
            set
            {
                strExecutingAssemblyName = value;
            }
        }

        public static Stream GetStream(string relativeUri, string assemblyName)
        {
            StreamResourceInfo res = Application.GetResourceStream(new Uri(assemblyName + ";component/" + relativeUri, UriKind.Relative));
            if (res == null)
            {
                res = Application.GetResourceStream(new Uri(relativeUri, UriKind.Relative));
            }
            if (res != null)
            {
                return res.Stream;
            }
            else
            {
                return null;
            }
        }

        public static Stream GetStream(string relativeUri)
        {
            return GetStream(relativeUri, ExecutingAssemblyName);
        }

        public static BitmapImage GetBitmap(string relativeUri, string assemblyName)
        {
            Stream s = GetStream(relativeUri, assemblyName);
            if (s == null) return null;
            using (s)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(s);
                return bmp;
            }
        }

        public static BitmapImage GetBitmap(string relativeUri)
        {
            return GetBitmap(relativeUri, ExecutingAssemblyName);
        }

        public static string GetString(string relativeUri, string assemblyName)
        {
            Stream s = GetStream(relativeUri, assemblyName);
            if (s == null) return null;
            using (StreamReader reader = new StreamReader(s))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetString(string relativeUri)
        {
            return GetString(relativeUri, ExecutingAssemblyName);
        }

        public static FontSource GetFontSource(string relativeUri, string assemblyName)
        {
            Stream s = GetStream(relativeUri, assemblyName);
            if (s == null) return null;
            using (s)
            {
                return new FontSource(s);
            }
        }

        public static FontSource GetFontSource(string relativeUri)
        {
            return GetFontSource(relativeUri, ExecutingAssemblyName);
        }

        public static object GetXamlObject(string relativeUri, string assemblyName)
        {
            string str = GetString(relativeUri, assemblyName);
            if (str == null) return null;
            object obj = System.Windows.Markup.XamlReader.Load(str);
            return obj;
        }

        public static object GetXamlObject(string relativeUri)
        {
            return GetXamlObject(relativeUri, ExecutingAssemblyName);
        }

    }
}
