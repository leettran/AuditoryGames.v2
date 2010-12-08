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

namespace AudioFramework
{
    /// <summary>
    /// Abstract wrapper for auditory stimuli generator
    /// 
    /// @author Nicolas Van Labeke
    /// </summary>
    public abstract class IFrequencySequencer
    {
        public delegate void StimuliStarted();
        public delegate void StimuliChanged();
        public delegate void StimuliStopped();

        public event StimuliStarted stimuliStarted = null;
        public event StimuliChanged stimuliChanged = null;
        public event StimuliStopped stimuliStopped = null;

        public Sequencer sequencer = null;
        protected Dictionary<int, ISampleMaker> intervalVoices = null;

        protected MediaElement _elt = null;

        public abstract void ResetSequencer();

        protected IFrequencySequencer(MediaElement elt)
        {
            this._elt = elt;
            this.sequencer = new Sequencer();
            this.intervalVoices = new Dictionary<int, ISampleMaker>();

            SynthMediaStreamSource source = new SynthMediaStreamSource(44100, 2);
            source.SampleMaker = this.sequencer;

            this.sequencer.Tempo = 600;
            this._elt.SetSource(source);
            this._elt.Stop();
            this.sequencer.StepIndex = this.sequencer.StepCount - 1;


        }

        protected void setSequencer(int interIdx)
        {

            this.intervalVoices = new Dictionary<int, ISampleMaker>();
            int inc = 0;
            int intervalNb = 4;// interIdx;

            double[] inter = { 5000.0, 3000.0, 5000.0, 3000.0 };

            this.sequencer.StepCount = 80*2;
            IWaveForm form = new SineWaveForm();

            double percent = 10 / 100d;
            uint value = (uint)(15000 * percent);

            /// 
            ///
            ///
            ///
            ///
            ///
            ///
            int[] pos = { 0, 10, 20, 10, 40 ,10 , 60 , 10 };

            for (inc = 0; inc < intervalNb; inc++)
            {
                //Pitch pitch = new Pitch(11, 3);
                Oscillator osc1 = new Oscillator();
                osc1.WaveForm = form;

                Voice voice = new Voice();
                voice.Attenuation = 0;
                voice.Oscillators.AddRange(new List<Oscillator>() { osc1 });
                voice.Frequency = inter[inc];//pitch.Frequency;
                voice.Envelope.Attack = value;
                voice.Envelope.Decay = value;
                this.intervalVoices.Add(inc, voice);

                //this.sequencer.AddNote(voice, (3 + 2) * inc + 1, 3);
                this.sequencer.AddNote(voice, pos[2*inc], pos[2*inc + 1]);

            }
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
        }

        public override void ResetSequencer()
        {
            setSequencer(3);
        }
    }

    public class Frequency2IGenerator : IFrequencySequencer
    {
        public Frequency2IGenerator(MediaElement elt) : base(elt)
        {
            //this.sequencer.StepCount = (int)this.stepBox.Value;
            this.sequencer.Reset();
        }

        public override void ResetSequencer()
        {
            //this.sequencer.StepCount = (int)this.stepBox.Value;
            this.sequencer.Reset();
        }

   }
}
