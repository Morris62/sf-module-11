using FFMpegCore;
using VoiceTextBot.Extentions;

namespace VoiceTextBot.Utilities;

public static class AudioConverter
{
    public static void TryConvert(string inputFile, string outputFile)
    {
        GlobalFFOptions.Configure(options =>
            options.BinaryFolder = Path.Combine(DirectoryExtention.GetSolutionRoot(), "ffmpeg"));

        FFMpegArguments
            .FromFileInput(inputFile)
            .OutputToFile(outputFile, true, options => options
                .WithFastStart())
            .ProcessSynchronously();
    }
}