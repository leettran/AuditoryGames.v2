/*  Auditory Training Games in Silverlight
    Copyright (C) 2008-2012 Nicolas Van Labeke (LSRI, Nottingham University)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace Kindohm.KSynth.Library
{
    // Pitch.cs (c) Charles Petzold, 2009
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
