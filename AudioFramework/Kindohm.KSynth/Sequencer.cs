using System;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;

namespace Kindohm.KSynth.Library
{
    /// <summary>
    /// Implementation of a basic sequencer.
    /// @author Mike Hodnick &lt; http://kindohm.com/ &gt;
    /// @author Nicolas Van Labeke &lt; http://www.lsri.nottingham.ac.uk/nvl/ &gt;
    /// @version 1.0 - changed access to methods ProcessCurrentStep(), ProcessPreSampleTick() and ProcessPostSampleTick() to allow override.  
    /// </summary>
    public class Sequencer : ISampleMaker
    {
        protected int tempo;
        protected int sampleCounter;
        protected int samplesPerQuarter;
        protected int stepCount;
        protected int stepIndex;
        protected int elapsedSeconds = 0;
        protected int elapsedSampleTicks = 0;
        protected bool stepChanged;

        public Dictionary<int, StepEvent> stepEvents;
        public List<VoiceNote> voicesInPlay;

        public Sequencer()
        {
            this.Tempo = 2800;
            this.StepCount = 64;
            this.stepEvents = new Dictionary<int, StepEvent>();
            this.voicesInPlay = new List<VoiceNote>();
            this.VoiceCount = 1;
            this.Delay = new Delay();
            this.Delay.Length = 0;
        }

        public int VoiceCount { get; set; }
        public int StepIndex { get { return this.stepIndex; } set { this.stepIndex = value; } }
        public Delay Delay { get; protected set; }

        public int StepCount
        {
            get { return this.stepCount; }
            set
            {
                this.stepCount = value;
            }
        }

        public int Tempo
        {
            set
            {
                this.tempo = value;
                this.samplesPerQuarter = 44100 * 60 / this.tempo;
            }
            get
            {
                return tempo;
            }
        }

        public void Reset()
        {

            this.stepEvents.Clear();
            this.voicesInPlay.Clear();
        }


        public void AddNote(ISampleMaker voice, int step, int duration)
        {
            StepEvent stepEvent = null;
            if (this.stepEvents.ContainsKey(step))
                stepEvent = this.stepEvents[step];
            else
            {
                stepEvent = new StepEvent();
                this.stepEvents.Add(step, stepEvent);
            }
            stepEvent.Step = step;
            VoiceNote note = new VoiceNote();
            note.Duration = duration;
            note.Voice = voice;
            stepEvent.VoiceNotes.Add(voice, note);
        }

        public void DeleteNote(ISampleMaker voice, int step)
        {
            StepEvent stepEvent = null;
            if (this.stepEvents.ContainsKey(step))
                stepEvent = this.stepEvents[step];
            else
                return;

            if (stepEvent.VoiceNotes.ContainsKey(voice))
            {
                bool f = stepEvent.VoiceNotes.Remove(voice);
                this.stepEvents.Remove(step);
                Debug.WriteLine(f);
                this.stepChanged = true;
            }
        }

        public void ModifyNote(ISampleMaker voice, int step, int newDuration)
        {
            StepEvent stepEvent = null;
            if (this.stepEvents.ContainsKey(step))
            {
                stepEvent = this.stepEvents[step];
                stepEvent.VoiceNotes[voice].Duration = newDuration;
            }
            else
                throw new SequencerException("Step not found at position '" + step.ToString() + "' to modify.");
        }

        public void ModifyNote(ISampleMaker voice, ISampleMaker newVoice, int step)
        {
            StepEvent stepEvent = null;
            if (this.stepEvents.ContainsKey(step))
            {
                stepEvent = this.stepEvents[step];
                if (stepEvent.VoiceNotes.ContainsKey(voice))
                {
                    int duration = stepEvent.VoiceNotes[voice].Duration;
                    stepEvent.VoiceNotes.Remove(voice);
                    this.AddNote(newVoice, step, duration);
                }
            }
        }

