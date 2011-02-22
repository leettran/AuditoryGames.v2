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

namespace LSRI.AuditoryGames.GameFramework.Data
{
    public class GameOptions : UserModelEntity
    {
        private int _unit;
        private int _sizeGate;
        private int _subSpeed;
        private int _subAccel;
        private double _timeGate;
        private int _maxGates;
        private int _maxLives;

        [Display(Name = "Gates Number", Description = "Number of gates to cross (including final one) in each level")]
        [Range(1, 10)]
        public int MaxGates
        {
            get
            {
                return _maxGates;
            }
            set
            {
                if (_maxGates != value)
                {
                    _maxGates = value;
                    OnPropertyChanged("MaxGates");
                }
            }
        }

        [Display(Name = "Lives", Description = "Number of lives available at each level")]
        [Range(1, 10)]
        public int MaxLives
        {
            get
            {
                return _maxLives;
            }
            set
            {
                if (_maxLives != value)
                {
                    _maxLives = value;
                    OnPropertyChanged("MaxLives");
                }
            }
        }


        [Display(Name = "Unit Size", Description = "Size (in pixel) of the elementary game unit (for grid-based movement and location")]
        [Range(5,30)]
        public int UnitSize
        {
            get
            {
                return _unit;
            }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged("UnitSize");
                }
            }
        }

        [Display(Name = "Gate Range", Description = "Number of zones (in game unit) in both side of the gate")]
        [Range(1, 5)]
        public int GateSize
        {
            get
            {
                return _sizeGate;
            }
            set
            {
                if (_sizeGate != value)
                {
                    _sizeGate = value;
                    OnPropertyChanged("GateSize");
                }
            }
        }

        [Display(Name = "Submarine speed", Description = "Basic speed (in pixel per second) of the submarine")]
        [Range(25, 200)]
        public int SubmarineSpeed
        {
            get
            {
                return _subSpeed;
            }
            set
            {
                if (_subSpeed != value)
                {
                    _subSpeed = value;
                    double ff = 800.0 - 20.0 - 48.0;
                    double gg = _subSpeed;

                    TimeOnGate = ff/gg;
                    OnPropertyChanged("SubmarineSpeed");
                }
            }
        }

        [Display(Name = "Submarine Acceleration", Description = "Basic increment (in pixel per second) of submarine speed when accelerating")]
        [Range(25, 200)]
        public int SubmarineAcceleration
        {
            get
            {
                return _subAccel;
            }
            set
            {
                if (_subAccel != value)
                {
                    _subAccel = value;
                    OnPropertyChanged("SubmarineAcceleration");
                }
            }
        }

        [Display(Name = "Gate Max Time", Description = "Maximum time (ms) to get through each gate",Prompt="DDDDDD",ShortName="fddfdf")]
        [DisplayFormat(DataFormatString="{0:G")]
        [Editable(false)]
        public double TimeOnGate
        {
            get
            {
                return _timeGate;
            }
            set
            {
                if (_timeGate != value)
                {
                    _timeGate = value;
                    OnPropertyChanged("TimeOnGate");
                }
            }
        }


        public GameOptions()
        {
            this._unit = 15;
            this._sizeGate = 2;
            this._subSpeed = 50;
            this._subAccel = 50;
            this._maxGates = 5;
            this._maxLives = 4;

            this._timeGate = (800 - 20 - 48) / _subSpeed;
        }

        public GameOptions Clone()
        {
            GameOptions tmp = new GameOptions();
            tmp._unit = this._unit;
            tmp._sizeGate = this._sizeGate;
            tmp._subSpeed = this._subSpeed;
            tmp._subAccel = this._subAccel;
            tmp._maxGates = this._maxGates;
            tmp._maxLives = this._maxLives;
            return tmp;
        }

        public void Copy(GameOptions tmp)
        {
            this._unit = tmp._unit;
            this._sizeGate = tmp._sizeGate;
            this._subSpeed = tmp._subSpeed;
            this._subAccel = tmp._subAccel;
            this._maxGates = tmp._maxGates;
            this._maxLives = tmp._maxLives;
        }

        #region IEditableObject

        private GameOptions _tmpModel = null;

        override public void BeginEdit()
        {
            _tmpModel = this.Clone();
        }

        override public void CancelEdit()
        {
            this.Copy(_tmpModel);
        }

        override public void EndEdit()
        {
            _tmpModel = null;
        }
        #endregion

    }
}
