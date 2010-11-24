using System;
using System.Collections.Generic;

namespace Kindohm.KSynth.Library
{
    public class Voice : AttenuatorBase, IVoice
    {
        public Voice()
        {
            this.Oscillators = new List<Oscillator>();
            this.Envelope = new Envelope();
            this.Envelope.Attack = 0;
            this.Envelope.Decay = 0;
            this.Envelope.Sustain = uint.MaxValue / 2;
        }

        public List<Oscillator> Oscillators { get; protected set; }
        public Envelope Envelope { get; protected set; }

        public StereoSample GetSample()
        {
            StereoSample finalSample = new StereoSample();
            if (this.Oscillators.Count > 0)
            {
                int multiplier = 65536 / Oscillators.Count;

                foreach (ISampleMaker input in Oscillators)
                {
                    StereoSample sample = input.GetSample();
                    finalSample.LeftSample += (short)(((multiplier * sample.LeftSample) >> 16) / this.Oscillators.Count);
                    finalSample.RightSample += (short)(((multiplier * sample.RightSample) >> 16) / this.Oscillators.Count);
                }
                finalSample = this.Envelope.ProcessSample(finalSample);
            }

            if (this.Attenuation < 0)
                return this.Attenuate(finalSample);
            else
                return finalSample;
        }

        public double Frequency
        {
            get
            {
                return this.Oscillators[0].Frequency;
            }
            set
            {
                foreach (Oscillator osc in this.Oscillators)
                    osc.Frequency = value;
            }
        }

        public double PitchBend
        {
            get
            {
                return this.Oscillators[0].PitchBend;
            }
            set
            {
                foreach (Oscillator osc in this.Oscillators)
                    osc.PitchBend = value;
            }
        }

        public void Reset()
        {
            foreach (Oscillator osc in this.Oscillators)
                osc.ResetPitchBend();
        }
    }
}
