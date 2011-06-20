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
using System.Text;

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

    public abstract class GameLogger
    {
        //private static readonly string STORAGE_FILENAME = @"logger.xml";
        //private static readonly string STORAGE_CSVFILENAME = @"logger.csv";

        protected static readonly string LOG_GAMESTARTED = "GAME_STARTED";
        protected static readonly string LOG_GAMEENDED = "GAME_ENDED";
        protected static readonly string LOG_LEVELSTARTED = "LEVEL_STARTED";
        protected static readonly string LOG_LEVELENDED = "LEVEL_ENDED";

        //private ObservableCollection<GameEvent> _events;

        /*public ObservableCollection<GameEvent> Events
        {
            get
            {
                return _events;
            }
            set
            {
                _events = value;
            }

        }*/

        public GameLogger()
        {
            //this._events = new ObservableCollection<GameEvent>();
        }

        public abstract String getStorageName();
   //     {
    //        return STORAGE_CSVFILENAME;
   //     }

        /*/// <summary>
        /// 
        /// </summary>
        /// <returns>A deep copy of the Submarine Game options</returns>
        public GameLogger Clone()
        {
            GameLogger tt = new GameLogger();
            foreach (GameEvent rec in this._events)
            {
                tt._events.Add(rec.Clone());
            }
            return tt;
        }*/

        public static bool IncreaseStorageQuota(int nQuota)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                long iQuotaMax = isoStore.AvailableFreeSpace;
                long iQuotaCur = isoStore.Quota;
                long iQuotaNew = nQuota * 1024 * 1024;
                if (iQuotaNew > iQuotaCur)
                {
                    if (isoStore.IncreaseQuotaTo(iQuotaNew) == true)
                    {
                        // The user clicked Yes to the
                        // host's prompt to approve the quota increase.
                        return true;
                    }
                    else
                    {
                        // The user clicked No to the
                        // host's prompt to approve the quota increase.
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
         }

        protected void WriteLogFile(DateTime now,string message, string stackTrace)
        {

            try
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForSite())
                {

                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(this.getStorageName(), FileMode.Append, isoStore))
                    {

                        using (StreamWriter writer = new StreamWriter(isoStream))
                        {

                            StringBuilder sb = new StringBuilder();

                            sb.Append(now.ToString("dd/MM/yyyy,HH:mm:ss.ffff"));
                            sb.Append(",");
                            sb.Append(message);
                            sb.Append(",");
                            sb.AppendLine(stackTrace);

                            writer.Write(sb.ToString());
                            writer.Close();

                        }

                    }
                }

            }
            catch (Exception e)
            {
                
               throw e;
            }


        }

        public abstract void logGameStarted();
      //  {
      //      WriteLogFile(DateTime.Now,GameLogger.LOG_GAMESTARTED, "");
      //  }

        public abstract void logGameEnded();
     //   {
     //       WriteLogFile(DateTime.Now,GameLogger.LOG_GAMEENDED, "");
     //   }

        public abstract void logLevelStarted();
    //    {
    //        WriteLogFile(DateTime.Now,GameLogger.LOG_LEVELSTARTED, "");
    //    }

        public abstract void logLevelEnded(int win);
     //   {
    //        WriteLogFile(DateTime.Now,GameLogger.LOG_LEVELENDED, "");
     //   }

        /*/// <summary>
        /// Save the configuration into isolated storage
        /// </summary>
        public void SaveLog()
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream isoStream = store.OpenFile(GameLogger.STORAGE_FILENAME, FileMode.Create))
                    {
                        XmlSerializer s = new XmlSerializer(typeof(GameLogger));
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

        public GameLogger GetCurrentLog()
        {
            GameLogger umXML = null;
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(GameLogger.STORAGE_FILENAME))
                    {
                        using (IsolatedStorageFileStream isoStream = store.OpenFile(GameLogger.STORAGE_FILENAME, FileMode.Open))
                        {
                            XmlSerializer s = new XmlSerializer(typeof(GameLogger));
                            TextReader writer = new StreamReader(isoStream);
                            umXML = s.Deserialize(writer) as GameLogger;
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
        }*/
    }
}
