using System;

namespace Kindohm.KSynth.Library
{
    public class SineWaveForm : IWaveForm
    {
        const double factor = 2 * Math.PI / ushort.MaxValue;

        public short GetSample(ushort phaseAngle)
        {
            return (short)(short.MaxValue * Math.Sin(factor * phaseAngle));
        }

    }
}
