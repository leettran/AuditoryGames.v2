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

        /// <summary>
        /// 
        /// </summary>
        public enum Staircase
        {
            [Display(Name = "One-down, One-up", Description = "dfdffdfddf")]
            One_One,       ///< dfdfdf
            [Display(Name = "Two-down, One-up", Description = "dfdffdfddf")]
            Two_One,  ///<
            [Display(Name = "Three-down, One-up", Description = "dfdffdfddf")]
            Three_One,  ///<
        }

        [Display(Name = "Rule", Description = "The staircase adaptive rule to use (number of success to move 'down' and failure to move 'up'")]
        public Staircase StaircaseRule { get; set; }

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
            this.StaircaseRule = Staircase.Two_One;
        }

        public AuditoryModel Clone()
        {
            AuditoryModel tmp = new AuditoryModel();
            tmp.Base = this.Base;
            tmp.Step = this.Step;
            tmp.StaircaseRule = this.StaircaseRule;
            return tmp;
        }

        public void Copy(AuditoryModel tmp)
        {
            this.Base = tmp.Base;
            this.Step = tmp.Step;
            this.StaircaseRule = tmp.StaircaseRule;
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
