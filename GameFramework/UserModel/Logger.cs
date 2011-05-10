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
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using LSRI.AuditoryGames.Utils;

namespace LSRI.AuditoryGames.GameFramework.Data
{
    public class GameEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public enum EventType
        {
            UNKNOWN,            ///< blah blah
            GAME_START,         ///< blah blah
            GAME_STOP           ///< blah blah
        }

        public DateTime TimeStamp { get; set; }
        public EventType Type { get; set; }
        public SerializableDictionary<string, object> CustomProperties { get; set; }
        //public List<KeyValuePair<string, string>> XProperties { get; set; }


        public GameEvent()
        {
            this.TimeStamp = DateTime.Now;
            this.Type = EventType.UNKNOWN;
            this.CustomProperties = new SerializableDictionary<string, object>();
            //this.XProperties = new List<KeyValuePair<string, string>>();
        }


        public virtual GameEvent Clone()
        {
            GameEvent tt = new GameEvent();
            tt.TimeStamp = this.TimeStamp;
            tt.Type = this.Type;
            tt.CustomProperties = new SerializableDictionary<string, object>();
            foreach (var ff in this.CustomProperties)
            {
                tt.CustomProperties.Add(ff.Key, ff.Value);
            }
            //tt.XProperties = new List<KeyValuePair<string, string>>(this.XProperties);
            return tt;
        }
    }

    public class Logger
    {
        private static readonly string STORAGE_FILENAME = @"logger.xml";

        private ObservableCollection<GameEvent> _events;

        public ObservableCollection<GameEvent> Events
        {
            get
            {
                return _events;
            }
            set
            {
                _events = value;
            }

        }

        public Logger()
        {
            this._events = new ObservableCollection<GameEvent>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A deep copy of the Submarine Game options</returns>
        public Logger Clone()
        {
            Logger tt = new Logger();
            foreach (GameEvent rec in this._events)
            {
                tt._events.Add(rec.Clone());
            }
            return tt;
        }



        /// <summary>
        /// Save the configuration into isolated storage
        /// </summary>
        public void SaveLog()
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream isoStream = store.OpenFile(Logger.STORAGE_FILENAME, FileMode.Create))
                    {
                        XmlSerializer s = new XmlSerializer(typeof(Logger));
                        TextWriter writer = new StreamWriter(isoStream);
                        s.Serialize(writer, this.Clone());
                        writer.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("SERIALIZATION ERROR : " + e.Message);
            }
        }

        public Logger GetCurrentLog()
        {
            Logger umXML = null;
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(Logger.STORAGE_FILENAME))
                    {
                        using (IsolatedStorageFileStream isoStream = store.OpenFile(Logger.STORAGE_FILENAME, FileMode.Open))
                        {
                            XmlSerializer s = new XmlSerializer(typeof(Logger));
                            TextReader writer = new StreamReader(isoStream);
                            umXML = s.Deserialize(writer) as Logger;
                            writer.Close();
                        }
                    }
                }
                if (umXML != null)
                {
                    //this.Copy(umXML);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("DE-SERIALIZATION ERROR : " + e.Message);
            }
            return umXML;
        }
    }
}
