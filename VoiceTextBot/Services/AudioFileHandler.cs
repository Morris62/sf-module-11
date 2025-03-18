using Telegram.Bot;
using VoiceTextBot.Configuration;
using VoiceTextBot.Utilities;

namespace VoiceTextBot.Services;

public class AudioFileHandler(ITelegramBotClient botClient, AppSettings appSettings) : IFileHandler
{
    public async Task Download(string fileId, CancellationToken cancellationToken)
    {
        var inputAudioFilePath = Path.Combine(appSettings.DownloadsFolder,
            $"{appSettings.AudioFileName}.{appSettings.InputAudioFormat}");

        using (FileStream destinationStream = File.Create(inputAudioFilePath))
        {
            // Загружаем информацию о файле
            var file = await botClient.GetFileAsync(fileId, cancellationToken);
            if (file.FilePath == null)
                return;

            // Скачиваем файл
            await botClient.DownloadFile(file.FilePath, destinationStream, cancellationToken: cancellationToken);
        }
    }

    public string Process(string languageCode)
    {
        var inputAudioPath = Path.Combine(appSettings.DownloadsFolder,
            $"{appSettings.AudioFileName}.{appSettings.InputAudioFormat}");
        var outputAudioPath = Path.Combine(appSettings.DownloadsFolder,
            $"{appSettings.AudioFileName}.{appSettings.OutputAudioFormat}");

        Console.WriteLine("Начинаем конвертацию");
        AudioConverter.TryConvert(inputAudioPath, outputAudioPath);
        Console.WriteLine("Файл конвертирован");


        Console.WriteLine("Начинаем распознование");
        var speechText = SpeechDetector.Detect(outputAudioPath, 48000, languageCode);
        Console.WriteLine("Файл распознан");


        return speechText;
    }
}