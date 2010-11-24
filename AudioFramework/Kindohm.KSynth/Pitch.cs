// Pitch.cs (c) Charles Petzold, 2009

using System;

namespace Kindohm.KSynth.Library
{
    public class Pitch
    {
        static readonly double semitone = Math.Pow(2, 1.0 / 12);
        static readonly double middleC = 440 * Math.Pow(semitone, 3) / 2;

        public Note Note { get; protected set; }
        public int Octave { get; protected set; }
        public double Frequency { get; protected set; }

        public Pitch(Note note, int octave) 
        {
            Note = note;
            Octave = octave;
            Frequency = middleC * Math.Pow(semitone, (int)Note) * Math.Pow(2, Octave - 4); 
        }

        public Pitch(int noteValue, int octave):this((Note)noteValue, octave)
        {
        }
    }
}
