using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Kindohm.KSynth.Library;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace TestSequencer
{
    public partial class MainPage : UserControl
    {
        Sequencer sequencer;
        Dictionary<int, ISampleMaker> intervalVoices;
        bool playing = false;
        
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

                void setSequencer(int interIdx)
        {
            this.intervalVoices = new Dictionary<int, ISampleMaker>();

            int inc = 0;
            int intervalNb = interIdx;

            double[] inter={5000.0,3000.0,5000.0};

            this.sequencer.StepCount = 15+5;
            IWaveForm form = new SineWaveForm();


            double percent = 5 / 100d;
            uint value = (uint)(15000 * percent);

            for (inc = 0; inc < intervalNb; inc++)
            {
                //Pitch pitch = new Pitch(11, 3);
                Oscillator osc1 = new Oscillator();
                osc1.WaveForm = form;

                Voice voice = new Voice();
                voice.Attenuation = 0;
                voice.Oscillators.AddRange(new List<Oscillator>() { osc1});
                voice.Frequency = inter[inc];//pitch.Frequency;
                voice.Envelope.Attack = value;
                voice.Envelope.Decay = value;
                this.intervalVoices.Add(inc, voice);

                this.sequencer.AddNote(voice, (3+2)*inc+1, 3);

            }

            SynthMediaStreamSource source = new SynthMediaStreamSource(44100, 2);
            source.SampleMaker = this.sequencer;
            this.sequencer.Tempo = 360;
            this.media.SetSource(source);
            this.media.Stop();
            this.sequencer.StepIndex = this.sequencer.StepCount - 1;
            //      
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.sequencer = new Sequencer();
            setSequencer(3);

        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.media == null) return; ;
            BitmapImage image = null;
            Uri uri = null;
            if (playing)
            {
                this.media.Stop();
                uri = new Uri("media/play.png", UriKind.Relative);
                this.playing = false;
                this.sequencer.StepIndex = this.sequencer.StepCount - 1;
            }
            else
            {
                this.media.Play();
                uri = new Uri("media/stop.png", UriKind.Relative);
                this.playing = true;
            }
            image = new BitmapImage(uri);
            this.playImage.Source = image;

        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (this.media == null) return; ;
            Button_Click(null,null);
            RadioButton rb = sender as RadioButton;
            Debug.WriteLine("You chose: " + rb.GroupName + ": " + rb.Name);
            this.sequencer.Reset();  
            setSequencer((rb.Name== "c3I")? 3 : 2);
        }
  
    }
}
