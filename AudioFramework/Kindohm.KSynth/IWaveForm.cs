using System;

namespace Kindohm.KSynth.Library
{
    public interface IWaveForm
    {
        short GetSample(ushort phaseAngle);
    }
}
