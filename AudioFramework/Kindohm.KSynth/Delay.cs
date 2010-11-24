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
