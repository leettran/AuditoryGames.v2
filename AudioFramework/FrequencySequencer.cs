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
using Kindohm.KSynth.Library;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AudioFramework
{
    public class SequencerExt : Sequencer
    {
        public delegate void StepStarted();
        public delegate void StepChanged();
        public delegate void StepEnded();

        public event StepStarted _stepStartedHook = null;
        public event StepChanged _stepChangedHook = null;
        public event StepEnded _stepEndedHook = null;

        override protected void ProcessCurrentStep()
        {
            if (this.stepIndex == 0)
            {
                if (_stepStartedHook != null) this._stepStartedHook();
            }
            base.ProcessCurrentStep();
        }

        override protected void ProcessPreSampleTick()
        {
            sampleCounter++;

            if (sampleCounter > samplesPerQuarter)
            {
                sampleCounter = 0;
                this.stepIndex++;
                this.stepChanged = true;
                if (this.stepIndex >= this.StepCount)
                {
                    if (_stepEndedHook != null) this._stepEndedHook();
                    this.stepIndex = 0;

                }
                else
                {
                    if (_stepChangedHook != null) this._stepChangedHook();
                }
            }
        }
    }
    /// <summary>
    /// Abstract wrapper for auditory stimuli generator
    /// 
    /// @author Nicolas Van Labeke
    /// </summary>
    public abstract class IFrequencySequencer
    {

        /// <summary>
        /// Instance of the sequencer to be used for the stimuli generation.
        /// </summary>
        protected SequencerExt _sequencer = null;

        /// <summary>
        /// Reference to the GUI media element.
        /// </summary>
        protected MediaElement _elt = null;

        /// <summary>
        /// Internal storage of voices for the sequencer. 
        /// 
        /// To be used for reset to factory settings.
        /// </summary>
        protected Dictionary<int, ISampleMaker> _intervalVoices = null;

        public abstract void ResetSequencer();


        void _sequencer__stepChangedHook()
        {
            Debug.WriteLine("SEQUENCER - NEXT STEP : " + this._sequencer.StepIndex);
        }

        void _sequencer__stepEndedHook()
        {
            Debug.WriteLine("SEQUENCER - LAST STEP ##### : " + this._sequencer.StepIndex);
        }

        void _sequencer__stepStartedHook()
        {
            Debug.WriteLine("SEQUENCER - STARTED ##### : " + this._sequencer.StepIndex);
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="elt">Reference to the media element of the GUI</param>
        protected IFrequencySequencer(MediaElement elt)
        {
            this._elt = elt;
            this._sequencer = new SequencerExt();
            this._sequencer._stepChangedHook += new SequencerExt.StepChanged(_sequencer__stepChangedHook);
            this._sequencer._stepEndedHook += new SequencerExt.StepEnded(_sequencer__stepEndedHook);
            this._sequencer._stepStartedHook += new SequencerExt.StepStarted(_sequencer__stepStartedHook);

            this._intervalVoices = new Dictionary<int, ISampleMaker>();

            SynthMediaStreamSource source = new SynthMediaStreamSource(44100, 2);
            source.SampleMaker = this._sequencer;

            this._sequencer.Tempo = 60 * 10; // 1 beat per 10th second
            this._elt.SetSource(source);
        }

        /// <summary>
        /// Initialise the voices of the sequencer.
        /// </summary>
        /// <param name="interIdx"></param>
        protected void setSequencer(int interIdx)
        {

            this._intervalVoices = new Dictionary<int, ISampleMaker>();
            int inc = 0;
            int intervalNb = 2;// interIdx;

            double[] inter = { 5000.0, 3000.0, 1000.0, 3000.0 };

            this._sequencer.StepCount = 19;
            this._sequencer.VoiceCount = 3;
            double percent = 10 / 100d;
            uint value = (uint)(15000 * percent);

            int[] pos = { 0, 2, 7, 2,0,19};

 
            for (inc = 0; inc < intervalNb; inc++)
            {
                //Pitch pitch = new Pitch(11, 3);
                IWaveForm form = new SineWaveForm();
                Oscillator osc1 = new Oscillator();
                Oscillator osc2 = new Oscillator();
                osc1.WaveForm = new SineWaveForm();
                osc2.WaveForm = new SineWaveForm();

                Voice voice = new Voice();
                voice.Attenuation = 0;
                voice.Oscillators.AddRange(new List<Oscillator>() { osc1});
                voice.Frequency = inter[inc];//pitch.Frequency;
                voice.Envelope.Attack = value;
                voice.Envelope.Decay = value;
                this._intervalVoices.Add(inc, voice);

                //this.sequencer.AddNote(voice, (3 + 2) * inc + 1, 3);
                this._sequencer.AddNote(voice, pos[2 * inc], pos[2 * inc + 1]);

            }

            if (false)
           {
                //Pitch pitch = new Pitch(11, 3);
               IWaveForm form = new SineWaveForm();
               Oscillator osc1 = new Oscillator();
               Oscillator osc2 = new Oscillator();
               osc1.WaveForm = new WhiteNoiseWaveForm();
               osc2.WaveForm = new WhiteNoiseWaveForm();

                Voice voice = new Voice();
                voice.Attenuation = -25;
                voice.Oscillators.AddRange(new List<Oscillator>() {osc1, osc2 });
                voice.Frequency = 3000;
                //voice.Envelope.Attack = value;
                //voice.Envelope.Decay = value;
                this._intervalVoices.Add(inc, voice);

                //this.sequencer.AddNote(voice, (3 + 2) * inc + 1, 3);
               this._sequencer.AddNote(voice, 0, 19);

            }

            
            this._elt.Stop();
            this._sequencer.StepIndex = -1;// this._sequencer.StepCount - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (_elt != null) _elt.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (_elt != null) _elt.Stop();
            this._sequencer.StepIndex = 0;// this._sequencer.StepCount - 1;
        }


    }

    /// <summary>
    /// Implementation of a 2-Interval stimuli generator
    /// @author Nicolas Van Labeke
    /// </summary>
    public class Frequency3IGenerator : IFrequencySequencer
    {
        public Frequency3IGenerator(MediaElement elt) : base(elt)
        {
            setSequencer(3);
            this._sequencer._stepEndedHook += new SequencerExt.StepEnded(_sequencer__stepEnded3IHook);
        }

        public override void ResetSequencer()
        {
            setSequencer(3);
        }

        public delegate void AttachExecuteDelegate();
        public void AttachExecute()
        {
            // here we can modify all the GUI elements; we’re now in the
            // main thread of the application
            if (this._elt != null) this._elt.Stop();
            this._sequencer.StepIndex = 0;// this._sequencer.StepCount - 1;
        }

        public void ThreadProc(object stateInfo)
        {
            if (this._elt != null) this._elt.Stop();
            this._sequencer.StepIndex = 0;// this._sequencer.StepCount - 1;

        }

        void _sequencer__stepEnded3IHook()
        {
            Debug.WriteLine("SEQUENCER - ENDED ## SHUT IT DOWN : " + this._sequencer.StepIndex);
            //this.Stop();
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc));
        }
    }

    /// <summary>
    /// Implementation of a 3-Interval stimuli generator
    /// @author Nicolas Van Labeke
    /// </summary>
    public class Frequency2IGenerator : IFrequencySequencer
    {
        public Frequency2IGenerator(MediaElement elt) : base(elt)
        {
            //this.sequencer.StepCount = (int)this.stepBox.Value;
            //this._sequencer.Reset();
        }

        public override void ResetSequencer()
        {
            //this.sequencer.StepCount = (int)this.stepBox.Value;
            //this._sequencer.Reset();
        }

   }
}
