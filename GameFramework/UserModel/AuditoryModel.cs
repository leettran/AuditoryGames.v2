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
        double _base;
        double _step;
        Staircase _rule;

        /// <summary>
        /// 
        /// </summary>
        public enum Staircase
        {
            One_One,       ///< dfdfdf
            Two_One,  ///<
            Three_One  ///<
        }

        [Display(Name = "Rule", Description = "The staircase adaptive rule to use (number of success to move 'down' and failure to move 'up'")]
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

        public AuditoryModel()
        {
            this.Base = .50;
            this.Step = .04;
            this._rule = Staircase.Two_One;
        }

        public AuditoryModel Clone()
        {
            AuditoryModel tmp = new AuditoryModel();
            tmp.Base = this.Base;
            tmp.Step = this.Step;
            tmp._rule = this._rule;
            return tmp;
        }

        public void Copy(AuditoryModel tmp)
        {
            this.Base = tmp.Base;
            this.Step = tmp.Step;
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
        }

        override public void EndEdit()
        {
            _tmpModel = null;
        }
        #endregion

    }
}
