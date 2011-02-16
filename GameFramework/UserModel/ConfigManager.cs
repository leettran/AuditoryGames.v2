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
    public abstract class IConfigurationManager
    {
        protected UserModelContainer _container = new UserModelContainer();
        protected AuditoryModel _auditory = new AuditoryModel();

        public static IConfigurationManager Instance { get; set; }

        public void Save()
        {
            
        }

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

        public ObservableCollection<UserModel> UserLists
        {
            get
            {
                return _container.UserModels;
            }
        }

        public UserModel CurrentModel
        {
            get
            {
                return _container.CurrentModel;
            }
        }
    }
}
