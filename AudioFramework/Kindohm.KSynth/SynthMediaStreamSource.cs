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
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Media;

namespace Kindohm.KSynth.Library
{
    //
    
    /// <summary>
    /// .
    /// 
    /// much of this code borrowed from Charles Petzold
    /// @see http://charlespetzold.com/blog/2009/07/Simple-Electronic-Music-Sequencer-for-Silverlight.html
    /// </summary>
    public class SynthMediaStreamSource : MediaStreamSource
    {
        const int BitsPerSample = 16;
        MediaStreamDescription mediaStreamDescription;
        long startPosition;
        long currentPosition;
        long currentTimeStamp;
        int sampleRate;
        int channelCount;
        int byteRate;
        short blockAlign;
        MemoryStream memoryStream;
        Dictionary<MediaSampleAttributeKeys, string> emptySampleDict = 
            new Dictionary<MediaSampleAttributeKeys, string>();

        public ISampleMaker SampleMaker { get; set; }
        public List<object> Tags { get; protected set; }

        public SynthMediaStreamSource(int sampleRate, int channelCount)
        {
            this.Tags = new List<object>();
            this.sampleRate = sampleRate;
            this.channelCount = channelCount;
            byteRate = sampleRate * channelCount * BitsPerSample / 8;
            blockAlign = (short)(channelCount * (BitsPerSample / 8));
            memoryStream = new MemoryStream();
            //this.AudioBufferLength = 500;
        }

        public SynthMediaStreamSource(int sampleRate, int channelCount, int buffer) : this(sampleRate,channelCount)
        {
            this.AudioBufferLength = buffer;
        }


        protected override void OpenMediaAsync()
        {
            startPosition = currentPosition = 0;

            Dictionary<MediaStreamAttributeKeys, string> streamAttributes = new Dictionary<MediaStreamAttributeKeys, string>();
            Dictionary<MediaSourceAttributesKeys, string> sourceAttributes = new Dictionary<MediaSourceAttributesKeys, string>();
            List<MediaStreamDescription> availableStreams = new List<MediaStreamDescription>();

            string format = "";
            format += ToLittleEndianString(string.Format("{0:X4}", 1));      // indicates PCM
            format += ToLittleEndianString(string.Format("{0:X4}", channelCount));
            format += ToLittleEndianString(string.Format("{0:X8}", sampleRate));
            format += ToLittleEndianString(string.Format("{0:X8}", byteRate));
            format += ToLittleEndianString(string.Format("{0:X4}", blockAlign));
            format += ToLittleEndianString(string.Format("{0:X4}", BitsPerSample));
            format += ToLittleEndianString(string.Format("{0:X4}", 0));

            streamAttributes[MediaStreamAttributeKeys.CodecPrivateData] = format;
            mediaStreamDescription = new MediaStreamDescription(MediaStreamType.Audio, streamAttributes);
            availableStreams.Add(mediaStreamDescription);
            sourceAttributes[MediaSourceAttributesKeys.Duration] = "0";
            sourceAttributes[MediaSourceAttributesKeys.CanSeek] = "false";
            ReportOpenMediaCompleted(sourceAttributes, availableStreams);
        }

        protected override void GetSampleAsync(MediaStreamType mediaStreamType)
        {
            int numSamples = 512;
            int bufferByteCount = channelCount * BitsPerSample / 8 * numSamples;

            for (int i = 0; i < numSamples; i++)
            {
                StereoSample stereoSample = this.SampleMaker.GetSample();
                memoryStream.WriteByte((byte)(stereoSample.LeftSample & 0xFF));
                memoryStream.WriteByte((byte)(stereoSample.LeftSample >> 8));
                memoryStream.WriteByte((byte)(stereoSample.RightSample & 0xFF));
                memoryStream.WriteByte((byte)(stereoSample.RightSample >> 8));
            }

            // Send out the next sample
            MediaStreamSample mediaStreamSample =
                new MediaStreamSample(mediaStreamDescription, memoryStream, currentPosition,
                                      bufferByteCount, currentTimeStamp, emptySampleDict);

            // Move timestamp and position forward
            currentTimeStamp += bufferByteCount * 10000000L / byteRate;
            currentPosition += bufferByteCount;

            ReportGetSampleCompleted(mediaStreamSample);
        }

        protected override void SeekAsync(long seekToTime)
        {
            ReportSeekCompleted(seekToTime);
        }

        protected override void CloseMedia()
        {
            startPosition = currentPosition = 0;
            mediaStreamDescription = null;
        }

        protected override void GetDiagnosticAsync(MediaStreamSourceDiagnosticKind diagnosticKind)
        {
            throw new NotImplementedException();
        }

        protected override void SwitchMediaStreamAsync(MediaStreamDescription mediaStreamDescription)
        {
            throw new NotImplementedException();
        }

        string ToLittleEndianString(string bigEndianString)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bigEndianString.Length; i += 2)
                builder.Insert(0, bigEndianString.Substring(i, 2));

            return builder.ToString();
        }

    }
}
