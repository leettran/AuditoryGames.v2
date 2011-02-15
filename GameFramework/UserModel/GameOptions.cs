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
        private bool _keypress;

        [Display(Name = "Key Down Mode", Description = "Indicates whether the game works on KeyDown or KeyUp basis")]
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

        public GameOptions()
        {
            this._keypress = true;
        }

        public GameOptions Clone()
        {
            GameOptions tmp = new GameOptions();
            tmp._keypress = this._keypress;
            return tmp;
        }

        public void Copy(GameOptions tmp)
        {
            this._keypress = tmp._keypress;
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
