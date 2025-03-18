using System.Text;
using Newtonsoft.Json.Linq;
using VoiceTextBot.Extentions;
using Vosk;

namespace VoiceTextBot.Utilities;

public static class SpeechDetector
{
    public static string Detect(string audioPath, float inputBitrate, string languageCode)
    {
        Vosk.Vosk.SetLogLevel(-1);
        var modelPath = Path.Combine(DirectoryExtention.GetSolutionRoot(), $"Speech-models",
            $"vosk-model-small-{languageCode.ToLower()}");
        Model model = new(modelPath);

        return GetWords(model, audioPath, inputBitrate);
    }

    public static string GetWords(Model model, string audioPath, float inputBitrate)
    {
        var rec = new VoskRecognizer(model, inputBitrate);
        rec.SetMaxAlternatives(0);
        rec.SetWords(true);

        var textBuffer = new StringBuilder();

        using (Stream source = File.OpenRead(audioPath))
        {
            var buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (rec.AcceptWaveform(buffer, bytesRead))
                {
                    var sentenceJson = rec.Result();
                    var sentenceObj = JObject.Parse(sentenceJson);
                    var sentence = sentenceObj["text"].ToString();

                    textBuffer.Append(StringExtention.UppercaseFirst(sentence));
                    textBuffer.Append(". ");
                }
            }
        }

        var finalSentence = rec.FinalResult();
        var finalSentenceObj = JObject.Parse(finalSentence);
        textBuffer.Append(finalSentenceObj["text"].ToString());

        return textBuffer.ToString();
    }
}