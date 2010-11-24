
namespace Kindohm.KSynth.Library
{
    public interface IVoice : ISampleMaker
    {
        Envelope Envelope { get; }
    }
}
