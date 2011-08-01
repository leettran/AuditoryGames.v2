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
        protected virtual void OnPropertyChanged(string propName)
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
        /// <summary>
        /// 
        /// </summary>
        private List<bool> _name;

        /// <summary>
        /// 
        /// </summary>
         
        [Display(Name = "Level -1", Description = "Success of last level")]
        public bool Level1 { 
            set; 
            get; 
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Level -2", Description = "Success of last-but-1 level")]
        public bool Level2 { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Level -3", Description = "Success of last-but-two level")]
        public bool Level3 { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public WinPattern()
        {
            this.Level1 = this.Level2 = this.Level3 = true;
            _name = new List<bool>();
        }

        #region IEditableObject Gates
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
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class Gates : UserModelEntity
    {
        /// <summary>
        /// 
        /// </summary>
        private double[] _name;

        /// <summary>
        /// 
        /// </summary>
        [Display(AutoGenerateField = false)]
        public double[] Data
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }

        }


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

        public Gates Clone()
        {
            Gates temp = new Gates();
            temp.Data = new double[this.Data.Length];
            for (int i=0;i<this.Data.Length;i++)
            {
                temp.Data[i] = this.Data[i];
            }
            return temp;
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
            User,       ///< Indicate the current (real) user
            Stereotype  ///< Indicate a stereotypical user (for debuggin and testing purpose)
        }

        string  _Name;          ///< Name of the user
        double  _FqTraining;    ///< Training frequency of the user
        double _FqDelta;        ///< Frequency Difference limen (theoretical) of the user
        int _currLevel;         ///< Current level of the game played by the user
        int _currGate;          ///< Current gate crossed by the user
        int _currLife;          ///< Current number of remaining lives
        int _currScore;         ///< current score (at current level)
        UserType _userType;     ///< Type of the user
        Gates   _gate;          ///<  ddd
        bool _showDebug;
                                
        HighScoreContainer _scores;


        double _FqComparison;   ///< Current  comparison frequency played


        [Display(AutoGenerateField = false)]
        public HighScoreContainer Scores
        {
            get
            {
                return _scores;
            }
            set
            {
                _scores = value;
            }

        }


        [Display(Name = "Type", Description = "Either a real or a Stereotypical user")]
        [ReadOnly(true)] 
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
        public double FrequencyDelta
        {
            get { return _FqDelta; }
            set
            {
                if (_FqDelta != value)
                {
                    _FqDelta = value;
                    OnPropertyChanged("FrequencyDelta");
                }
            }
        }


        [Display(AutoGenerateField = false)]
        public double FrequencyComparison
        {

            get { return _FqComparison; }
            set
            {
                if (_FqComparison != value)
                {
                    _FqComparison = value;
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Current Level", Description = "Current maximum level reached in the game")]
        [Range(1, 1000)]
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
        [Display(Name = "Current Gate", Description = "Number of gates crossed during the current level")]
        [Range(0, 4)]
        public int CurrentGate
        {
            get { return _currGate; }
            set
            {
                if (_currGate != value)
                {
                    _currGate = value;
                    OnPropertyChanged("CurrentGate");
                }
            }
        }

        [Display(Name = "Lives", Description = "Number of remaining lives to finish the current level")]
        [Range(1, 4)]
        public int CurrentLife
        {
            get { return _currLife; }
            set
            {
                if (_currLife != value)
                {
                    _currLife = value;
                    OnPropertyChanged("CurrentLife");
                }
            }
        }

        [Display(Name = "Score", Description = "Current score at the current level")]
        [ReadOnly(true)]
        public int CurrentScore
        {
            get { return _currScore; }
            set
            {
                if (_currScore != value)
                {
                    _currScore = value;
                    OnPropertyChanged("CurrentScore");
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
                    OnPropertyChanged("Gates");
                }
            }
        }

        [Display(Name = "Success Pattern", Description = "The outcomes (success or failure) of the last three levels")]
        public WinPattern Pattern { set; get; }

        [Display(Name = "Show Debug", Description = "Display the in-game debug information")]
        public bool ShowDebug
        {
            get { return _showDebug; }
            set
            {
                if (_showDebug != value)
                {
                    _showDebug = value;
                    OnPropertyChanged("ShowDebug");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UserModel()
        {
            this._userType = UserType.Stereotype;
            this._Name = "";
            this._FqTraining = 2500;
            this._FqDelta = 0;
            this._FqComparison = 0;
            this._currLevel = 1;
            this._currGate = 0;
            this._currLife = 4;
            this._currScore = 0;
            this._scores = new HighScoreContainer();
            this.Gates = new Gates();
            this.Pattern = new WinPattern();
            this._showDebug = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserModel Clone()
        {
            UserModel tmp = new UserModel();
            tmp._userType = this.Type;
            tmp._Name = this.Name;
            tmp._FqTraining = this.FrequencyTraining;
            tmp._FqDelta = this.FrequencyDelta;
            tmp._currLevel = this.CurrentLevel;
            tmp._currGate = this._currGate;
            tmp._currLife = this._currLife;
            tmp._showDebug = this._showDebug;
            tmp._currScore = this._currScore;
            tmp.Gates = this.Gates;
            tmp.Pattern = this.Pattern;
            tmp.Scores = this.Scores.Clone();

            return tmp;
        }

        public void Copy(UserModel tmp)
        {
            this._userType = tmp.Type;
            this._Name = tmp.Name;
            this._currLevel = tmp.CurrentLevel;
            this._currGate = tmp._currGate;
            this._currLife = tmp._currLife;
            this._FqTraining = tmp.FrequencyTraining;
            this._FqDelta = tmp.FrequencyDelta;
            this._currScore = tmp._currScore;
            this._showDebug = tmp._showDebug;
            this.Gates = tmp.Gates;
            this.Pattern = tmp.Pattern;
            this.Scores = tmp.Scores.Clone();
        }


        public bool IsGateVisible(double posRatio)
        {
            double vis = -1;
            if (this.CurrentGate < Gates.Data.Length)
                vis = Gates.Data[this.CurrentGate];
            return posRatio < vis;
        }

        #region Stereotypes
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserModel Beginner()
        {
            return new UserModel
            {
                Name = "Beginner",
                FrequencyDelta = 3000,
                CurrentLevel = 1,
                Scores = HighScoreContainer.Default()
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
                FrequencyDelta = 4500,
                CurrentLevel = 10,
                 Scores = HighScoreContainer.Default()
           };
            return GG;
        }

        #endregion

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

    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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
            UserModels = new ObservableCollection<UserModel>();
            /*{
                 new UserModel{
                    Type = UserModel.UserType.User,
                    Name = "Current User"
                },
               UserModel.Beginner(),
                UserModel.Expert()

            };*/
            CurrentModel = null;// UserModels[0];
        }


        public static UserModelContainer Default()
        {
            UserModelContainer temp = new UserModelContainer();
            temp.UserModels.Add(new UserModel
            {
                Type = UserModel.UserType.User,
                Name = "Current User"
            });
            temp.CurrentModel = temp.UserModels[0];
            //temp.UserModels.Add(UserModel.Beginner());
           // temp.UserModels.Add();
            return temp;

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
