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
using System.ComponentModel.DataAnnotations;
using LSRI.AuditoryGames.GameFramework.Data;

namespace LSRI.TreasureHunter.Model
{
    public class TreasureUser : UserModelEntity
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
        int _currLife;          ///< Current number of remaining lives
        int _currGold;          ///< Current number of remaining lives
        int _currScore;         ///< current score (at current level)
        UserType _userType;     ///< Type of the user
        Gates _currTimer;

        public int _currExposure; 
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

        [Display(Name = "Lives", Description = "Number of remaining lives to finish the current level")]
        [Range(1, 20)]
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

        [Display(Name = "Gold", Description = "Number of gold nuggests  already collected")]
        [Range(1, 20)]
        public int CurrentGold
        {
            get { return _currGold; }
            set
            {
                if (_currGold != value)
                {
                    _currGold = value;
                    OnPropertyChanged("CurrentGold");
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

        [Display(Name = "Success Pattern", Description = "The outcomes (success or failure) of the last three levels")]
        public WinPattern Pattern { set; get; }

        [Display(Name = "Visual Timer", Description = "Current score at the current level")]
        public Gates VisualTiming
        {
            get { return _currTimer; }
            set
            {
                if (_currTimer != value)
                {
                    _currTimer = value;
                    OnPropertyChanged("VisualTiming");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public TreasureUser()
        {
            this._userType = UserType.Stereotype;
            this._Name = "";
            this._FqTraining = 2500;
            this._FqDelta = 300;
            this._FqComparison = 0;
            this._currLevel = 1;
            this._currLife = 4;
            this._currGold = 0;
            this._currScore = 0;
            this._currExposure = 0;
            this._scores = new HighScoreContainer();
            this.Pattern = new WinPattern();
            this.VisualTiming = new Gates();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreasureUser Clone()
        {
            TreasureUser tmp = new TreasureUser();
            tmp._userType = this.Type;
            tmp._Name = this.Name;
            tmp._FqTraining = this.FrequencyTraining;
            tmp._FqDelta = this.FrequencyDelta;
            tmp._currLevel = this.CurrentLevel;
            tmp._currLife = this._currLife;
            tmp._currScore = this._currScore;
            tmp.Pattern = this.Pattern;
            tmp.Scores = this.Scores.Clone();
            tmp.VisualTiming = this.VisualTiming.Clone();

            return tmp;
        }

        public void Copy(TreasureUser tmp)
        {
            this._userType = tmp.Type;
            this._Name = tmp.Name;
            this._currLevel = tmp.CurrentLevel;
            this._currLife = tmp._currLife;
            this._FqTraining = tmp.FrequencyTraining;
            this._FqDelta = tmp.FrequencyDelta;
            this._currScore = tmp._currScore;
            this.Pattern = tmp.Pattern;
            this.Scores = tmp.Scores.Clone();
            this.VisualTiming = tmp.VisualTiming.Clone();
        }

        #region Stereotypes
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TreasureUser Beginner()
        {
            return new TreasureUser
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
        public static TreasureUser Expert()
        {
            var GG = new TreasureUser
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

        private TreasureUser _tmpModel = null;

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
}
