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
using System.ComponentModel.DataAnnotations;

namespace LSRI.AuditoryGames.GameFramework.Data
{

    public class AuditoryModel : UserModelEntity
    {
        private double _base;
        private double _step;
        private bool _keypress;
        private Staircase _rule;

        /// <summary>
        /// 
        /// </summary>
        public enum Staircase
        {
            One_One,        ///< dfdfdf
            Two_One,        ///<
            Three_One       ///<
        }

        [Display(Name = "Rule", Description = "The adaptive staircase rule to use (number of success to move 'down' and failure to move 'up'")]
        public Staircase StaircaseRule
        {
            get { return _rule; }
            set
            {
                if (_rule != value)
                {
                    _rule = value;
                    OnPropertyChanged("Rule");
                }
            }
        }

        [Display(Name = "Base", Description = "The initial value for the frequency delta (in percent of the training frequency)")]
        public double Base
        {
            get { return _base; }
            set
            {
                if (_base != value)
                {
                    _base = value;
                    OnPropertyChanged("Base");
                }
            }
        }

        [Display(Name = "Step", Description = "The subsequent change in frequency delta (substract/add, depending on success/failure)")]
        public double Step
        {
            get { return _step; }
            set
            {
                if (_step != value)
                {
                    _step = value;
                    OnPropertyChanged("Step");
                }
            }
        }

        public bool KeyPressed
        {
            get
            {
                return _keypress;
            }
            set
            {
                if (_keypress != value)
                {
                    _keypress = value;
                    OnPropertyChanged("KeyPressed");
                }
            }
        }

        public AuditoryModel()
        {
            this._base = .50;
            this._step = .04;
            this._keypress = true;
            this._rule = Staircase.One_One;
        }

        public AuditoryModel Clone()
        {
            AuditoryModel tmp = new AuditoryModel();
            tmp._keypress = this._keypress;
            tmp._rule = this._rule;
            tmp._base = this._base;
            tmp._step = this._step;
            return tmp;
        }

        public void Copy(AuditoryModel tmp)
        {
            if (tmp == null) return;
            this._keypress = tmp._keypress;
            this._base = tmp._base;
            this._step = tmp._step;
            this._rule = tmp._rule;
        }

        #region IEditableObject

        private AuditoryModel _tmpModel = null;

        override public void BeginEdit()
        {
            _tmpModel = this.Clone();
        }

        override public void CancelEdit()
        {
            this.Copy(_tmpModel);
            _tmpModel = null;
        }

        override public void EndEdit()
        {
            _tmpModel = null;
        }
        #endregion

    }

}
