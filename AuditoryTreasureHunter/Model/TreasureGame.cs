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

namespace LSRI.TreasureHunter.Model
{
    public class TreasureGame : UserModelEntity
    {
        private int _nbZones;
        private int _nbDepth;


        [Display(Name = "Zones - Initial Number", Description = "Number of zones to explore")]
        [Range(5, 20)]
        public int InitZones
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
                    OnPropertyChanged("InitZones");
                }
            }
        }

        [Display(Name = "Depths - Initial Number", Description = "Number of zones to explore")]
        [Range(5, 20)]
        public int InitZones
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
                    OnPropertyChanged("InitZones");
                }
            }
        }


        public override void BeginEdit()
        {
            throw new NotImplementedException();
        }

        public override void CancelEdit()
        {
            throw new NotImplementedException();
        }

        public override void EndEdit()
        {
            throw new NotImplementedException();
        }
    }
}
