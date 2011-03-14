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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace LSRI.TreasureHunter.Model
{
    public class TreasureGame : UserModelEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum DisplayMode
        {
            None,         
            Position,     
            Content    
        };

        /// <summary>
        /// 
        /// </summary>
        public enum OpacityMode
        {
            None,          
            ByZone,         
            ByDepth,       
            Both          
        };

        /// <summary>
        /// 
        /// </summary>
        public enum DetectionMode
        {
            Proximity,
            Value,
            Distance
        };

        public enum FogOfWar
        {
            None,
            ByLocation,
            ByContent
        };

        private int _nbZones;
        private int _nbDepth;
        private bool _isMaxGold;
        private int _MaxGold;
        private int _nbCharges;
        private DetectionMode _mdDetect;
        private DisplayMode _mdDisplay;
        private OpacityMode _mdOpacity;


        public int _sizeZones;
        public string _curSetup;
        public int _curGold;

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Zones", Description = "Number of zones to explore")]
        [Range(5, 30)]
        public int Zones
        {
            get
            {
                return _nbZones;
            }
            set
            {
                if (_nbZones != value)
                {
                    _nbZones = value;
                    OnPropertyChanged("Zones");
                }
            }
        }

        [Display(Name = "Depth", Description = "Number of zones to explore")]
        [Range(1, 10)]
        public int Depth
        {
            get
            {
                return _nbDepth;
            }
            set
            {
                if (_nbDepth != value)
                {
                    _nbDepth = value;
                    OnPropertyChanged("Depth");
                }
            }
        }

        [Display(Name = "Charges", Description = "Number of digging charges the user will have")]
        [Range(1, 20)]
        public int Charges
        {
            get
            {
                return _nbCharges;
            }
            set
            {
                if (_nbCharges != value)
                {
                    _nbCharges = value;
                    OnPropertyChanged("Charges");
                }
            }
        }


        public class MyValidators
        {
            public static ValidationResult CheckGold(int gold)
            {
                bool isValid = (gold >= 1) && (gold <= (TreasureOptions.Instance.Game.Zones * 2 / 3));

                // Perform validation logic here and set isValid to true or false.

                if (isValid)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(
                        String.Format(
                        "The field Gold must be between 0 and {0}", TreasureOptions.Instance.Game.Zones * 2 / 3));
                }
            }

        }

        [Display(Name = "Gold", Description = "Number of Gold nugget to extract")]
        //[Range(1, 10)]
        [CustomValidation(typeof(MyValidators), "CheckGold")]
        public int Gold
        {
            get
            {
                return _MaxGold;
            }
            set
            {
                if (_MaxGold != value)
                {
                    _MaxGold = value;
                    OnPropertyChanged("Gold");
                }
            }
        }

        [Display(Name = "Maximize Gold", Description = "The number of nuggets is maximised, i.e. 2/3 of the number of zones",AutoGenerateField = true)]
        public bool MaxGold
        {
            get
            {
                return _isMaxGold;
            }
            set
            {
                if (_isMaxGold != value)
                {
                    _isMaxGold = value;
                    OnPropertyChanged("MaxGold");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Opacity mode", Description = "Opacity of ground will happen by zone, by depth, both or not at all.")]
        public OpacityMode Opacity
        {
            get
            {
                return _mdOpacity;
            }
            set
            {
                if (_mdOpacity != value)
                {
                    _mdOpacity = value;
                    OnPropertyChanged("Opacity");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Display mode", Description = "Nuggets will be displayed by content (gold or rubbish), " +
            "by location (placeholder) or not at all")]
        public DisplayMode Display
        {
            get
            {
                return _mdDisplay;
            }
            set
            {
                if (_mdDisplay != value)
                {
                    _mdDisplay = value;
                    OnPropertyChanged("Display");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Detection Mode", Description = "Gold will be detected by proximity (only the surrounding zones), " +
            "by distance (the side with the nearest nugget) or by value (the side with the biggest amount of nuggets)")]
        public DetectionMode Detection
        {
            get
            {
                return _mdDetect;
            }
            set
            {
                if (_mdDetect != value)
                {
                    _mdDetect = value;
                    OnPropertyChanged("DetectionMode");
                }
            }
        }





        /// <summary>
        /// 
        /// </summary>
        public TreasureGame()
        {
            this._mdDetect = DetectionMode.Proximity;
            this._mdDisplay = DisplayMode.Content;
            this._mdOpacity = OpacityMode.Both;
            this._nbZones = 10;
            this._nbDepth = 10;
            this._isMaxGold = true;
            this._MaxGold = 5;
            this._nbCharges = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreasureGame Clone()
        {
            TreasureGame tmp = new TreasureGame();
            tmp._nbZones = this._nbZones;
            tmp._nbDepth = this._nbDepth;
            tmp._nbCharges = this._nbCharges;
            tmp._isMaxGold = this._isMaxGold;
            tmp._MaxGold = this._MaxGold;
            tmp._mdDetect = this._mdDetect;
            tmp._mdDisplay = this._mdDisplay;
            tmp._mdOpacity = this._mdOpacity;

            return tmp;
        }

        public void Copy(TreasureGame tmp)
        {
            if (tmp == null) return;
            this._nbZones = tmp._nbZones;
            this._nbDepth = tmp._nbDepth;
            this._nbCharges = tmp._nbCharges;
            this._isMaxGold = tmp._isMaxGold;
            this._MaxGold = tmp._MaxGold;
            this._mdDetect = tmp._mdDetect;
            this._mdDisplay = tmp._mdDisplay;
            this._mdOpacity = tmp._mdOpacity;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<String> GetLevelDescriptors()
        {
            List<String> setup = new List<String>();
            this._curGold = 0;
            if (this.MaxGold) 
                this._curGold = this.Zones - (this.Zones / 3);
            else 
                this._curGold = this.Gold;

            GenerateDescriptions("", this._curGold, this.Zones, setup);
            return setup;
        }


        /// <summary>
        /// Recursively compute the list of possible configuration of a level by placing the specified amound of gold
        /// in the specified amount of zones, such as a maximum of 2 gold nuggets are side-by-side.
        /// If a descriptor is valid, it is added to the list.
        /// </summary>
        /// <param name="curr">the current level descriptor to compute</param>
        /// <param name="gold">the number of gold nugget to place</param>
        /// <param name="size">the number of zone to use</param>
        /// <param name="holder">the list of valid descriptors computed so far</param>
        private void GenerateDescriptions(string curr, int gold, int size, List<String> holder)
        {
            if (gold == 0)
            {
                StringBuilder sb1 = new StringBuilder(curr);
                for (int i = 0; i < size; i++) sb1.Append('0');
                holder.Add(sb1.ToString());
                return;
            }
            else if (size == 0) return;
            else if (gold > size) return;
            else if (curr.Length <= 1)
            {
                //String ff = curr + '1';
                StringBuilder sb1 = new StringBuilder(curr);
                sb1.Append('1');
                StringBuilder sb2 = new StringBuilder(curr);
                sb2.Append('0');
                GenerateDescriptions(sb1.ToString(), gold - 1, size - 1, holder);
                GenerateDescriptions(sb2.ToString(), gold, size - 1, holder);
            }
            else
            {
                if ((curr[curr.Length - 1] == '1' && curr[curr.Length - 2] == '1'))
                {
                    //String ff = curr + '0';
                    StringBuilder sb1 = new StringBuilder(curr);
                    sb1.Append('0');
                    GenerateDescriptions(sb1.ToString(), gold, size - 1, holder);
                }
                else
                {
                    //tring ff = curr + '1';
                    StringBuilder sb1 = new StringBuilder(curr);
                    sb1.Append('1');
                    StringBuilder sb2 = new StringBuilder(curr);
                    sb2.Append('0');
                    GenerateDescriptions(sb1.ToString(), gold - 1, size - 1, holder);
                    GenerateDescriptions(sb2.ToString(), gold, size - 1, holder);
                }
            }

        }



        #region IEditableObject

        private TreasureGame _tmpModel = null;

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
