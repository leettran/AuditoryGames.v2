using System;
using System.Windows;
namespace Kindohm.KSynth.Library
{
    public class AttenuatorBase : DependencyObject
    {
        const int attentuationConstant = 65536;
        double attenuation = 0;        // in db
        int attenuationMultiplier = attentuationConstant;

        public double Attenuation
        {
            set
            {
                attenuation = value;
                attenuationMultiplier = (int)(attentuationConstant * Math.Pow(10, attenuation / 20.0));
            }
            get
            {
                return attenuation;
            }
        }

        protected StereoSample Attenuate(StereoSample sample)
        {
            sample.LeftSample = (short)((sample.LeftSample * attenuationMultiplier) >> 16);
            sample.RightSample = (short)((sample.RightSample * attenuationMultiplier) >> 16);
            return sample;
        }

    }
}
