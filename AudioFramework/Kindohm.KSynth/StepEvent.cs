using System;
using System.Collections.Generic;

namespace Kindohm.KSynth.Library
{
    public class StepEvent
    {
        public StepEvent()
        {
            this.VoiceNotes = new Dictionary<ISampleMaker, VoiceNote>();
        }

        public int Step { get; set; }

        public Dictionary<ISampleMaker, VoiceNote> VoiceNotes { get; protected set; }
    }
}
