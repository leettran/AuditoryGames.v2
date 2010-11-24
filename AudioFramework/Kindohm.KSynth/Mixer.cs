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
