using System;

namespace Kindohm.KSynth.Library
{
    public class SquareWaveForm : IWaveForm
    {
        public short GetSample(ushort phaseAngle)
        {
            return phaseAngle < (ushort)short.MaxValue ? short.MinValue : short.MaxValue;
        }

    }
}
