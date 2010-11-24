using System;

namespace Kindohm.KSynth.Library
{
    public class SawWaveForm : IWaveForm
    {
        public short GetSample(ushort phaseAngle)
        {
            return (short)(short.MinValue + phaseAngle);
        }

    }
}
