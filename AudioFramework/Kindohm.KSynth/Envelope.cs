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
using System.Diagnostics;

namespace Kindohm.KSynth.Library
{
    public class Envelope : AttenuatorBase
    {
        uint counter = 0;
        uint attack;
        uint decay;
        double attackSlope = 0;
        double decaySlope = 0;
        const int MinAttenuation = 50;
        uint releaseCounter = 0;

        public Envelope()
        {
            this.Attack = 0;
            this.Sustain = uint.MaxValue;
            this.Decay = 0;
            this.Active = true;
        }

        public bool Releasing { get; protected set; }
        public bool Active { get; protected set; }

        public void Release()
        {
            this.Releasing = true;
        }

        public uint Attack
        {
            get { return this.attack; }
            set
            {
                this.attack = value;
                this.attackSlope = MinAttenuation / (double)this.attack;
            }
        }

        public uint Decay
        {
            get { return this.decay; }
            set
            {
                this.decay = value;
                this.decaySlope = -MinAttenuation / (double)
                    ((this.decay + this.Sustain + this.attack) - (this.attack + this.Sustain));
            }
        }

        public uint Sustain { get; set; }

        public void Reset()
        {
            counter = 0;
            this.releaseCounter = 0;
            this.Active = true;
            this.Releasing = false;
        }

        public StereoSample ProcessSample(StereoSample sample)
        {
            if (!this.Active)
            {
                return new StereoSample();
            }

            if (this.Releasing)
            {
                this.releaseCounter++;
                return this.ProcessRelease(sample);
            }
            else
            {
                counter++;

                if (counter < this.Attack)
                {
                    return this.ProcessAttack(sample);
                }
                else
                    return sample;
            }


            //else if (counter > (this.Attack + this.Sustain))
            //{
            //    return this.ProcessRelease(sample);
            //}
            //return sample;
        }

        StereoSample ProcessAttack(StereoSample sample)
        {
            //y = mx + b
            this.Attenuation = (int)((double)counter * this.attackSlope) - MinAttenuation;
            return this.Attenuate(sample);
        }

        StereoSample ProcessRelease(StereoSample sample)
        {
            //y = mx + b
            this.Attenuation = (int)((double)this.releaseCounter * this.decaySlope);
            if (this.Attenuation < -50)
            {
                this.Active = false;
                this.Attenuation = -100;
            }
            return this.Attenuate(sample);
        }
    }
}
