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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace LSRI.AuditoryGames.GameFramework.Data
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class UserModelObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }

    public class UserModel : UserModelObject
    {
        string _Name;
        double _FqTraining;
        double _FqComparison;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public double FrequencyTraining
        {
            get { return _FqTraining; }
            set
            {
                if (_FqTraining != value)
                {
                    _FqTraining = value;
                    OnPropertyChanged("FrequencyTraining");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double FrequencyComparison
        {
            get { return _FqComparison; }
            set
            {
                if (_FqComparison != value)
                {
                    _FqComparison = value;
                    OnPropertyChanged("FrequencyComparison");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UserModel()
        {
            this.Name = "";
            this.FrequencyTraining = 5000;
            this.FrequencyComparison = 3000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserModel Beginner()
        {
            return new UserModel
            {
                Name = "Beginner",
                FrequencyComparison = 3000
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserModel Expert()
        {
            return new UserModel
            {
                Name = "Expert",
                FrequencyComparison = 4500
            };
        }


    }

    public class UserModelContainer : UserModelObject
    {
        ObservableCollection<UserModel> _usermodels;
        UserModel _currentmodel;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<UserModel> UserModels
        {
            get { return _usermodels; }
            set
            {
                if (_usermodels != value)
                {
                    _usermodels = value;
                    OnPropertyChanged("UserModels");
                }
            }
        }

        public UserModel CurrentModel
        {
            get { return _currentmodel; }
            set
            {
                if (_currentmodel != value)
                {
                    _currentmodel = value;
                    OnPropertyChanged("CurrentModel");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UserModelContainer()
        {
            UserModels = new ObservableCollection<UserModel>
            {
                UserModel.Beginner(),
                UserModel.Expert(),
                new UserModel{
                    Name = "Current User"
                }

            };
            CurrentModel = UserModels[0];
        }
    }

}