        public StereoSample GetSample()
        {
            this.ProcessPreSampleTick();

            if (this.stepChanged)
            {
                this.ProcessCurrentStep();
                this.stepChanged = false;
            }

            StereoSample sample = new StereoSample();
            StereoSample delayedSample = new StereoSample();
            if (this.voicesInPlay.Count > 0)
            {
                int left = 0;
                int right = 0;
                try
                {
                    foreach (VoiceNote item in this.voicesInPlay)
                    {
                        StereoSample voiceSample = item.Voice.GetSample();
                        left += voiceSample.LeftSample;
                        right += voiceSample.RightSample;
                        if (item.Voice is WavPlayer == false)
                        {
                            delayedSample.LeftSample += voiceSample.LeftSample;
                            delayedSample.RightSample += voiceSample.RightSample;
                        }
                    }

                }
                catch (Exception)
                {
                    
                   // throw;
                }
                sample.LeftSample = (short)left;
                sample.RightSample = (short)right;
            }

            delayedSample = this.Delay.ProcessSample(delayedSample);

            StereoSample final = new StereoSample()
            {
                LeftSample = (short)(delayedSample.LeftSample + sample.LeftSample),
                RightSample = (short)(delayedSample.RightSample + sample.RightSample)
            };

            this.ProcessPostSampleTick();

            return final;

        }

        virtual protected void ProcessCurrentStep()
        {
            if (this.stepIndex == 0)
            {
                for (int i = this.voicesInPlay.Count - 1; i >= 0; i--)
                {
                    VoiceNote note = this.voicesInPlay[i];
                    note.Elapsed = 0;
                    note.Voice.Reset();
                    if (note.Voice is IVoice)
                        ((IVoice)note.Voice).Envelope.Reset();
                    this.voicesInPlay.RemoveAt(i);
                }
            }

            //Increment duration of voices in play
            foreach (VoiceNote item in this.voicesInPlay)
            {
                item.Elapsed++;
            }

            //remove voices whose duration has completed
            for (int i = this.voicesInPlay.Count - 1; i >= 0; i--)
            {
                VoiceNote note = this.voicesInPlay[i];

                if (note.Voice is IVoice)
                {
                    if (note.Elapsed >= note.Duration)
                    {
                        IVoice voice = (IVoice)note.Voice;
                        if (!voice.Envelope.Releasing)
                            voice.Envelope.Release();
                        else if (!voice.Envelope.Active)
                        {
                            this.voicesInPlay.RemoveAt(i);
                            note.Elapsed = 0;
                            note.Voice.Reset();
                            voice.Envelope.Reset();
                        }
                    }
                }
                else
                {
                    if (note.Elapsed >= note.Duration)
                    {
                        this.voicesInPlay.RemoveAt(i);
                        note.Elapsed = 0;
                        note.Voice.Reset();
                        if (note.Voice is IVoice)
                            ((IVoice)note.Voice).Envelope.Reset();
                    }
                }

                if ((note.Voice is IVoice == false && note.Elapsed >= note.Duration) ||
                    (note.Voice is IVoice && note.Elapsed >= note.Duration && ((IVoice)note.Voice).Envelope.Active == false))
                {
                    this.voicesInPlay.RemoveAt(i);
                    note.Elapsed = 0;
                    note.Voice.Reset();
                    if (note.Voice is IVoice)
                        ((IVoice)note.Voice).Envelope.Reset();
                }
            }

            //add new notes on this step
            if (this.stepEvents.ContainsKey(this.stepIndex))
            {
                StepEvent stepEvent = this.stepEvents[this.stepIndex];
                foreach (ISampleMaker key in stepEvent.VoiceNotes.Keys)
                {
                    this.voicesInPlay.Add(stepEvent.VoiceNotes[key]);
                }
            }
        }

        virtual protected void ProcessPreSampleTick()
        {
            sampleCounter++;

            if (sampleCounter > samplesPerQuarter)
            {
                sampleCounter = 0;
                this.stepIndex++;
                this.stepChanged = true;
                if (this.stepIndex >= this.StepCount)
                    this.stepIndex = 0;
            }
        }

        virtual protected void ProcessPostSampleTick()
        {
            if (++elapsedSampleTicks == 44100)
            {
                elapsedSeconds++;
                elapsedSampleTicks = 0;
            }
        }

    }
}
