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

        [Display(Name = "Gate Size", Description = "Size (in game unit) of the gate")]
        [Range(1, 11)]
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


        public GameOptions()
        {
            this._unit = 15;
            this._sizeGate = 5;
        }

        public GameOptions Clone()
        {
            GameOptions tmp = new GameOptions();
            tmp._unit = this._unit;
            return tmp;
        }

        public void Copy(GameOptions tmp)
        {
            this._unit = tmp._unit;
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
