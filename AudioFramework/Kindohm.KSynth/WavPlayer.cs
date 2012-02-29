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
using System.Diagnostics;

namespace Kindohm.KSynth.Library
{
    public class WavPlayer : AttenuatorBase, ISampleMaker
    {
        int index;

        public byte[] WaveData { get; set; }

        public StereoSample GetSample()
        {
            if (index >= (this.WaveData.Length - 200))
            {
                return new StereoSample();
            }
            StereoSample sample = new StereoSample();
            byte[] left = new byte[2] { WaveData[index], WaveData[index + 1] };
            sample.LeftSample = BitConverter.ToInt16(left, 0);

            if (index + 2 < WaveData.Length && index + 3 < WaveData.Length)
            {
                byte[] right = new byte[2] { WaveData[index + 2], WaveData[index + 3] };

                sample.RightSample = BitConverter.ToInt16(left, 0);
            }

            index = index + 4;
            if (this.Attenuation < 0)
                return this.Attenuate(sample);
            else return sample;
        }

        public void Reset()
        {
            this.index = 0;
        }
    }
}
