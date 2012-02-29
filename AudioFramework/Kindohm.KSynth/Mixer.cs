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
    public class Mixer : AttenuatorBase, ISampleMaker
    {
        public bool PassThrough { get; set; }

        public IList<ISampleMaker> Inputs { get; protected set; }

        public Mixer()
        {
            this.Inputs = new List<ISampleMaker>();
        }

        public StereoSample GetSample()
        {
            StereoSample finalSample = new StereoSample();
            if (Inputs.Count > 0)
            {
                int multiplier = 1;
                if (!this.PassThrough)
                    multiplier = 65536 / Inputs.Count;
                else
                    multiplier = 65536;

                foreach (ISampleMaker input in Inputs)
                {
                    StereoSample sample = input.GetSample();
                    finalSample.LeftSample += (short)((multiplier * sample.LeftSample) >> 16);
                    finalSample.RightSample += (short)((multiplier * sample.RightSample) >> 16);
                    finalSample = this.Attenuate(finalSample);
                }
            }

            return finalSample;
        }

        public void Reset() { }

    }
}
