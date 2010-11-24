using System;

namespace Kindohm.KSynth.Library
{
    public class TriangleWaveForm : IWaveForm
    {
        public short GetSample(ushort phaseAngle)
        {
            return (short)(phaseAngle < (ushort.MaxValue) ? 1 * short.MinValue + 2 * phaseAngle :
                                                                              3 * short.MaxValue - 2 * phaseAngle);
        }

    }
}
