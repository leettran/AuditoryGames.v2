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
        private Staircase _rule;
        private double _base;
        private double _step;
        private double _minFq;
        private double _maxFq;

        private int _nBufferSize;
        private double _lBeat;
        private int _lStim;
        private int _lIntStim;
        private int _lIntSignal;


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

        [Display(Name = "Frequency Min", GroupName = "Frequency", Description = "The mininum frequency to be played.")]
        [Required]
        public double MinFrequency
        {
            get { return _minFq; }
            set
            {
                if (_minFq != value)
                {
                    _minFq = value;
                    OnPropertyChanged("MinFrequency");
                }
            }
        }

        [Display(Name = "Frequency Max", GroupName = "Frequency", Description = "The maximum frequency to be played.")]
        [Required]
        public double MaxFrequency
        {
            get { return _maxFq; }
            set
            {
                if (_maxFq != value)
                {
                    _maxFq = value;
                    OnPropertyChanged("MaxFrequency");
                }
            }
        }

        [Display(Name = "Beat length", GroupName = "Stimuli", Description = "Duration (ms) of the elementary sound beat")]
        [Range(25,1000)]
        public double DurationBeat
        {
            get { return _lBeat; }
            set
            {
                if (_lBeat != value)
                {
                    _lBeat = value;
                    OnPropertyChanged("DurationBeat");
                }
            }
        }


        [Display(Name = "Stimuli duration", GroupName = "Stimuli", Description = "Duration (in beats) of the stimuli")]
        [Range(1,100)]
        public int DurationStimuli
        {
            get { return _lStim; }
            set
            {
                if (_lStim != value)
                {
                    _lStim = value;
                    OnPropertyChanged("DurationStimuli");
                }
            }
        }

        [Display(Name = "Inter-Stimuli duration", GroupName = "Stimuli", Description = "Duration (maximum) of the silence between stimuli")]
        [Range(1, 100)]
        public int DurationInterStimuli
        {
            get { return _lIntStim; }
            set
            {
                if (_lIntStim != value)
                {
                    _lIntStim = value;
                    OnPropertyChanged("DurationInterStimuli");
                }
            }
        }

        [Display(Name = "Inter-Signals duration", GroupName = "Stimuli", Description = "Duration (maximum) of the silence between each pair of stimuli")]
        [Range(1, 100)]
        public int DurationInterSignal
        {
            get { return _lIntSignal; }
            set
            {
                if (_lIntSignal != value)
                {
                    _lIntSignal = value;
                    OnPropertyChanged("DurationInterSignal");
                }
            }
        }

        [Display(Name = "Audio Buffer Length", GroupName = "Synthesiser", Description = "Length of the audio buffer (in ms); results in latency or clicks")]
        [Range(15,1000)]
        public int BufferLength
        {
            get { return _nBufferSize; }
            set
            {
                if (_nBufferSize != value)
                {
                    _nBufferSize = value;
                    OnPropertyChanged("BufferLength");
                }
            }
        }

        public AuditoryModel()
        {
            this._base = .50;
            this._step = .04;
            this._minFq = 500;
            this._maxFq = 5000;
            this._lBeat = 25.0;
            this._lStim = 8;
            this._lIntStim = 10;
            this._lIntSignal = 20;
            this._nBufferSize = 500;
            this._rule = Staircase.One_One;
        }

        public AuditoryModel Clone()
        {
            AuditoryModel tmp = new AuditoryModel();
            tmp._rule = this._rule;
            tmp._base = this._base;
            tmp._step = this._step;
            tmp._minFq = this._minFq;
            tmp._maxFq = this._maxFq;
            tmp._lBeat = this._lBeat;
            tmp._lStim = this._lStim;
            tmp._lIntStim = this._lIntStim;
            tmp._lIntSignal = this._lIntSignal;
            tmp._nBufferSize = this._nBufferSize;
            return tmp;
        }

        public void Copy(AuditoryModel tmp)
        {
            if (tmp == null) return;
            this._base = tmp._base;
            this._step = tmp._step;
            this._rule = tmp._rule;
            this._minFq = tmp._minFq;
            this._maxFq = tmp._maxFq;

            this._lBeat = tmp._lBeat;
            this._lStim = tmp._lStim;
            this._lIntStim = tmp._lIntStim;
            this._lIntSignal = tmp._lIntSignal;
            this._nBufferSize = tmp._nBufferSize;
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
