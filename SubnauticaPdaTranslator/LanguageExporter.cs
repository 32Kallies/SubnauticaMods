using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SubnauticaPdaTranslator;

public class LanguageExporter
{
    public void ExportAll(string outputFileNameWithoutExtension)
    {
        var linesInLanguage = new Dictionary<string, (string, string)>();
        Dictionary<string, PDALog.EntryData> logs = PDALog.mapping;
        foreach (var log in logs)
        {
            var logData = log.Value;
            if (logData.icon == null || !logData.icon.name.ToLower().Contains("pda"))
                continue;

            var fmodAsset = logData.sound;
            if (fmodAsset == null || string.IsNullOrWhiteSpace(fmodAsset.path))
                continue;
            
            if (!Language.main.Contains(logData.key))
                continue;

            var translation = Language.main.Get(logData.key);
            if (string.IsNullOrWhiteSpace(translation))
                continue;
            
            linesInLanguage.Add(logData.key, (fmodAsset.path, translation));
        }
        var path = $"{outputFileNameWithoutExtension}.json";
        var json = JsonConvert.SerializeObject(linesInLanguage, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        });
        File.WriteAllText(path, json);
    }
}