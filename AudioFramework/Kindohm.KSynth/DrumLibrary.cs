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
using System.Collections.ObjectModel;

namespace Kindohm.KSynth.Library
{
    public class DrumLibrary
    {
        const string LibraryRoot = "drums/";

        public string Name { get; set; }
        public string KickPath { get; set; }
        public string SnarePath { get; set; }
        public string HatPath { get; set; }
        public string TicPath { get; set; }
        public string TocPath { get; set; }

        public static ObservableCollection<DrumLibrary> GetDrumLibraries()
        {
            ObservableCollection<DrumLibrary> items = new ObservableCollection<DrumLibrary>();

            //akuma
            DrumLibrary akuma = new DrumLibrary() { Name = "Akuma" };
            akuma.KickPath = DrumLibrary.LibraryRoot + "akumakick.wav";
            akuma.SnarePath = DrumLibrary.LibraryRoot + "akumasnare.wav";
            akuma.HatPath = DrumLibrary.LibraryRoot + "akumahat.wav";
            akuma.TicPath = DrumLibrary.LibraryRoot + "akumatic.wav";
            akuma.TocPath = DrumLibrary.LibraryRoot + "akumatoc.wav";

            //amen
            DrumLibrary amen = new DrumLibrary() { Name = "Amen" };
            amen.KickPath = DrumLibrary.LibraryRoot + "amenkick.wav";
            amen.SnarePath = DrumLibrary.LibraryRoot + "amensnare.wav";
            amen.HatPath = DrumLibrary.LibraryRoot + "amenride.wav";
            amen.TicPath = DrumLibrary.LibraryRoot + "amentic.wav";
            amen.TocPath = DrumLibrary.LibraryRoot + "amentoc.wav";

            //kindohm
            DrumLibrary kindohm = new DrumLibrary() { Name = "Kindohm" };
            kindohm.KickPath = DrumLibrary.LibraryRoot + "kindohmkick.wav";
            kindohm.SnarePath = DrumLibrary.LibraryRoot + "kindohmsnare.wav";
            kindohm.HatPath = DrumLibrary.LibraryRoot + "kindohmhat.wav";
            kindohm.TicPath = DrumLibrary.LibraryRoot + "kindohmtic.wav";
            kindohm.TocPath = DrumLibrary.LibraryRoot + "kindohmtoc.wav";

            //science
            DrumLibrary science = new DrumLibrary() { Name = "Science" };
            science.KickPath = DrumLibrary.LibraryRoot + "sciencekick.wav";
            science.SnarePath = DrumLibrary.LibraryRoot + "sciencesnare.wav";
            science.HatPath = DrumLibrary.LibraryRoot + "sciencehat.wav";
            science.TicPath = DrumLibrary.LibraryRoot + "sciencetic.wav";
            science.TocPath = DrumLibrary.LibraryRoot + "sciencetoc.wav";

            items.Add(akuma);
            items.Add(amen);
            items.Add(kindohm);
            items.Add(science);

            return items;
        }
    }
}
