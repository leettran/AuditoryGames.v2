using System;

namespace Kindohm.KSynth.Library
{
    public class WhiteNoiseWaveForm : IWaveForm
    {
        Random _rd = new Random();

        public short GetSample(ushort phaseAngle)
        {
           // return phaseAngle < (ushort)short.MaxValue ? short.MinValue : short.MaxValue;

            return (short)_rd.Next((int)short.MinValue, (int)short.MaxValue);
        }

    }
}
