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
