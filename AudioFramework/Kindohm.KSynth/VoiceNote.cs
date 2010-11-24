using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Kindohm.KSynth.Library
{
    public class VoiceNote
    {
        public ISampleMaker Voice { get; set; }
        public int Duration { get; set; }
        public int Elapsed { get; set; }
    }
}
