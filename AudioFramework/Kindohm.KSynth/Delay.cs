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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Kindohm.KSynth.Library
{
    public class Delay : AttenuatorBase
    {
        StereoSample[] samples;
        bool enabled;
        int length;
        int currentAddIndex;
        bool full = false;

        public Delay()
        {
            this.Enabled = false;
        }

        public int Length
        {
            get { return this.length; }
            set
            {
                this.length = value;
                this.ResetSamples();
            }
        }

        public bool Enabled {
            get { return enabled; }

            set
            {
                this.enabled = value;
                if (!this.enabled && this.samples != null)
                    this.ResetSamples();
            }
        }

        void ResetSamples()
        {
            this.samples = new StereoSample[this.length];
            this.currentAddIndex = 0;
            this.full = false;
        }
        
        public StereoSample ProcessSample(StereoSample sample)
        {

            if (!this.Enabled || this.Length == 0)
                return new StereoSample();

            this.currentAddIndex++;
            if (this.currentAddIndex >= this.length)
            {
                this.currentAddIndex = 0;
                this.full = true;
            }

            this.samples[currentAddIndex] = sample;

            if (this.full == true)
            {
                int sampleIndex = this.currentAddIndex == this.length - 1 ? 0 : this.currentAddIndex + 1;
                
                StereoSample delayedSample = samples[sampleIndex];
                delayedSample.LeftSample = (short)(delayedSample.LeftSample / 3);
                delayedSample.RightSample = (short)(delayedSample.RightSample / 3);
                //StereoSample newSample = new StereoSample();

                //int newLeftSample = sample.LeftSample + delayedSample.LeftSample / 3;
                //int newRightSample = sample.RightSample + delayedSample.RightSample / 3;

                //newSample.LeftSample = (short)newLeftSample;
                //newSample.RightSample = (short)newRightSample;
                return delayedSample;
            }

            return sample;
            
        }
    }
}
