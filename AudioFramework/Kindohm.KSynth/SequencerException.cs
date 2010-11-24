using System;

namespace Kindohm.KSynth.Library
{
    public class SequencerException : Exception
    {
        public SequencerException() : base() { }
        public SequencerException(string message) : base(message) { }
        public SequencerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
