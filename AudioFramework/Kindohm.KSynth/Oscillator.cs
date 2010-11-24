using System;
using System.Collections.Generic;

namespace Kindohm.KSynth.Library
{
    public class Oscillator : ISampleMaker
    {
        double frequency;
        double detune;
        double totalFrequency;
        uint phaseAngleIncrement;
        uint phaseAngle = 0;
        uint phaseShift;
        double internalFrequency;

        public Oscillator()
        {
            this.Pan = 0;
        }

        public uint PhaseShift
        {
            get { return this.phaseShift; }
            set { this.phaseShift = value; }
        }

        public double Detune
        {
            get
            {
                return this.detune;
            }
            set
            {
                this.detune = value;
                this.totalFrequency = this.detune + this.frequency;
                this.UpdatePhaseAngleIncrement();
            }
        }

        public double Frequency
        {
            set
            {
                this.internalFrequency = value;
                this.frequency = this.internalFrequency;
                this.totalFrequency = this.detune + this.frequency;
                this.UpdatePhaseAngleIncrement();
            }
            get
            {
                return internalFrequency;
            }
        }

        public short Pan { get; set; }

        void UpdatePhaseAngleIncrement()
        {
            phaseAngleIncrement = (uint)(this.totalFrequency * uint.MaxValue / 44100);
        }
        public void Reset()
        {
            this.phaseAngle = 0;
            this.frequency = this.internalFrequency;
        }

        public void ResetPitchBend()
        {
            this.frequency = this.internalFrequency;
        }

        public StereoSample GetSample()
        {
            ushort wholePhaseAngle = (ushort)((phaseAngle >> 16));
            short sample = this.WaveForm.GetSample(wholePhaseAngle);
            phaseAngle = phaseAngle + phaseAngleIncrement + this.phaseShift;

            StereoSample stereoSample = new StereoSample() { LeftSample = sample, RightSample = sample };

            if (this.Pan > 0)
            {
                //right. decrease left
                //double percent = this.Pan / short.MaxValue / this.Pan;
                double percent = this.Pan / short.MaxValue;
                stereoSample.LeftSample = (short)(stereoSample.LeftSample - (short)((double)stereoSample.LeftSample * percent));
            }
            else if (this.Pan < 0)
            {
                //left. decrease right
                //double percent = short.MinValue / this.Pan;
                double percent = this.Pan / short.MinValue;
                stereoSample.RightSample = (short)(stereoSample.RightSample - (short)((double)stereoSample.RightSample * (percent)));
            }

            this.Bend();
            return stereoSample;
        }

        public IWaveForm WaveForm { get; set; }

        public double PitchBend { get; set; }

        void Bend()
        {
            this.frequency += this.PitchBend;
            this.totalFrequency = this.detune + this.frequency;
            this.UpdatePhaseAngleIncrement();
        }
    }
}
