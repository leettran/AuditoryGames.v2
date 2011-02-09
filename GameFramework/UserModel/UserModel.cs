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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSRI.AuditoryGames.GameFramework.Data
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public abstract class UserModelEntity : INotifyPropertyChanged, IEditableObject
    {
        #region INotifyPropertyChanged UserModelEntity

        /// <summary>
        /// interface
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
        #endregion

        #region IEditableObject UserModelEntity

        /// <summary>
        /// 
        /// </summary>
        abstract public void BeginEdit();

        /// <summary>
        /// 
        /// </summary>
        abstract public void CancelEdit();

        /// <summary>
        /// 
        /// </summary>
        abstract public void EndEdit();

        #endregion
    }


    public class WinPattern : UserModelEntity
    {
        public bool Level1 { set; get; }
        public bool Level2 { set; get; }
        public bool Level3 { set; get; }

        public WinPattern()
        {
            this.Level1 = this.Level2 = this.Level3 = true;
        }

        public override void BeginEdit()
        {
            //throw new NotImplementedException();
        }

        public override void CancelEdit()
        {
           // throw new NotImplementedException();
        }

        public override void EndEdit()
        {
            //throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Gates : UserModelEntity
    {
        private double[] _name;
 
        /// <summary>
        /// 
        /// </summary>
        [Range(0.0,1.0)]
        public double Gate1
        {
            get { return _name[0]; }
            set
            {
                if (_name[0] != value)
                {
                    _name[0] = value;
                    OnPropertyChanged("Gate1");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Range(0.0, 1.0)]
        public double Gate2
        {
            get { return _name[1]; }
            set
            {
                if (_name[1] != value)
                {
                    _name[1] = value;
                    OnPropertyChanged("Gate1");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Range(0.0, 1.0)]
        public double Gate3
        {
            get { return _name[2]; }
            set
            {
                if (_name[2] != value)
                {
                    _name[3] = value;
                    OnPropertyChanged("Gate1");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Range(0.0, 1.0)]
        public double Gate4
        {
            get { return _name[3]; }
            set
            {
                if (_name[3] != value)
                {
                    _name[3] = value;
                    OnPropertyChanged("Gate1");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Range(0.0, 1.0)]
        public double Gate5
        {
            get { return _name[4]; }
            set
            {
                if (_name[4] != value)
                {
                    _name[4] = value;
                    OnPropertyChanged("Gate1");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Gates()
        {
            _name = new double[] { 
                .9,
                .5,
                .25,
                .10,
                0
            };
        }

        #region IEditableObject Gates

        private double[] _temp;
        override public void BeginEdit()
        {
            //throw new NotImplementedException();
            _temp = new double[this._name.Length];
            _temp = this._name.Clone() as double[];

        }

        override public void CancelEdit()
        {
            //throw new NotImplementedException();
            this._name = _temp.Clone() as double[];
        }

        override public void EndEdit()
        {
            //throw new NotImplementedException();
            this._temp = null;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserModel : UserModelEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum UserType
        {
            User,       ///< dfdfdf
            Stereotype   ///<
        }

        string  _Name;
        double  _FqTraining;
        double  _FqComparison; 
        int     _currLevel;
        UserType _userType;
        Gates   _gate;

        //string[] _tt;
        //ObservableCollection<string> _hh;

        public UserType Type
        {
            get { return _userType; }
            set
            {
                if (_userType != value)
                {
                    _userType = value;
                    OnPropertyChanged("Type");
                }
            }
        }
         /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Name", Description = "Name/identifier of user")]
        [Required]
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
        [Display(Name = "Training", GroupName = "Frequency", Description = "Training frequency of the user")]
        [Required]
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
        [Display(Name = "Delta (Fq)", GroupName = "Frequency", Description = "Limit of theoretical audible frequency difference")]
        [Required]
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
        [Display(Name = "Current Level", Description = "Current maximum level reached in the game")]
        [Range(1,1000)]
        public int CurrentLevel
        {
            get { return _currLevel; }
            set
            {
                if (_currLevel != value)
                {
                    _currLevel = value;
                    OnPropertyChanged("CurrentLevel");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Gates", Description = "Timing of appearence of each gate")]
        public Gates Gates
        {
            get { return _gate; }
            set
            {
                if (_gate != value)
                {
                    _gate = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public WinPattern Pattern { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public UserModel()
        {
            this._userType = UserType.Stereotype;
            this.Name = "";
            this.Gates = new Gates();
            this.Pattern = new WinPattern();
            this.FrequencyTraining = 5000;
            this.FrequencyComparison = 3000;
            this.CurrentLevel = 1;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserModel Clone()
        {
            UserModel tmp = new UserModel();
            tmp._userType = this.Type;
            tmp.Name = this.Name;
            tmp.CurrentLevel = this.CurrentLevel;
            tmp.FrequencyTraining = this.FrequencyComparison;
            tmp.FrequencyComparison = this.FrequencyComparison;
            tmp.Gates = this.Gates;

            return tmp;
        }

        public void Copy(UserModel tmp)
        {
            this._userType = tmp.Type; 
            this.Name = tmp.Name;
            this.CurrentLevel = tmp.CurrentLevel;
            this.FrequencyTraining = tmp.FrequencyComparison;
            this.FrequencyComparison = tmp.FrequencyComparison;
            this.Gates = tmp.Gates;
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
                FrequencyComparison = 3000,
                CurrentLevel = 1
           };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserModel Expert()
        {
            var GG =  new UserModel
            {
                Name = "Expert",
                FrequencyComparison = 4500,
                CurrentLevel = 10
            };
            return GG;
        }


        #region IEditableObject

        private UserModel _tmpModel = null;

        override public void BeginEdit()
        {
            //throw new NotImplementedException();
            _tmpModel = this.Clone();
        }

        override public void CancelEdit()
        {
           // throw new NotImplementedException();
            Copy(_tmpModel);
        }

        override public void EndEdit()
        {
            //throw new NotImplementedException();
            _tmpModel = null;
        }
        #endregion
    }

    public class UserModelContainer : UserModelEntity
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
                    Type = UserModel.UserType.User,
                    Name = "Current User"
                }

            };
            CurrentModel = UserModels[0];
        }

        #region IEditableObject
        override public void BeginEdit()
        {
        }

        override public void CancelEdit()
        {
        }

        override public void EndEdit()
        {
        }
        #endregion
    }

}
