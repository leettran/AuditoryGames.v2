/*  Auditory Training Games in Silverlight
    Copyright (C) 2008-2012 Nicolas Van Labeke (LSRI, Nottingham University)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
