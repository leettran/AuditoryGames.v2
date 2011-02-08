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


        abstract public void BeginEdit();

        abstract public void CancelEdit();

        abstract public void EndEdit();
    }

    public class Gates : UserModelEntity
    {
        private double[] _name;
 
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

    public class UserModel : UserModelEntity
    {
        string _Name;
        double _FqTraining;
        double _FqComparison;
        Gates _gate;

        //string[] _tt;
        ObservableCollection<string> _hh;

         /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Name", GroupName = "Identification", Description = "Name of user")]
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
        [Display(Name = "Training", GroupName = "Frequency", Description = "Phone number of the form (###) ###-####")]
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
        [Display(Name = "Comparison", GroupName = "Frequency", Description = "Phone number of the form (###) ###-####")]
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


        /*
        public string[] GatesOpening
        {
            get { return _tt; }
            set
            {
                if (_tt != value)
                {
                    _tt = value;
                    OnPropertyChanged("GatesOpening");
                }
            }
        }

        */
        public ObservableCollection<string> GatesOpening2
        {
            get { return _hh; }
            set
            {
                if (_hh != value)
                {
                    _hh = value;
                    OnPropertyChanged("GatesOpening2");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UserModel()
        {
            this.Name = "";
            this.Gates = new Gates();
            this.FrequencyTraining = 5000;
            this.FrequencyComparison = 3000;
            //this.GatesOpening = new String[]{"a","b","c"};
            this.GatesOpening2 = new ObservableCollection<string> { "sddf" };
            //this._name = new string[] { "0", "1", "2", "3", "4" };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserModel Clone()
        {
            UserModel tmp = new UserModel();
            tmp.Name = this.Name;
            tmp.FrequencyTraining = this.FrequencyComparison;
            tmp.FrequencyComparison = this.FrequencyComparison;
            tmp.Gates = this.Gates;
            tmp.GatesOpening2 = new ObservableCollection<string>();
            foreach (string s in this.GatesOpening2)
            {
                string tt = "" + s;
                tmp.GatesOpening2.Add(tt);
            }

            return tmp;
        }

        public void Copy(UserModel tmp)
        {
            this.Name = tmp.Name;
            this.FrequencyTraining = tmp.FrequencyComparison;
            this.FrequencyComparison = tmp.FrequencyComparison;
            this.Gates = tmp.Gates;
            this.GatesOpening2 = new ObservableCollection<string>();
            while (tmp.GatesOpening2.Count != 0)
            {
                this.GatesOpening2.Add(tmp.GatesOpening2[0]);
                tmp.GatesOpening2.RemoveAt(0);
            }
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
            var GG =  new UserModel
            {
                Name = "Expert",
                FrequencyComparison = 4500
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
