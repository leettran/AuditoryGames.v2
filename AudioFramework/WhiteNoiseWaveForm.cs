using System;

namespace Kindohm.KSynth.Library
{
    /// <summary>
    /// @author Nicolas Van Labeke &lt; http://www.lsri.nottingham.ac.uk/nvl/ &gt;
    /// </summary>
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
