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

        [Display(Name = "Key Down Mode", Description = "Indicates whether the game works on KeyDown or KeyUp basis")]
        public bool KeyPressed { get; set; }

        public GameOptions()
        {
            this.KeyPressed = true;
        }

        public GameOptions Clone()
        {
            GameOptions tmp = new GameOptions();
            tmp.KeyPressed = this.KeyPressed;
            return tmp;
        }

        public void Copy(GameOptions tmp)
        {
            this.KeyPressed = tmp.KeyPressed;
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
