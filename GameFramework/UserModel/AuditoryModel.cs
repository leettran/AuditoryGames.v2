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

namespace LSRI.AuditoryGames.GameFramework.Data
{
    public class AuditoryModel : UserModelEntity
    {
        double _base;
        double _step;

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
