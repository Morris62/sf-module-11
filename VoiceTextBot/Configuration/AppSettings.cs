namespace VoiceTextBot.Configuration;

public class AppSettings
{
    public string BotToken { get; set; }
    public string DownloadsFolder { get; set; }
    public string AudioFileName { get; set; }
    public string InputAudioFormat { get; set; }
    public string OutputAudioFormat { get; set; }
    public float InputBitrate { get; set; }
}