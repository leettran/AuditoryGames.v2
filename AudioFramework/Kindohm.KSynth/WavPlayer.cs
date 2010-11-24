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
